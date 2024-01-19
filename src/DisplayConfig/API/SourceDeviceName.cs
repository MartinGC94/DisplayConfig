using MartinGC94.DisplayConfig.Native;
using MartinGC94.DisplayConfig.Native.Enums;
using MartinGC94.DisplayConfig.Native.Structs;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.API
{
    internal static class SourceDeviceName
    {
        public static string GetDeviceName(DISPLAYCONFIG_PATH_SOURCE_INFO sourceInfo)
        {
            var deviceNameInfo = new DISPLAYCONFIG_SOURCE_DEVICE_NAME()
            {
                header = new DISPLAYCONFIG_DEVICE_INFO_HEADER()
                {
                    type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_GET_SOURCE_NAME,
                    adapterId = sourceInfo.adapterId,
                    id = sourceInfo.id
                }
            };
            deviceNameInfo.header.size = (uint)Marshal.SizeOf(deviceNameInfo);
            ReturnCode result = NativeMethods.DisplayConfigGetDeviceInfo(ref deviceNameInfo);
            if (result != ReturnCode.ERROR_SUCCESS)
            {
                throw new Win32Exception((int)result);
            }

            return deviceNameInfo.viewGdiDeviceName;
        }
    }
}