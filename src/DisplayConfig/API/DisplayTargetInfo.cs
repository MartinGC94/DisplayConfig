using MartinGC94.DisplayConfig.Native.Enums;
using MartinGC94.DisplayConfig.Native;
using MartinGC94.DisplayConfig.Native.Structs;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace MartinGC94.DisplayConfig.API
{
    internal class DisplayTargetInfo
    {
        public static DISPLAYCONFIG_TARGET_DEVICE_NAME GetTargetDeviceName(LUID adapterId, uint targetId)
        {
            var targetNameInfo = new DISPLAYCONFIG_TARGET_DEVICE_NAME()
            {
                header = new DISPLAYCONFIG_DEVICE_INFO_HEADER()
                {
                    type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_NAME,
                    adapterId = adapterId,
                    id = targetId
                }
            };
            targetNameInfo.header.size = (uint)Marshal.SizeOf(targetNameInfo);
            var result =  NativeMethods.DisplayConfigGetDeviceInfo(ref targetNameInfo);
            if (result != ReturnCode.ERROR_SUCCESS)
            {
                throw new Win32Exception((int)result);
            }

            return targetNameInfo;
        }
    }
}