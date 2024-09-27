using MartinGC94.DisplayConfig.Native.Enums;
using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.Native.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DISPLAYCONFIG_PATH_TARGET_INFO
    {
        public LUID adapterId;
        public uint id;
        public uint modeInfoIdx;
        public DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY outputTechnology;
        public DISPLAYCONFIG_ROTATION rotation;
        public DISPLAYCONFIG_SCALING scaling;
        public DISPLAYCONFIG_RATIONAL refreshRate;
        public DISPLAYCONFIG_SCANLINE_ORDERING scanlineOrdering;
        public bool targetAvailable;
        public DisplayConfigStatusFlags statusFlags;

        public uint DesktopModeInfoIdx
        {
            get => (modeInfoIdx << 16) >> 16;
            set => modeInfoIdx = (TargetModeInfoIdx << 16) | value;
        }
        public uint TargetModeInfoIdx
        {
            get => modeInfoIdx >> 16;
            set => modeInfoIdx = (value << 16) | DesktopModeInfoIdx;
        }
    }
}