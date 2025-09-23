using MartinGC94.DisplayConfig.Native.Structs;
using MartinGC94.DisplayConfig.Native.Enums;
using MartinGC94.DisplayConfig.Native;
using System;

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
            // Windows includes predefined key handles that we can use without having to manually open/close them.
            // This is HKEY_LOCAL_MACHINE
            UIntPtr localMachineConstant = (UIntPtr)0x80000002;

            // This transforms a device path like: \\?\DISPLAY#DELA0E7#5&e94bc63&0&UID4352#{e6f07b5f-ee97-4a90-b076-33f57bf4eaa7}
            // into a registry path like: HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\DISPLAY\DELA0E7\5&e94bc63&0&UID4352\Device Parameters
            int keyStart = devicePath.IndexOf('#') + 1;
            int keyEnd = devicePath.LastIndexOf('#');
            string subKeyPath = $"SYSTEM\\CurrentControlSet\\Enum\\DISPLAY\\{devicePath.Substring(keyStart, devicePath.Length - keyStart - keyEnd).Replace('#', '\\')}\\Device Parameters";

            uint bufferSize = 256;
            byte[] outputData = null;
            int returnCode = -1;
            while (bufferSize < 4096)
            {
                uint oldBufferSize = bufferSize;
                outputData = new byte[bufferSize];
                returnCode = NativeMethods.RegGetValueW(
                localMachineConstant,
                subKeyPath,
                "EDID",
                0x00000008, // Binary
                out uint _,
                outputData,
                ref bufferSize);

                // We exit for any error except ERROR_MORE_DATA, in which case we instead bump the buffersize and try again.
                if (returnCode != 234)
                {
                    break;
                }

                bufferSize = oldBufferSize * 2;
            }

            if (returnCode != 0)
            {
                return null;
            }

            Array.Resize(ref outputData, (int)bufferSize);
            EdidInfo info;
            try
            {
                info = new EdidInfo(outputData);
            }
            catch
            {
                info = null;
            }

            return info;
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