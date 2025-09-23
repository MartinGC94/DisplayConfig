using System;
using System.Globalization;
using System.Text;

namespace MartinGC94.DisplayConfig.API
{
    public sealed class EdidInfo
    {
        public Version EdidVersion { get; }
        public string SerialNumber { get; }
        public DateTime ManufactureDate { get; }

        internal EdidInfo(byte[] rawData)
        {
            ValidateData(rawData);
            EdidVersion = GetVersion(rawData);
            SerialNumber = GetSerialNumber(rawData);
            ManufactureDate = GetManufactureDate(rawData);
        }

        private static void ValidateData(byte[] data)
        {
            if (data.Length < 128)
            {
                throw new ArgumentException("EDID is too small to be valid.");
            }

            if (data[0] != 00
                || data[1] != 0xFF
                || data[2] != 0xFF
                || data[3] != 0xFF
                || data[4] != 0xFF
                || data[5] != 0xFF
                || data[6] != 0xFF
                || data[7] != 00)
            {
                throw new ArgumentException("EDID header is invalid.");
            }
        }

        private static Version GetVersion(byte[] data)
        {
            return new Version(data[18], data[19]);
        }

        private static string GetSerialNumber(byte[] data)
        {
            int offset = 72;
            while (offset < 126)
            {
                if (data[offset] == 0 && data[offset + 3] == 255)
                {
                    return Encoding.ASCII.GetString(data, offset + 5, 13).Trim();
                }

                offset += 18;
            }

            // Fallback to the numeric serial number if we can't find it in one of the monitor descriptors.
            return BitConverter.ToInt32(data, 12).ToString();
        }

        private static DateTime GetManufactureDate(byte[] data)
        {
            int week = data[16];
            int year = 1990 + data[17];

            if (week == byte.MinValue || week == byte.MaxValue)
            {
                return new DateTime(year, 1, 1);
            }

            // Get first datetime of a given week: https://stackoverflow.com/a/9064954
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            int weekNum = week;
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }
    }
}