using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.Native.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DISPLAYCONFIG_PATH_SOURCE_INFO
    {
        public LUID adapterId;
        public uint id;
        public uint modeInfoIdx;
        public uint statusFlags;

        public uint CloneGroupId => (modeInfoIdx << 16) >> 16;
        public uint SourceModeInfoIdx => modeInfoIdx >> 16;
    }
}