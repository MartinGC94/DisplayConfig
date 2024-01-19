using MartinGC94.DisplayConfig.Native.Enums;
using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.Native.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DISPLAYCONFIG_TARGET_DEVICE_NAME_FLAGS
    {
        public DeviceNameEdidFlags Flags { get => (DeviceNameEdidFlags)value; }
        public uint value;
    }
}
