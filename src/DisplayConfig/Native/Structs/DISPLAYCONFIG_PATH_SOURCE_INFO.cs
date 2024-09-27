using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.Native.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DISPLAYCONFIG_PATH_SOURCE_INFO
    {
        public LUID adapterId;
        public uint id;
        public uint modeInfoIdx;
        public uint statusFlags;

        public uint CloneGroupId
        {
            get => (modeInfoIdx << 16) >> 16;
            set => modeInfoIdx = (SourceModeInfoIdx << 16) | value;
        }
        public uint SourceModeInfoIdx
        {
            get => modeInfoIdx >> 16;
            set => modeInfoIdx = (value << 16) | CloneGroupId;
        }

        public void ResetModeAndSetCloneGroup(uint cloneGroup)
        {
            modeInfoIdx = (API.DisplayConfig.DISPLAYCONFIG_PATH_SOURCE_MODE_IDX_INVALID << 16) | cloneGroup;
        }
    }
}