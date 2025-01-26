using MartinGC94.DisplayConfig.Native.Structs;
using MartinGC94.DisplayConfig.Native;
using System;
using System.Management.Automation;
using System.Runtime.InteropServices;
using MartinGC94.DisplayConfig.Native.Enums;
using System.ComponentModel;

namespace MartinGC94.DisplayConfig.API
{
    internal sealed class VirtualResolution
    {
        internal static void ToggleVirtualResolution(Cmdlet command, uint[] displayIds, bool enabled)
        {
            var config = DisplayConfig.GetConfig(command);
            foreach (uint id in displayIds)
            {
                int index;
                try
                {
                    index = config.GetDisplayIndex(id);
                }
                catch (ArgumentException error)
                {
                    command.WriteError(Utils.GetInvalidDisplayIdError(error, id));
                    continue;
                }

                try
                {
                    config.ValidatePathIsActive(index);
                }
                catch (Exception error) when (!(error is PipelineStoppedException))
                {
                    command.WriteError(new ErrorRecord(error, "InactiveDisplay", ErrorCategory.InvalidArgument, id));
                    continue;
                }

                LUID adapterId = config.PathArray[index].targetInfo.adapterId;
                uint sourceId = config.PathArray[index].targetInfo.id;
                var virtualResolutionInfo = new DISPLAYCONFIG_SUPPORT_VIRTUAL_RESOLUTION()
                {
                    header = new DISPLAYCONFIG_DEVICE_INFO_HEADER()
                    {
                        adapterId = adapterId,
                        id = sourceId,
                        type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_SET_SUPPORT_VIRTUAL_RESOLUTION
                    },
                    value = (uint)(enabled ? 0 : 1)
                };
                virtualResolutionInfo.header.size = (uint)Marshal.SizeOf(virtualResolutionInfo);

                ReturnCode res = NativeMethods.DisplayConfigSetDeviceInfo(ref virtualResolutionInfo);
                if (res != ReturnCode.ERROR_SUCCESS)
                {
                    var error = new Win32Exception((int)res);
                    command.WriteError(new ErrorRecord(error, "FailedToConfigureVirtualResolution", Utils.GetErrorCategory(error), id));
                }
            }
        }
    }
}