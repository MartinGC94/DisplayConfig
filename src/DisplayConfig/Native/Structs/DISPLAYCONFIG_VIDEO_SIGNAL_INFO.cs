using MartinGC94.DisplayConfig.Native.Enums;
using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.Native.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DISPLAYCONFIG_VIDEO_SIGNAL_INFO
    {
        public ulong pixelRate;
        public DISPLAYCONFIG_RATIONAL hSyncFreq;
        public DISPLAYCONFIG_RATIONAL vSyncFreq;
        public DISPLAYCONFIG_2DREGION activeSize;
        public DISPLAYCONFIG_2DREGION totalSize;
        public uint videoStandardRaw;
        public DISPLAYCONFIG_SCANLINE_ORDERING scanLineOrdering;

        public uint VideoStandard => (videoStandardRaw << 16) >> 16;
        public uint VSyncFreqDivider => (videoStandardRaw << 10) >> 26;
    }
}