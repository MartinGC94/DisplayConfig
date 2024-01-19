using MartinGC94.DisplayConfig.Native.Enums;
using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.Native.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DISPLAYCONFIG_SOURCE_MODE
    {
        public uint width;
        public uint height;
        public DISPLAYCONFIG_PIXELFORMAT pixelFormat;
        public POINTL position;

        public override bool Equals(object obj)
        {
            return obj is DISPLAYCONFIG_SOURCE_MODE sourceMode &&
                sourceMode.width == width &&
                sourceMode.height == height &&
                sourceMode.pixelFormat == pixelFormat &&
                sourceMode.position.x == position.x &&
                sourceMode.position.y == position.y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (int)width + (int)height + (int)pixelFormat + position.x + position.y;
            }
        }

        public bool IsPrimary()
        {
            return position.x == 0 && position.y == 0;
        }
    }
}