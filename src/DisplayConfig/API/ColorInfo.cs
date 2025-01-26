using MartinGC94.DisplayConfig.Native.Enums;
using MartinGC94.DisplayConfig.Native.Structs;
using MartinGC94.DisplayConfig.Native;
using System;
using System.ComponentModel;
using System.Management.Automation;
using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.API
{
    internal sealed class ColorInfo
    {
        public bool AdvancedColorSupported { get; }
        public bool AdvancedColorEnabled { get; }
        public bool WideColorEnforced { get; }
        public bool AdvancedColorForceDisabled { get; }
        public ColorEncoding ColorEncoding { get; }
        public uint BitsPerColorChannel { get; }
        public uint SDRWhiteLevel { get; }
        public float WhiteLevelInNits { get; }

        private ColorInfo(DISPLAYCONFIG_GET_ADVANCED_COLOR_INFO colorData, DISPLAYCONFIG_SDR_WHITE_LEVEL sdrData)
        {
            AdvancedColorSupported = colorData.AdvancedColorSupported;
            AdvancedColorEnabled = colorData.AdvancedColorEnabled;
            WideColorEnforced = colorData.WideColorEnforced;
            AdvancedColorForceDisabled = colorData.AdvancedColorForceDisabled;
            ColorEncoding = (ColorEncoding)colorData.colorEncoding;
            BitsPerColorChannel = colorData.bitsPerColorChannel;
            SDRWhiteLevel = sdrData.SDRWhiteLevel;
            WhiteLevelInNits = sdrData.WhiteLevelInNits;
        }

        internal static ColorInfo GetColorInfo(DisplayConfig config, uint displayId)
        {
            int index = config.GetDisplayIndex(displayId);
            LUID adapter = config.PathArray[index].targetInfo.adapterId;
            uint targetId = config.PathArray[index].targetInfo.id;
            var colorData = GetAdvancedColorInfo(adapter, targetId);
            var sdrData = GetSdrInfo(adapter, targetId);
            return new ColorInfo(colorData, sdrData);
        }

        private static DISPLAYCONFIG_GET_ADVANCED_COLOR_INFO GetAdvancedColorInfo(LUID adapterId, uint targetId)
        {
            var colorInfo = new DISPLAYCONFIG_GET_ADVANCED_COLOR_INFO()
            {
                header = new DISPLAYCONFIG_DEVICE_INFO_HEADER()
                {
                    type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_GET_ADVANCED_COLOR_INFO,
                    adapterId = adapterId,
                    id = targetId
                }
            };
            colorInfo.header.size = (uint)Marshal.SizeOf(colorInfo);
            ReturnCode result = NativeMethods.DisplayConfigGetDeviceInfo(ref colorInfo);
            if (result != ReturnCode.ERROR_SUCCESS)
            {
                throw new Win32Exception((int)result);
            }

            return colorInfo;
        }

        private static DISPLAYCONFIG_SDR_WHITE_LEVEL GetSdrInfo(LUID adapterId, uint targetId)
        {
            var sdrInfo = new DISPLAYCONFIG_SDR_WHITE_LEVEL()
            {
                header = new DISPLAYCONFIG_DEVICE_INFO_HEADER()
                {
                    type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_GET_SDR_WHITE_LEVEL,
                    adapterId = adapterId,
                    id = targetId
                }
            };
            sdrInfo.header.size = (uint)Marshal.SizeOf(sdrInfo);
            ReturnCode result = NativeMethods.DisplayConfigGetDeviceInfo(ref sdrInfo);
            if (result != ReturnCode.ERROR_SUCCESS)
            {
                throw new Win32Exception((int)result);
            }

            return sdrInfo;
        }

        internal static void ToggleAdvancedColor(Cmdlet command, uint[] displayIds, bool enabled)
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
                var advancedColorInfo = new DISPLAYCONFIG_SET_ADVANCED_COLOR_STATE()
                {
                    header = new DISPLAYCONFIG_DEVICE_INFO_HEADER()
                    {
                        adapterId = adapterId,
                        id = sourceId,
                        type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_SET_ADVANCED_COLOR_STATE
                    },
                    value = (uint)(enabled ? 1 : 0)
                };
                advancedColorInfo.header.size = (uint)Marshal.SizeOf(advancedColorInfo);

                ReturnCode res = NativeMethods.DisplayConfigSetDeviceInfo(ref advancedColorInfo);
                if (res != ReturnCode.ERROR_SUCCESS)
                {
                    var error = new Win32Exception((int)res);
                    command.WriteError(new ErrorRecord(error, "FailedToConfigureAdvancedColor", Utils.GetErrorCategory(error), id));
                }
            }
        }
    
        internal static void SetSdrWhiteLevel(LUID adapterId, uint targetId, uint whiteLevel)
        {
            var whiteLevelInfo = new SdrWhiteLevelSet()
            {
                header = new DISPLAYCONFIG_DEVICE_INFO_HEADER()
                {
                    adapterId = adapterId,
                    id = targetId,
                    type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_SET_SDR_WHITE_LEVEL
                },
                SDRWhiteLevel = whiteLevel,
                unknownValue = 1
            };
            whiteLevelInfo.header.size = (uint)Marshal.SizeOf(whiteLevelInfo);
            ReturnCode res = NativeMethods.DisplayConfigSetDeviceInfo(ref whiteLevelInfo);
            if (res != ReturnCode.ERROR_SUCCESS)
            {
                throw new Win32Exception((int)res);
            }
        }
    }
}