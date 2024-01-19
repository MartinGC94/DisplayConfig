using System;

namespace MartinGC94.DisplayConfig.Native.Enums
{
    [Flags]
    public enum DeviceNameEdidFlags : uint
    {
        FriendlyNameFromEdid = 0x00000001,
        FriendlyNameForced = 0x00000002,
        EdidIdsValid = 0x00000004
    }
}
