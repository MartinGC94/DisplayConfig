using MartinGC94.DisplayConfig.Native.Enums;
using MartinGC94.DisplayConfig.Native.Structs;
using MartinGC94.DisplayConfig.Native;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.API
{
    internal static class AdapterName
    {
        public static string GetAdapterName(LUID adapterId)
        {
            var adapterNameInfo = new DISPLAYCONFIG_ADAPTER_NAME()
            {
                header = new DISPLAYCONFIG_DEVICE_INFO_HEADER()
                {
                    type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_GET_ADAPTER_NAME,
                    adapterId = adapterId
                }
            };
            adapterNameInfo.header.size = (uint)Marshal.SizeOf(adapterNameInfo);
            ReturnCode result = NativeMethods.DisplayConfigGetDeviceInfo(ref adapterNameInfo);
            if (result != ReturnCode.ERROR_SUCCESS)
            {
                throw new Win32Exception((int)result);
            }

            return adapterNameInfo.adapterDevicePath;
        }
    }
}