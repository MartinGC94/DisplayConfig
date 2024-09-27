using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.Native.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINTL
    {
        public int x;
        public int y;

        public override string ToString()
        {
            return $"{x} {y}";
        }

        public override bool Equals(object obj)
        {
            return obj is POINTL item && item.x == x && item.y == y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return x + y;
            }
        }
    }
}