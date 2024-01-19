using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.Native.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LUID
    {
        public uint LowPart;
        public int HighPart;

        public override string ToString()
        {
            return $"{LowPart} {HighPart}";
        }

        public override bool Equals(object obj)
        {
            return obj is LUID id && id.LowPart == LowPart && id.HighPart == HighPart;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)LowPart) + HighPart;
            }
        }
    }
}