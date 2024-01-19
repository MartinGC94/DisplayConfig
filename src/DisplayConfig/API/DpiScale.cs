using MartinGC94.DisplayConfig.Native.Enums;
using MartinGC94.DisplayConfig.Native;
using MartinGC94.DisplayConfig.Native.Structs;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.API
{
    public sealed class DpiScale
    {
        public uint CurrentScale { get; }
        public uint RecommendedScale { get; }
        public uint MaxScale { get; }

        internal DpiScale(DpiConfigGet dpiInfo)
        {
            CurrentScale = DpiValues[dpiInfo.currentRelativeScale - dpiInfo.minRelativeScale];
            RecommendedScale = DpiValues[0 - dpiInfo.minRelativeScale];
            MaxScale = DpiValues[dpiInfo.maxRelativeScale - dpiInfo.minRelativeScale];
        }

        internal static readonly uint[] DpiValues = new uint[] { 100, 125, 150, 175, 200, 225, 250, 300, 350, 400, 450, 500 };

        internal static DpiConfigGet GetDpiInfo(LUID adapterId, uint sourceId)
        {
            var dpiInfo = new DpiConfigGet()
            {
                header = new DISPLAYCONFIG_DEVICE_INFO_HEADER()
                {
                    adapterId = adapterId,
                    id = sourceId,
                    type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_GET_DPI_SCALE
                }
            };
            dpiInfo.header.size = (uint)Marshal.SizeOf(dpiInfo);
            ReturnCode res = NativeMethods.DisplayConfigGetDeviceInfo(ref dpiInfo);
            if (res != ReturnCode.ERROR_SUCCESS)
            {
                throw new Win32Exception((int)res);
            }

            return dpiInfo;
        }

        internal static void SetDpiScale(LUID adapterId, uint sourceId, int relativeScale)
        {
            var dpiInfo = new DpiConfigSet()
            {
                header = new DISPLAYCONFIG_DEVICE_INFO_HEADER()
                {
                    adapterId = adapterId,
                    id = sourceId,
                    type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_SET_DPI_SCALE
                },
                relativeScale = relativeScale
            };
            dpiInfo.header.size = (uint)Marshal.SizeOf(dpiInfo);
            ReturnCode res = NativeMethods.DisplayConfigSetDeviceInfo(ref dpiInfo);
            if (res != ReturnCode.ERROR_SUCCESS)
            {
                throw new Win32Exception((int)res);
            }
        }
    }
}