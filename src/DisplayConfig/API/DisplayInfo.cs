using MartinGC94.DisplayConfig.Native.Structs;
using MartinGC94.DisplayConfig.Native.Enums;
using MartinGC94.DisplayConfig.Native;
using System;
using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.API
{
    public sealed class DisplayInfo
    {
        public uint DisplayId { get; }
        public string DisplayName { get; }
        public bool Active { get; }
        public string GdiDeviceName { get; }
        public bool Primary { get; }
        public POINTL Position { get; }
        public ModeInfo Mode { get; }
        public ConnectionType ConnectionType { get; }
        public string DevicePath { get; }
        public DisplayRotation Rotation { get; }
        public EdidInfo EdidData { get; }

        private DisplayInfo(DisplayConfig config, int displayIndex, uint displayId)
        {
            DISPLAYCONFIG_TARGET_DEVICE_NAME displayNameInfo = config.GetDeviceNameInfo(displayIndex);
            DisplayId = displayId;
            DisplayName = displayNameInfo.monitorFriendlyDeviceName;
            Active = config.PathArray[displayIndex].flags.HasFlag(PathInfoFlags.DISPLAYCONFIG_PATH_ACTIVE);
            if (Active)
            {
                GdiDeviceName = config.isImportedConfig
                    ? null
                    : SourceDeviceName.GetDeviceName(config.PathArray[displayIndex].sourceInfo);
                uint sourceModeIndex = config.PathArray[displayIndex].sourceInfo.SourceModeInfoIdx;
                Primary = config.ModeArray[sourceModeIndex].modeInfo.sourceMode.IsPrimary();
                Position = config.ModeArray[sourceModeIndex].modeInfo.sourceMode.position;
                uint targetModeIndex = config.PathArray[displayIndex].targetInfo.TargetModeInfoIdx;
                Mode = new ModeInfo(
                    config.ModeArray[sourceModeIndex].modeInfo.sourceMode,
                    config.ModeArray[targetModeIndex].modeInfo.targetMode.targetVideoSignalInfo);
                Rotation = (DisplayRotation)config.PathArray[displayIndex].targetInfo.rotation;
            }
            else
            {
                GdiDeviceName = null;
                Primary = false;
                Position = new POINTL();
                Mode = null;
                Rotation = DisplayRotation.None;
            }

            ConnectionType = (ConnectionType)displayNameInfo.outputTechnology;
            DevicePath = displayNameInfo.monitorDevicePath;
            EdidData = GetEdidDataFromRegistry(DevicePath);
        }

        private EdidInfo GetEdidDataFromRegistry(string devicePath)
        {
            int classGuidIndex = devicePath.LastIndexOf('#');
            Guid classGuid = new Guid(devicePath.Substring(classGuidIndex + 1));

            const int instanceStartIndex = 4;
            string pnpDeviceInstanceId = devicePath.Substring(instanceStartIndex, classGuidIndex - instanceStartIndex).Replace('#', '\\');

            IntPtr deviceInfoSet = NativeMethods.SetupDiGetClassDevsW(ref classGuid, pnpDeviceInstanceId, IntPtr.Zero, DIGCF.DIGCF_DEVICEINTERFACE);
            IntPtr invalidHandle = new IntPtr(-1);

            if (deviceInfoSet != invalidHandle)
            {
                var deviceInfoData = new SP_DEVINFO_DATA();
                deviceInfoData.cbSize = (uint)Marshal.SizeOf(deviceInfoData);

                if (NativeMethods.SetupDiEnumDeviceInfo(deviceInfoSet, 0, ref deviceInfoData))
                {
                    IntPtr systemParamsKey = NativeMethods.SetupDiOpenDevRegKey(deviceInfoSet, ref deviceInfoData, DICSFLAG.DICS_FLAG_GLOBAL, 0, DIREG.DIREG_DEV, REGSAM.KEY_QUERY_VALUE);
                    if (systemParamsKey != invalidHandle)
                    {
                        uint bufferSize = 256;
                        byte[] outputData = null;
                        int returnCode = -1;

                        while (bufferSize < 4096)
                        {
                            uint oldBufferSize = bufferSize;
                            outputData = new byte[bufferSize];
                            returnCode = NativeMethods.RegQueryValueExW(systemParamsKey, "EDID", 0, out uint _, outputData, ref bufferSize);

                            // We exit for any error except ERROR_MORE_DATA, in which case we instead bump the buffersize and try again.
                            if (returnCode != 234)
                            {
                                break;
                            }

                            bufferSize = oldBufferSize * 2;
                        }

                        _ = NativeMethods.RegCloseKey(systemParamsKey);
                        if (returnCode == 0)
                        {
                            _ = NativeMethods.SetupDiDestroyDeviceInfoList(deviceInfoSet);
                            Array.Resize(ref outputData, (int)bufferSize);
                            EdidInfo result;
                            try
                            {
                                result = new EdidInfo(outputData);
                            }
                            catch
                            {
                                result = null;
                            }

                            return result;
                        }
                    }
                }

                _ = NativeMethods.SetupDiDestroyDeviceInfoList(deviceInfoSet);
            }

            return null;
        }

        internal static DisplayInfo GetDisplayInfo(DisplayConfig config, uint displayId)
        {
            int index = config.GetDisplayIndex(displayId);
            return new DisplayInfo(config, index, displayId);
        }

        internal static DisplayInfo GetDisplayInfo(DisplayConfig config, int displayIndex)
        {
            uint id = config.GetDisplayId(displayIndex);
            return new DisplayInfo(config, displayIndex, id);
        }
    }
}