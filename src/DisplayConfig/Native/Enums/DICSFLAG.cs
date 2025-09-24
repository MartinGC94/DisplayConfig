using System;

namespace MartinGC94.DisplayConfig.Native.Enums
{
    internal enum DICSFLAG : uint
    {
        DICS_FLAG_GLOBAL         = 0x00000001,
        DICS_FLAG_CONFIGSPECIFIC = 0x00000002,
        DICS_FLAG_CONFIGGENERAL  = 0x00000004
    }
}