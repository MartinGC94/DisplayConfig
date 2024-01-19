using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.Native.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct DISPLAYCONFIG_SUPPORT_VIRTUAL_RESOLUTION
    {
        public DISPLAYCONFIG_DEVICE_INFO_HEADER header;
        public uint value;
        public bool DisableMonitorVirtualResolution => (value << 31) >> 31 == 1;
    }
}