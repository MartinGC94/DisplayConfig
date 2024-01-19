using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.Native.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DISPLAYCONFIG_RATIONAL
    {
        public uint Numerator;
        public uint Denominator;

        public override string ToString()
        {
            return $"{Numerator} {Denominator}";
        }

        public double AsDouble()
        {
            return (double)Numerator / Denominator;
        }
    }
}