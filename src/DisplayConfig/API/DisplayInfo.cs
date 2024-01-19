using MartinGC94.DisplayConfig.Native.Structs;
using MartinGC94.DisplayConfig.Native.Enums;

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

        internal DisplayInfo(DisplayConfig config, uint displayId)
        {
            int displayIndex = config.GetDisplayIndex(displayId);
            DisplayId = displayId;
            DisplayName = config.AvailablePathNames[displayIndex].monitorFriendlyDeviceName;
            Active = config.PathArray[displayIndex].flags.HasFlag(PathInfoFlags.DISPLAYCONFIG_PATH_ACTIVE);
            if (Active)
            {
                GdiDeviceName = SourceDeviceName.GetDeviceName(config.PathArray[displayIndex].sourceInfo);
                uint sourceModeIndex = config.PathArray[displayIndex].sourceInfo.modeInfoIdx;
                Primary = config.ModeArray[sourceModeIndex].modeInfo.sourceMode.IsPrimary();
                Position = config.ModeArray[sourceModeIndex].modeInfo.sourceMode.position;
                uint targetModeIndex = config.PathArray[displayIndex].targetInfo.modeInfoIdx;
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

            ConnectionType = (ConnectionType)config.AvailablePathNames[displayIndex].outputTechnology;
            DevicePath = config.AvailablePathNames[displayIndex].monitorDevicePath;
        }
    }
}