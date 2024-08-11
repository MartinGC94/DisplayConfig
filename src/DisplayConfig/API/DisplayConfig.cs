using MartinGC94.DisplayConfig.Native.Enums;
using MartinGC94.DisplayConfig.Native;
using MartinGC94.DisplayConfig.Native.Structs;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace MartinGC94.DisplayConfig.API
{
    public sealed class DisplayConfig
    {
        public DISPLAYCONFIG_PATH_INFO[] PathArray { get; set; }
        public DISPLAYCONFIG_MODE_INFO[] ModeArray { get; set; }
        public DisplayConfigFlags Flags { get; set; }
        public DISPLAYCONFIG_TOPOLOGY_ID TopologyID { get; set; }
        /// <summary>
        /// Contains indexes to display paths that are, or can be enabled in <see cref="PathArray"/>
        /// The indexes are sorted by <see cref="SortPathIndexes(List{int}, DISPLAYCONFIG_PATH_INFO[], out DISPLAYCONFIG_TARGET_DEVICE_NAME[])"/>
        /// where the goal is to match the the order assigned to displays in the Settings app as closely as possible.
        /// Meaning that the first index should point to the path that connects to Display 1 in the settings app.
        /// </summary>
        public int[] AvailablePathIndexes { get; set; }
        /// <summary>
        /// Contains device name info of the connected displays. The order is the same as <see cref="AvailablePathIndexes"/>
        /// </summary>
        public DISPLAYCONFIG_TARGET_DEVICE_NAME[] AvailablePathNames { get; set; }

        internal const uint DISPLAYCONFIG_PATH_MODE_IDX_INVALID = 0xffffffff;

        public static DisplayConfig GetConfig(DisplayConfigFlags flags)
        {
            DISPLAYCONFIG_PATH_INFO[] pathArray;
            uint pathArraySize;
            DISPLAYCONFIG_MODE_INFO[] modeArray;
            uint modeArraySize;
            DISPLAYCONFIG_TOPOLOGY_ID topology;
            do
            {
                ReturnCode code = NativeMethods.GetDisplayConfigBufferSizes(flags, out pathArraySize, out modeArraySize);
                if (code != ReturnCode.ERROR_SUCCESS)
                {
                    throw new Win32Exception((int)code);
                }

                pathArray = new DISPLAYCONFIG_PATH_INFO[pathArraySize];
                modeArray = new DISPLAYCONFIG_MODE_INFO[modeArraySize];
                if (flags.HasFlag(DisplayConfigFlags.QDC_DATABASE_CURRENT))
                {
                    code = NativeMethods.QueryDisplayConfig(flags, ref pathArraySize, pathArray, ref modeArraySize, modeArray, out topology);
                }
                else
                {
                    code = NativeMethods.QueryDisplayConfig(flags, ref pathArraySize, pathArray, ref modeArraySize, modeArray, IntPtr.Zero);
                    topology = DISPLAYCONFIG_TOPOLOGY_ID.None;
                }

                if (code == ReturnCode.ERROR_SUCCESS)
                {
                    break;
                }

                if (code != ReturnCode.ERROR_INSUFFICIENT_BUFFER)
                {
                    throw new Win32Exception((int)code);
                }
            } while (true);

            Array.Resize(ref pathArray, (int)pathArraySize);
            Array.Resize(ref modeArray, (int)modeArraySize);
            List<int> unsortedIndexes = GetAvailablePathIndexes(pathArray);
            int[] availablePathIndexes = SortPathIndexes(unsortedIndexes, pathArray, out DISPLAYCONFIG_TARGET_DEVICE_NAME[] targetDeviceInfo);

            return new DisplayConfig()
            {
                PathArray = pathArray,
                ModeArray = modeArray,
                Flags = flags,
                TopologyID = topology,
                AvailablePathIndexes = availablePathIndexes,
                AvailablePathNames = targetDeviceInfo
            };
        }

        private static List<int> GetAvailablePathIndexes(DISPLAYCONFIG_PATH_INFO[] pathArray)
        {
            var result = new List<int>();
            var idTable = new Dictionary<LUID, HashSet<uint>>();
            for (int i = 0; i < pathArray.Length; i++)
            {
                DISPLAYCONFIG_PATH_INFO path = pathArray[i];
                if (!path.targetInfo.targetAvailable)
                {
                    continue;
                }

                if (idTable.TryGetValue(path.targetInfo.adapterId, out HashSet<uint> idSet))
                {
                    if (idSet.Add(path.targetInfo.id))
                    {
                        result.Add(i);
                    }
                }
                else
                {
                    var newSet = new HashSet<uint>
                    {
                        path.targetInfo.id
                    };
                    idTable.Add(path.targetInfo.adapterId, newSet);

                    result.Add(i);
                }
            }

            return result;
        }

        private List<int> GetActivePathIndexes()
        {
            var result = new List<int>();
            foreach (int index in AvailablePathIndexes)
            {
                if (PathArray[index].flags.HasFlag(PathInfoFlags.DISPLAYCONFIG_PATH_ACTIVE))
                {
                    result.Add(index);
                }
            }

            return result;
        }

        /// <summary>
        /// Sorts the discovered path indexes, ideally it would match the display settings in Windows but they are undocumented.
        /// The next best thing is to sort by output tech and connectorInstance (which should increment by 1 for each display with the same output tech)
        /// </summary>
        private static int[] SortPathIndexes(List<int> indexes, DISPLAYCONFIG_PATH_INFO[] pathArray, out DISPLAYCONFIG_TARGET_DEVICE_NAME[] targetNameInfo)
        {
            var nameInfoTable = new Dictionary<int, DISPLAYCONFIG_TARGET_DEVICE_NAME>(indexes.Count);
            var adapterNameTable = new Dictionary<LUID, string>();
            foreach (int pathIndex in indexes)
            {
                DISPLAYCONFIG_PATH_TARGET_INFO targetInfo = pathArray[pathIndex].targetInfo;
                DISPLAYCONFIG_TARGET_DEVICE_NAME deviceNameInfo = DisplayTargetInfo.GetTargetDeviceName(targetInfo.adapterId, targetInfo.id);
                if (!adapterNameTable.ContainsKey(targetInfo.adapterId))
                {
                    adapterNameTable.Add(targetInfo.adapterId, AdapterName.GetAdapterName(targetInfo.adapterId));
                }
                nameInfoTable.Add(pathIndex, deviceNameInfo);
            }

            int[] result = indexes.OrderBy(i => adapterNameTable[pathArray[i].targetInfo.adapterId]).ThenBy(i =>
            {
                // A lower number means a higher priority.
                // From personal observation it seems like Windows prioritizes like this:
                // 1: internal display. 2: PC specific connectors like DVI/Displayport. 3: HDMI/others.
                uint priority;
                switch (pathArray[i].targetInfo.outputTechnology)
                {
                    case DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_INTERNAL:
                        priority = 50;
                        break;

                    case DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_DISPLAYPORT_EMBEDDED:
                        priority = 100;
                        break;

                    case DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_DVI:
                        priority = 150;
                        break;

                    case DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_DISPLAYPORT_EXTERNAL:
                        priority = 200;
                        break;

                    case DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_HDMI:
                        priority = 250;
                        break;

                    default:
                        priority = 300;
                        break;
                }

                priority += nameInfoTable[i].connectorInstance;

                return priority;
            }).ToArray();
            targetNameInfo = new DISPLAYCONFIG_TARGET_DEVICE_NAME[result.Length];
            for (int i = 0; i < result.Length; i++)
            {
                targetNameInfo[i] = nameInfoTable[result[i]];
            }

            return result;
        }

        public void ApplyConfig(SetDisplayConfigFlags flags)
        {
            uint modeArrayLength = ModeArray is null
                ? 0
                : (uint)ModeArray.Length;
            ReturnCode result = NativeMethods.SetDisplayConfig((uint)PathArray.Length, PathArray, modeArrayLength, ModeArray, flags);

            if (result != ReturnCode.ERROR_SUCCESS)
            {
                throw new Win32Exception((int)result);
            }
        }

        internal int GetDisplayIndex(uint displayId)
        {
            // The idea here is that users provide a 1-based display ID that usually matches the screen numbers shown in the Settings app
            // The AvailablePathIndexes array order should match the order in the Settings app,
            // but only if the QDC_ALL_PATHS flag was set, or if all connected displays are active.
            try
            {
                return AvailablePathIndexes[displayId - 1];
            }
            catch (IndexOutOfRangeException e)
            {
                throw new ArgumentException($"Failed to find display with id {displayId}", e);
            }
        }

        internal uint GetDisplayId(int index)
        {
            return (uint)Array.IndexOf(AvailablePathIndexes, index) + 1;
        }

        internal DISPLAYCONFIG_TARGET_DEVICE_NAME GetDeviceNameInfo(int displayIndex)
        {
            int index = Array.IndexOf(AvailablePathIndexes, displayIndex);
            return AvailablePathNames[index];
        }

        public DISPLAYCONFIG_ROTATION GetDisplayRotation(uint displayId)
        {
            int displayIndex = GetDisplayIndex(displayId);
            ValidatePathIsActive(displayIndex);
            return PathArray[displayIndex].targetInfo.rotation;
        }

        public void SetDisplayRotation(uint displayId, DISPLAYCONFIG_ROTATION rotation)
        {
            int displayIndex = GetDisplayIndex(displayId);
            ValidatePathIsActive(displayIndex);
            PathArray[displayIndex].targetInfo.rotation = rotation;
        }

        public DISPLAYCONFIG_SCALING GetDisplayScaling(uint displayId)
        {
            int displayIndex = GetDisplayIndex(displayId);
            ValidatePathIsActive(displayIndex);
            return PathArray[displayIndex].targetInfo.scaling;
        }

        public void SetDisplayScaling(uint displayId, DISPLAYCONFIG_SCALING scale)
        {
            int displayIndex = GetDisplayIndex(displayId);
            ValidatePathIsActive(displayIndex);
            PathArray[displayIndex].targetInfo.scaling = scale;
        }

        public double GetDisplayRefreshRate(uint displayId)
        {
            int displayIndex = GetDisplayIndex(displayId);
            DISPLAYCONFIG_RATIONAL result = GetDisplayRefreshRate(displayIndex);
            return result.AsDouble();
        }

        internal DISPLAYCONFIG_RATIONAL GetDisplayRefreshRate(int displayIndex)
        {
            ValidatePathIsActive(displayIndex);
            uint modeInfoIndex = PathArray[displayIndex].targetInfo.modeInfoIdx;
            ValidateModeIndex(modeInfoIndex, DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_TARGET);
            return ModeArray[modeInfoIndex].modeInfo.targetMode.targetVideoSignalInfo.vSyncFreq;
        }

        public void SetDisplayRefreshRate(uint displayId, double refreshRate)
        {
            int displayIndex = GetDisplayIndex(displayId);
            var convertedRefreshRate = refreshRate % 1 == 0
                ? new DISPLAYCONFIG_RATIONAL() { Numerator = (uint)refreshRate, Denominator = 1 }
                : new DISPLAYCONFIG_RATIONAL() { Numerator = (uint)(refreshRate * 1000), Denominator = 1000 };
            SetDisplayRefreshRate(displayIndex, convertedRefreshRate);
        }

        internal void SetDisplayRefreshRate(int displayIndex, DISPLAYCONFIG_RATIONAL refreshRate)
        {
            ValidatePathIsActive(displayIndex);
            uint modeInfoIndex = PathArray[displayIndex].targetInfo.modeInfoIdx;
            ValidateModeIndex(modeInfoIndex, DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_TARGET);
            ModeArray[modeInfoIndex].modeInfo.targetMode.targetVideoSignalInfo.vSyncFreq = refreshRate;
        }

        public DISPLAYCONFIG_2DREGION GetDisplayActiveSize(uint displayId)
        {
            int displayIndex = GetDisplayIndex(displayId);
            ValidatePathIsActive(displayIndex);
            uint modeInfoIndex = PathArray[displayIndex].targetInfo.modeInfoIdx;
            ValidateModeIndex(modeInfoIndex, DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_TARGET);
            return ModeArray[modeInfoIndex].modeInfo.targetMode.targetVideoSignalInfo.activeSize;
        }

        public void SetDisplayActiveSize(uint displayId, uint width, uint height)
        {
            int displayIndex = GetDisplayIndex(displayId);
            ValidatePathIsActive(displayIndex);
            uint modeInfoIndex = PathArray[displayIndex].targetInfo.modeInfoIdx;
            ValidateModeIndex(modeInfoIndex, DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_TARGET);
            ModeArray[modeInfoIndex].modeInfo.targetMode.targetVideoSignalInfo.activeSize.cx = width;
            ModeArray[modeInfoIndex].modeInfo.targetMode.targetVideoSignalInfo.activeSize.cy = height;
        }

        public DISPLAYCONFIG_SCANLINE_ORDERING GetDisplayScanlineOrder(uint displayId)
        {
            int displayIndex = GetDisplayIndex(displayId);
            ValidatePathIsActive(displayIndex);
            uint modeInfoIndex = PathArray[displayIndex].targetInfo.modeInfoIdx;
            ValidateModeIndex(modeInfoIndex, DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_TARGET);
            return ModeArray[modeInfoIndex].modeInfo.targetMode.targetVideoSignalInfo.scanLineOrdering;
        }

        public void SetDisplayScanlineOrder(uint displayId, DISPLAYCONFIG_SCANLINE_ORDERING order)
        {
            int displayIndex = GetDisplayIndex(displayId);
            ValidatePathIsActive(displayIndex);
            uint modeInfoIndex = PathArray[displayIndex].targetInfo.modeInfoIdx;
            ValidateModeIndex(modeInfoIndex, DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_TARGET);
            ModeArray[modeInfoIndex].modeInfo.targetMode.targetVideoSignalInfo.scanLineOrdering = order;
        }

        public DisplayResolution GetDisplayResolution(uint displayId)
        {
            int displayIndex = GetDisplayIndex(displayId);
            ValidatePathIsActive(displayIndex);
            uint modeInfoIndex = PathArray[displayIndex].sourceInfo.modeInfoIdx;
            ValidateModeIndex(modeInfoIndex, DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_SOURCE);
            DISPLAYCONFIG_SOURCE_MODE sourceMode = ModeArray[modeInfoIndex].modeInfo.sourceMode;
            return new DisplayResolution(sourceMode.width, sourceMode.height);
        }

        public void SetDisplayResolution(uint displayId, uint width, uint height)
        {
            int displayIndex = GetDisplayIndex(displayId);
            ValidatePathIsActive(displayIndex);
            uint modeInfoIndex = PathArray[displayIndex].sourceInfo.modeInfoIdx;
            ValidateModeIndex(modeInfoIndex, DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_SOURCE);
            ModeArray[modeInfoIndex].modeInfo.sourceMode.width = width;
            ModeArray[modeInfoIndex].modeInfo.sourceMode.height = height;
        }

        public uint GetPrimaryDisplayId()
        {
            foreach (var index in AvailablePathIndexes)
            {
                DISPLAYCONFIG_PATH_INFO pathInfo = PathArray[index];
                if (!pathInfo.flags.HasFlag(PathInfoFlags.DISPLAYCONFIG_PATH_ACTIVE))
                {
                    continue;
                }

                uint modeIndex = pathInfo.sourceInfo.modeInfoIdx;
                if (ModeArray[modeIndex].modeInfo.sourceMode.IsPrimary())
                {
                    return GetDisplayId(index);
                }
            }

            throw new Exception("Failed to find the primary display");
        }

        public void SetPrimaryDisplay(uint displayId)
        {
            int targetIndex = GetDisplayIndex(displayId);
            ValidatePathIsActive(targetIndex);

            uint targetModeIndex = PathArray[targetIndex].sourceInfo.modeInfoIdx;
            ValidateModeIndex(targetModeIndex, DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_SOURCE);

            DISPLAYCONFIG_SOURCE_MODE sourceModeInfo = ModeArray[targetModeIndex].modeInfo.sourceMode;
            if (sourceModeInfo.IsPrimary())
            {
                return;
            }

            for (int i = 0; i < ModeArray.Length; i++)
            {
                if (ModeArray[i].infoType != DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_SOURCE)
                {
                    continue;
                }

                if (ModeArray[i].modeInfo.sourceMode.Equals(sourceModeInfo))
                {
                    ModeArray[i].modeInfo.sourceMode.position.x = 0;
                    ModeArray[i].modeInfo.sourceMode.position.y = 0;
                    continue;
                }

                ModeArray[i].modeInfo.sourceMode.position.x -= sourceModeInfo.position.x;
                ModeArray[i].modeInfo.sourceMode.position.y -= sourceModeInfo.position.y;
            }
        }

        public void CloneDisplay(uint sourceId, HashSet<uint> destinationIds)
        {
            if (destinationIds is null)
            {
                throw new ArgumentNullException(nameof(destinationIds));
            }

            if (destinationIds.Count == 0)
            {
                throw new ArgumentException("Need to specify at least 1 destination ID", nameof(destinationIds));
            }

            int sourceIndex = GetDisplayIndex(sourceId);
            ValidatePathIsActive(sourceIndex);
            uint sourceModeIndex = PathArray[sourceIndex].sourceInfo.modeInfoIdx;
            ValidateModeIndex(sourceModeIndex, DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_SOURCE);

            var destinationIndexes = new HashSet<int>();
            foreach (uint id in destinationIds)
            {
                int destinationIndex = GetDisplayIndex(id);
                ValidatePathIsActive(destinationIndex);
                _ = destinationIndexes.Add(destinationIndex);
            }

            var newModes = new List<DISPLAYCONFIG_MODE_INFO>()
            {
                ModeArray[PathArray[sourceIndex].sourceInfo.modeInfoIdx]
            };
            var indexTable = new Dictionary<uint, uint>
            {
                { PathArray[sourceIndex].sourceInfo.modeInfoIdx, 0 }
            };
            PathArray[sourceIndex].sourceInfo.modeInfoIdx = 0;
            var removedSources = new List<DISPLAYCONFIG_SOURCE_MODE>();

            foreach (int indexOfAvailablePath in AvailablePathIndexes)
            {
                uint currentModeIndex;
                if (destinationIndexes.Contains(indexOfAvailablePath))
                {
                    removedSources.Add(ModeArray[PathArray[indexOfAvailablePath].sourceInfo.modeInfoIdx].modeInfo.sourceMode);
                    PathArray[indexOfAvailablePath].sourceInfo = PathArray[sourceIndex].sourceInfo;
                }
                else if (indexOfAvailablePath != sourceIndex)
                {
                    currentModeIndex = PathArray[indexOfAvailablePath].sourceInfo.modeInfoIdx;
                    if (!indexTable.TryGetValue(currentModeIndex, out uint newModeIndex))
                    {
                        newModeIndex = (uint)newModes.Count;
                        indexTable.Add(currentModeIndex, newModeIndex);
                        newModes.Add(ModeArray[currentModeIndex]);
                    }

                    PathArray[indexOfAvailablePath].sourceInfo.modeInfoIdx = newModeIndex;
                }

                currentModeIndex = PathArray[indexOfAvailablePath].targetInfo.modeInfoIdx;
                if (!indexTable.TryGetValue(currentModeIndex, out uint newIndex))
                {
                    newIndex = (uint)newModes.Count;
                    indexTable.Add(currentModeIndex, newIndex);
                    newModes.Add(ModeArray[currentModeIndex]);
                }

                PathArray[indexOfAvailablePath].targetInfo.modeInfoIdx = newIndex;

            }

            ModeArray = newModes.ToArray();
            foreach (DISPLAYCONFIG_SOURCE_MODE removedSource in removedSources.Distinct())
            {
                if (removedSource.IsPrimary())
                {
                    SetPrimaryDisplay(sourceId);
                    continue;
                }

                AdjustDesktopAfterRemovedSource(removedSource.position);
            }
        }

        public void DisableDisplay(uint displayId)
        {
            int pathIndex = GetDisplayIndex(displayId);
            if (PathArray[pathIndex].flags == 0)
            {
                // Display is already disabled
                return;
            }

            uint sourceIndex = PathArray[pathIndex].sourceInfo.modeInfoIdx;
            ValidateModeIndex(sourceIndex, DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_SOURCE);
            bool sourceIsCloned = false;
            foreach (int index in AvailablePathIndexes)
            {
                if (index == pathIndex)
                {
                    continue;
                }

                if (PathArray[index].sourceInfo.modeInfoIdx == sourceIndex)
                {
                    sourceIsCloned = true;
                    break;
                }
            }

            DISPLAYCONFIG_SOURCE_MODE oldSource = ModeArray[sourceIndex].modeInfo.sourceMode;
            if (!sourceIsCloned && oldSource.IsPrimary())
            {
                throw new Exception($"Cannot disable display {displayId} because it is the primary display.");
            }

            PathArray[pathIndex].sourceInfo.modeInfoIdx = DISPLAYCONFIG_PATH_MODE_IDX_INVALID;
            PathArray[pathIndex].targetInfo.modeInfoIdx = DISPLAYCONFIG_PATH_MODE_IDX_INVALID;
            PathArray[pathIndex].flags = 0;

            if (!sourceIsCloned)
            {
                AdjustDesktopAfterRemovedSource(oldSource.position);
            }

            var newModes = new List<DISPLAYCONFIG_MODE_INFO>();
            var indexTable = new Dictionary<uint, uint>();

            foreach (int indexOfAvailablePath in AvailablePathIndexes)
            {
                uint currentModeIndex = PathArray[indexOfAvailablePath].sourceInfo.modeInfoIdx;
                if (currentModeIndex != DISPLAYCONFIG_PATH_MODE_IDX_INVALID)
                {
                    if (!indexTable.TryGetValue(currentModeIndex, out uint newIndex))
                    {
                        newIndex = (uint)newModes.Count;
                        indexTable.Add(currentModeIndex, newIndex);
                        newModes.Add(ModeArray[currentModeIndex]);
                    }

                    PathArray[indexOfAvailablePath].sourceInfo.modeInfoIdx = newIndex;
                }

                currentModeIndex = PathArray[indexOfAvailablePath].targetInfo.modeInfoIdx;
                if (currentModeIndex != DISPLAYCONFIG_PATH_MODE_IDX_INVALID)
                {
                    if (!indexTable.TryGetValue(currentModeIndex, out uint newIndex))
                    {
                        newIndex = (uint)newModes.Count;
                        indexTable.Add(currentModeIndex, newIndex);
                        newModes.Add(ModeArray[currentModeIndex]);
                    }

                    PathArray[indexOfAvailablePath].targetInfo.modeInfoIdx = newIndex;
                }
            }

            ModeArray = newModes.ToArray();
        }

        public POINTL GetDisplayPosition(uint displayId)
        {
            int pathIndex = GetDisplayIndex(displayId);
            ValidatePathIsActive(pathIndex);
            uint modeIndex = PathArray[pathIndex].sourceInfo.modeInfoIdx;
            ValidateModeIndex(modeIndex, DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_SOURCE);
            return ModeArray[modeIndex].modeInfo.sourceMode.position;
        }

        /// <summary>
        /// Sets the position of a display.
        /// </summary>
        /// <remarks>
        /// This does not attempt to reposition other displays to fit the new position.
        /// It's the callers responsibility to make sure all desktop positions are valid (no overlapping displays, or gaps)
        /// </remarks>
        public void SetDisplayPosition(uint displayId, int x, int y)
        {
            int pathIndex = GetDisplayIndex(displayId);
            ValidatePathIsActive(pathIndex);
            uint modeIndex = PathArray[pathIndex].sourceInfo.modeInfoIdx;
            ValidateModeIndex(modeIndex, DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_SOURCE);
            ModeArray[modeIndex].modeInfo.sourceMode.position.x = x;
            ModeArray[modeIndex].modeInfo.sourceMode.position.y = y;
        }

        public void SetDisplayPositionLeftToRight(uint[] displayIDsInOrder)
        {
            if (displayIDsInOrder is null)
            {
                throw new ArgumentNullException(nameof(displayIDsInOrder));
            }

            int activeDisplayCount = GetActivePathIndexes().Count;
            if (displayIDsInOrder.Length != activeDisplayCount)
            {
                // ToDo: Relax the check for cloned displays.
                throw new ArgumentException($"The display IDs must include every active display", nameof(displayIDsInOrder));
            }

            var displayIndexesInOrder = new int[displayIDsInOrder.Length];
            uint primaryDisplayId = 0;
            for (int i = 0; i < displayIDsInOrder.Length; i++)
            {
                int displayIndex = GetDisplayIndex(displayIDsInOrder[i]);
                ValidatePathIsActive(displayIndex);
                displayIndexesInOrder[i] = displayIndex;
                DISPLAYCONFIG_PATH_INFO pathInfo = PathArray[displayIndex];
                
                if (primaryDisplayId == 0 && ModeArray[pathInfo.sourceInfo.modeInfoIdx].modeInfo.sourceMode.IsPrimary())
                {
                    primaryDisplayId = displayIDsInOrder[i];
                }
            }

            var modifiedSources = new HashSet<uint>();
            int xOffset = 0;
            for (int i = 0; i < displayIDsInOrder.Length; i++)
            {
                uint modeIndex = PathArray[displayIndexesInOrder[i]].sourceInfo.modeInfoIdx;
                if (!modifiedSources.Add(modeIndex))
                {
                    // Logical display position has already been updated (cloned display)
                    continue;
                }

                SetDisplayPosition(displayIDsInOrder[i], xOffset, 0);
                xOffset += (int)ModeArray[modeIndex].modeInfo.sourceMode.width;
            }

            SetPrimaryDisplay(primaryDisplayId);
        }

        private void ValidateModeIndex(uint modeInfoIndex, DISPLAYCONFIG_MODE_INFO_TYPE expectedType)
        {
            DISPLAYCONFIG_MODE_INFO_TYPE modeType;
            try
            {
                modeType = ModeArray[modeInfoIndex].infoType;
            }
            catch (IndexOutOfRangeException e)
            {
                throw new Exception($"Mode index {modeInfoIndex} did not point to a valid entry in the ModeArray.", e);
            }

            if (modeType != expectedType)
            {
                throw new Exception($"The mode entry at {modeInfoIndex} is a {modeType} instead of the expected {expectedType}.");
            }
        }

        internal void ValidatePathIsActive(int pathIndex)
        {
            if (!PathArray[pathIndex].flags.HasFlag(PathInfoFlags.DISPLAYCONFIG_PATH_ACTIVE))
            {
                throw new Exception($"Display ID '{GetDisplayId(pathIndex)}' is not active");
            }
        }

        /// <summary>
        /// Rearranges desktop position of the sources in <see cref="ModeArray"/> to close any gaps left after removing a source (disabling a display)
        /// </summary>
        /// <param name="oldSourcePosition">The position of the display that has been removed.</param>
        private void AdjustDesktopAfterRemovedSource(POINTL oldSourcePosition)
        {
            for (int i = 0; i < ModeArray.Length; i++)
            {
                if (ModeArray[i].infoType != DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_SOURCE)
                {
                    continue;
                }

                POINTL position = ModeArray[i].modeInfo.sourceMode.position;
                if (oldSourcePosition.x > 0)
                {
                    if (position.x > oldSourcePosition.x)
                    {
                        ModeArray[i].modeInfo.sourceMode.position.x -= oldSourcePosition.x;
                    }
                }
                else if (oldSourcePosition.x < 0 && position.x < oldSourcePosition.x)
                {
                    ModeArray[i].modeInfo.sourceMode.position.x -= oldSourcePosition.x;
                }

                if (oldSourcePosition.y > 0)
                {
                    if (position.y > oldSourcePosition.y)
                    {
                        ModeArray[i].modeInfo.sourceMode.position.y -= oldSourcePosition.y;
                    }
                }
                else if (oldSourcePosition.y < 0 && position.y < oldSourcePosition.y)
                {
                    ModeArray[i].modeInfo.sourceMode.position.y -= oldSourcePosition.y;
                }
            }
        }

        /// <summary>
        /// Moves the specified display to a <see cref="RelativePosition"/> relative to the other specified display.
        /// </summary>
        /// <remarks>
        /// This does not attempt to reposition other displays to fit the new position.
        /// It's the callers responsibility to make sure all desktop positions are valid (no overlapping displays, or gaps)
        /// </remarks>
        public void MoveDisplayPosition(uint displayId, uint relativeDisplayId, RelativePosition position)
        {
            int displayIndex = GetDisplayIndex(displayId);
            ValidatePathIsActive(displayIndex);

            int relativeDisplayIndex = GetDisplayIndex(relativeDisplayId);
            ValidatePathIsActive(relativeDisplayIndex);

            uint displayModeIndex = PathArray[displayIndex].sourceInfo.modeInfoIdx;
            ValidateModeIndex(displayModeIndex, DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_SOURCE);
            uint relativeDisplayModeIndex = PathArray[relativeDisplayIndex].sourceInfo.modeInfoIdx;
            ValidateModeIndex(relativeDisplayModeIndex, DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_SOURCE);
            if (displayModeIndex == relativeDisplayModeIndex)
            {
                throw new Exception("Both displays share the same source");
            }

            bool resetPrimary = false;
            if (ModeArray[displayModeIndex].modeInfo.sourceMode.IsPrimary())
            {
                // Temporarily change the primary display so I don't have to deal with a new offset for every other display.
                resetPrimary = true;
                SetPrimaryDisplay(relativeDisplayId);
            }

            DISPLAYCONFIG_SOURCE_MODE oldSourceMode = ModeArray[displayModeIndex].modeInfo.sourceMode;
            DISPLAYCONFIG_SOURCE_MODE relativeSourceMode = ModeArray[relativeDisplayModeIndex].modeInfo.sourceMode;
            switch (position)
            {
                case RelativePosition.Left:
                    ModeArray[displayModeIndex].modeInfo.sourceMode.position.x = relativeSourceMode.position.x - (int)oldSourceMode.width;
                    ModeArray[displayModeIndex].modeInfo.sourceMode.position.y = relativeSourceMode.position.y;
                    break;

                case RelativePosition.Right:
                    ModeArray[displayModeIndex].modeInfo.sourceMode.position.x = relativeSourceMode.position.x + (int)relativeSourceMode.width;
                    ModeArray[displayModeIndex].modeInfo.sourceMode.position.y = relativeSourceMode.position.y;
                    break;

                case RelativePosition.Above:
                    ModeArray[displayModeIndex].modeInfo.sourceMode.position.x = relativeSourceMode.position.x;
                    ModeArray[displayModeIndex].modeInfo.sourceMode.position.y = relativeSourceMode.position.y - (int)oldSourceMode.height;
                    break;

                case RelativePosition.Under:
                    ModeArray[displayModeIndex].modeInfo.sourceMode.position.x = relativeSourceMode.position.x;
                    ModeArray[displayModeIndex].modeInfo.sourceMode.position.y = relativeSourceMode.position.y + (int)relativeSourceMode.height;
                    break;

                default:
                    break;
            }

            if (resetPrimary)
            {
                SetPrimaryDisplay(displayId);
            }
        }

        /// <summary>
        /// Moves the specified display X and Y specified amount of pixels from its current location.
        /// </summary>
        /// <remarks>
        /// This does not attempt to reposition other displays to fit the new position.
        /// It's the callers responsibility to make sure all desktop positions are valid (no overlapping displays, or gaps)
        /// </remarks>
        public void MoveDisplayPosition(uint displayId, int xOffset, int yOffset)
        {
            int displayIndex = GetDisplayIndex(displayId);
            ValidatePathIsActive(displayIndex);
            uint displayModeIndex = PathArray[displayIndex].sourceInfo.modeInfoIdx;
            ValidateModeIndex(displayModeIndex, DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_SOURCE);

            uint tempPrimaryId = 0;
            if (ModeArray[displayModeIndex].modeInfo.sourceMode.IsPrimary())
            {
                // Temporarily change the primary display so I don't have to deal with a new offset for every other display.
                foreach (int index in AvailablePathIndexes)
                {
                    if (!PathArray[index].flags.HasFlag(PathInfoFlags.DISPLAYCONFIG_PATH_ACTIVE) ||
                        PathArray[index].sourceInfo.modeInfoIdx == displayModeIndex)
                    {
                        continue;
                    }

                    tempPrimaryId = GetDisplayId(index);
                    break;
                }

                if (tempPrimaryId == 0)
                {
                    // There is only 1 active display, so it doesn't make sense to adjust positions.
                    return;
                }

                SetPrimaryDisplay(tempPrimaryId);
            }

            ModeArray[displayModeIndex].modeInfo.sourceMode.position.x += xOffset;
            ModeArray[displayModeIndex].modeInfo.sourceMode.position.y += yOffset;

            if (tempPrimaryId != 0)
            {
                SetPrimaryDisplay(displayId);
            }
        }

        /// <summary>
        /// Swaps the desktop positions of the specified displays.
        /// </summary>
        /// <remarks>
        /// This does not attempt to reposition other displays to fit the new position.
        /// It's the callers responsibility to make sure all desktop positions are valid (no overlapping displays, or gaps)
        /// </remarks>
        public void SwapDisplayPosition(uint displayId1, uint displayId2)
        {
            int displayIndex1 = GetDisplayIndex(displayId1);
            ValidatePathIsActive(displayIndex1);
            uint displayModeIndex1 = PathArray[displayIndex1].sourceInfo.modeInfoIdx;
            ValidateModeIndex(displayModeIndex1, DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_SOURCE);

            int displayIndex2 = GetDisplayIndex(displayId2);
            ValidatePathIsActive(displayIndex2);
            uint displayModeIndex2 = PathArray[displayIndex2].sourceInfo.modeInfoIdx;
            ValidateModeIndex(displayModeIndex2, DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_SOURCE);

            if (displayModeIndex1 == displayModeIndex2)
            {
                throw new ArgumentException($"Displays {displayId1} and {displayId2} share the same source");
            }

            var oldSource1 = ModeArray[displayModeIndex1].modeInfo.sourceMode;
            var oldSource2 = ModeArray[displayModeIndex2].modeInfo.sourceMode;
            ModeArray[displayModeIndex1].modeInfo.sourceMode.position = oldSource2.position;
            ModeArray[displayModeIndex2].modeInfo.sourceMode.position = oldSource1.position;
            if (oldSource1.IsPrimary())
            {
                SetPrimaryDisplay(displayId1);
            }
            else if (oldSource2.IsPrimary())
            {
                SetPrimaryDisplay(displayId2);
            }
        }

        /// <summary>
        /// Updates all the display adapter ID references in the displayconfig to the adapter IDs found in a new display config scan.
        /// This is useful when importing an old serialized config where the adapter IDs may have changed.
        /// </summary>
        public void UpdateAdapterIds()
        {
            var newConfig = GetConfig(Flags);
            if (newConfig.AvailablePathNames.Length != AvailablePathNames.Length)
            {
                throw new Exception($"Cannot update adapter IDs because the amount of connected displays has changed. Old: {AvailablePathNames.Length} New: {newConfig.AvailablePathNames.Length}");
            }

            var luidMap = new Dictionary<LUID, LUID>(AvailablePathNames.Length);
            for (int i = 0; i < AvailablePathNames.Length; i++)
            {
                if (luidMap.TryGetValue(AvailablePathNames[i].header.adapterId, out LUID foundAdapter))
                {
                    if (!newConfig.AvailablePathNames[i].header.adapterId.Equals(foundAdapter))
                    {
                        throw new Exception("Cannot update adapter IDs because the adapter layout has changed.");
                    }

                    continue;
                }

                luidMap.Add(AvailablePathNames[i].header.adapterId, newConfig.AvailablePathNames[i].header.adapterId);
            }

            for (int i = 0; i < AvailablePathNames.Length; i++)
            {
                AvailablePathNames[i].header.adapterId = luidMap[AvailablePathNames[i].header.adapterId];
            }

            for (int i = 0; i < ModeArray.Length; i++)
            {
                ModeArray[i].adapterId = luidMap[ModeArray[i].adapterId];
            }

            for (int i = 0; i < PathArray.Length; i++)
            {
                PathArray[i].sourceInfo.adapterId = luidMap[PathArray[i].sourceInfo.adapterId];
                PathArray[i].targetInfo.adapterId = luidMap[PathArray[i].targetInfo.adapterId];
            }
        }
    }
}