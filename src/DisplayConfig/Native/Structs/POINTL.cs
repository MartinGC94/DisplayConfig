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
    }
}