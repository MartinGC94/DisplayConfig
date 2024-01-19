using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.Native.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct DpiConfigSet
    {
        public DISPLAYCONFIG_DEVICE_INFO_HEADER header;

        public int relativeScale;
    }
}