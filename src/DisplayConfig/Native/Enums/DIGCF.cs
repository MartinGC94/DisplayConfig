using System;

namespace MartinGC94.DisplayConfig.Native.Enums
{
    [Flags]
    internal enum DIGCF : uint
    {
        DIGCF_DEFAULT         =  0x00000001,
        DIGCF_PRESENT         =  0x00000002,
        DIGCF_ALLCLASSES      =  0x00000004,
        DIGCF_PROFILE         =  0x00000008,
        DIGCF_DEVICEINTERFACE =  0x00000010
    }
}