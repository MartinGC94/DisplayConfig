using System;

namespace MartinGC94.DisplayConfig.Native.Enums
{
    [Flags]
    public enum PathInfoFlags : uint
    {
        DISPLAYCONFIG_PATH_ACTIVE = 0x00000001,
        DISPLAYCONFIG_PATH_SUPPORT_VIRTUAL_MODE = 0x00000008,
        DISPLAYCONFIG_PATH_BOOST_REFRESH_RATE = 0x00000010
    }
}