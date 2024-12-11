using System;
using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.Native.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DISPLAYCONFIG_RATIONAL
    {
        public uint Numerator;
        public uint Denominator;

        public static DISPLAYCONFIG_RATIONAL FromDouble(double input)
        {
            double accuracy = 0.000000000000001;

            int sign = Math.Sign(input);

            if (sign == -1)
            {
                input = Math.Abs(input);
            }

            // Accuracy is the maximum relative error; convert to absolute maxError
            double maxError = sign == 0 ? accuracy : input * accuracy;

            int n = (int)Math.Floor(input);
            input -= n;

            if (input < maxError)
            {
                return new DISPLAYCONFIG_RATIONAL() { Numerator = (uint)(sign * n), Denominator = 1 };
            }

            if (1 - maxError < input)
            {
                return new DISPLAYCONFIG_RATIONAL() { Numerator = (uint)(sign * (n + 1)), Denominator = 1 };
            }

            // The lower fraction is 0/1
            uint lower_n = 0;
            uint lower_d = 1;

            // The upper fraction is 1/1
            uint upper_n = 1;
            uint upper_d = 1;

            while (true)
            {
                // The middle fraction is (lower_n + upper_n) / (lower_d + upper_d)
                uint middle_n = lower_n + upper_n;
                uint middle_d = lower_d + upper_d;

                if (middle_d * (input + maxError) < middle_n)
                {
                    // real + error < middle : middle is our new upper
                    upper_n = middle_n;
                    upper_d = middle_d;
                }
                else if (middle_n < (input - maxError) * middle_d)
                {
                    // middle < real - error : middle is our new lower
                    lower_n = middle_n;
                    lower_d = middle_d;
                }
                else
                {
                    // Middle is our best fraction
                    return new DISPLAYCONFIG_RATIONAL() { Numerator = (uint)((n * middle_d + middle_n) * sign), Denominator = middle_d };
                }
            }
        }

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