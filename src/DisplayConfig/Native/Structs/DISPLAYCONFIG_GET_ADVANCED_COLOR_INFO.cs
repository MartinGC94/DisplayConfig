using MartinGC94.DisplayConfig.Native.Enums;
using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.Native.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct DISPLAYCONFIG_GET_ADVANCED_COLOR_INFO
    {
        public DISPLAYCONFIG_DEVICE_INFO_HEADER header;
        public uint value;
        public bool AdvancedColorSupported => (value << 31) >> 31 == 1;
        public bool AdvancedColorEnabled => (value << 30) >> 31 == 1;
        public bool WideColorEnforced => (value << 29) >> 31 == 1;
        public bool AdvancedColorForceDisabled => (value << 28) >> 31 == 1;
        public DISPLAYCONFIG_COLOR_ENCODING colorEncoding;
        public uint bitsPerColorChannel;
    }
}