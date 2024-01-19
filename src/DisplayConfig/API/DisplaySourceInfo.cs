using MartinGC94.DisplayConfig.Native;
using MartinGC94.DisplayConfig.Native.Enums;
using MartinGC94.DisplayConfig.Native.Structs;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.API
{
    internal sealed class DisplaySourceInfo
    {
        public static string GetGdiDeviceName(LUID adapterId, uint sourceId)
        {
            var deviceNameInfo = new DISPLAYCONFIG_SOURCE_DEVICE_NAME()
            {
                header = new DISPLAYCONFIG_DEVICE_INFO_HEADER()
                {
                    type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_GET_SOURCE_NAME,
                    adapterId = adapterId,
                    id = sourceId
                }
            };
            deviceNameInfo.header.size = (uint)Marshal.SizeOf(deviceNameInfo);
            ReturnCode res = NativeMethods.DisplayConfigGetDeviceInfo(ref deviceNameInfo);
            if (res != ReturnCode.ERROR_SUCCESS)
            {
                throw new Win32Exception((int)res);
            }

            return deviceNameInfo.viewGdiDeviceName;
        }
    }
}