using System;

namespace MartinGC94.DisplayConfig.Native.Enums
{
    [Flags]
    public enum SetDisplayConfigFlags : uint
    {
        SDC_APPLY = 0x00000080,
        SDC_NO_OPTIMIZATION = 0x00000100,
        SDC_USE_SUPPLIED_DISPLAY_CONFIG = 0x00000020,
        SDC_SAVE_TO_DATABASE = 0x00000200,
        SDC_VALIDATE = 0x00000040,
        SDC_ALLOW_CHANGES = 0x00000400,
        SDC_TOPOLOGY_CLONE = 0x00000002,
        SDC_TOPOLOGY_EXTEND = 0x00000004,
        SDC_TOPOLOGY_INTERNAL = 0x00000001,
        SDC_TOPOLOGY_EXTERNAL = 0x00000008,
        SDC_TOPOLOGY_SUPPLIED = 0x00000010,
        SDC_USE_DATABASE_CURRENT = SDC_TOPOLOGY_INTERNAL | SDC_TOPOLOGY_CLONE | SDC_TOPOLOGY_EXTEND | SDC_TOPOLOGY_EXTERNAL,
        SDC_PATH_PERSIST_IF_REQUIRED = 0x00000800,
        SDC_FORCE_MODE_ENUMERATION = 0x00001000,
        SDC_ALLOW_PATH_ORDER_CHANGES = 0x00002000,
        SDC_VIRTUAL_MODE_AWARE = 0x00008000,
        SDC_VIRTUAL_REFRESH_RATE_AWARE = 0x00020000
    }
}