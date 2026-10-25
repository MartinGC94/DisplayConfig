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
        public bool AdvancedColorActive { get; }
        public bool AdvancedColorLimitedByPolicy { get; }
        public bool HDRSupported { get; }
        public bool HDREnabled { get; }
        public bool WideColorSupported { get; }
        public bool WideColorEnabled { get; }
        public ColorEncoding ColorEncoding { get; }
        public uint BitsPerColorChannel { get; }
        public ColorMode ColorMode { get; }
        public uint SDRWhiteLevel { get; }
        public float SDRWhiteLevelInNits { get; }
        public uint DisplayId { get; }

        private ColorInfo(DISPLAYCONFIG_GET_ADVANCED_COLOR_INFO_2 colorData, DISPLAYCONFIG_SDR_WHITE_LEVEL sdrData, uint displayId)
        {
            AdvancedColorSupported = colorData.AdvancedColorSupported;
            AdvancedColorActive = colorData.AdvancedColorActive;
            AdvancedColorLimitedByPolicy = colorData.AdvancedColorLimitedByPolicy;
            HDRSupported = colorData.HighDynamicRangeSupported;
            HDREnabled = colorData.HighDynamicRangeUserEnabled;
            WideColorSupported = colorData.WideColorSupported;
            WideColorEnabled = colorData.WideColorUserEnabled;
            ColorEncoding = (ColorEncoding)colorData.colorEncoding;
            BitsPerColorChannel = colorData.bitsPerColorChannel;
            ColorMode = (ColorMode)colorData.activeColorMode;
            SDRWhiteLevel = sdrData.SDRWhiteLevel;
            SDRWhiteLevelInNits = sdrData.WhiteLevelInNits;
            DisplayId = displayId;
        }

        internal static ColorInfo GetColorInfo(DisplayConfig config, uint displayId)
        {
            int index = config.GetDisplayIndex(displayId);
            LUID adapter = config.PathArray[index].targetInfo.adapterId;
            uint targetId = config.PathArray[index].targetInfo.id;
            var colorData = GetAdvancedColorInfo(adapter, targetId);
            var sdrData = GetSdrInfo(adapter, targetId);
            return new ColorInfo(colorData, sdrData, displayId);
        }

        private static DISPLAYCONFIG_GET_ADVANCED_COLOR_INFO_2 GetAdvancedColorInfo(LUID adapterId, uint targetId)
        {
            var colorInfo = new DISPLAYCONFIG_GET_ADVANCED_COLOR_INFO_2()
            {
                header = new DISPLAYCONFIG_DEVICE_INFO_HEADER()
                {
                    type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_GET_ADVANCED_COLOR_INFO_2,
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

        internal static void ToggleAdvancedColor(Cmdlet command, uint[] displayIds, bool enabled, ColorToggleKind colorKind)
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
                ReturnCode res = SetColorState(adapterId, sourceId, enabled, colorKind);
                if (res != ReturnCode.ERROR_SUCCESS)
                {
                    string action = enabled ? "enable" : "disable";
                    var internalError = new Win32Exception((int)res);
                    string errorMsg = $"Failed to {action} {colorKind} on display {id} due to error: {internalError.Message}";
                    var error = new ApiException(errorMsg, internalError);
                    command.WriteError(new ErrorRecord(error, "FailedToConfigureAdvancedColor", Utils.GetErrorCategory(internalError), id));
                }
            }
        }

        private static ReturnCode SetColorState(LUID adapterId, uint sourceId, bool enabled, ColorToggleKind colorKind)
        {
            ReturnCode res;

            switch (colorKind)
            {
                case ColorToggleKind.AdvancedColor:
                    var advancedColorState = new DISPLAYCONFIG_SET_ADVANCED_COLOR_STATE()
                    {
                        header = new DISPLAYCONFIG_DEVICE_INFO_HEADER()
                        {
                            adapterId = adapterId,
                            id = sourceId,
                            type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_SET_ADVANCED_COLOR_STATE
                        },
                        value = (uint)(enabled ? 1 : 0)
                    };
                    advancedColorState.header.size = (uint)Marshal.SizeOf(advancedColorState);

                    res = NativeMethods.DisplayConfigSetDeviceInfo(ref advancedColorState);
                    break;
                case ColorToggleKind.HDR:
                    var hdrState = new DISPLAYCONFIG_SET_HDR_STATE()
                    {
                        header = new DISPLAYCONFIG_DEVICE_INFO_HEADER()
                        {
                            adapterId = adapterId,
                            id = sourceId,
                            type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_SET_HDR_STATE
                        },
                        value = (uint)(enabled ? 1 : 0)
                    };
                    hdrState.header.size = (uint)Marshal.SizeOf(hdrState);

                    res = NativeMethods.DisplayConfigSetDeviceInfo(ref hdrState);
                    break;
                case ColorToggleKind.WCG:
                    var wcgState = new DISPLAYCONFIG_SET_WCG_STATE()
                    {
                        header = new DISPLAYCONFIG_DEVICE_INFO_HEADER()
                        {
                            adapterId = adapterId,
                            id = sourceId,
                            type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_SET_WCG_STATE
                        },
                        value = (uint)(enabled ? 1 : 0)
                    };
                    wcgState.header.size = (uint)Marshal.SizeOf(wcgState);

                    res = NativeMethods.DisplayConfigSetDeviceInfo(ref wcgState);
                    break;

                default:
                    throw new ArgumentException("Invalid colorKind");
            }

            return res;
        }

        internal static void SetSdrWhiteLevel(Cmdlet cmd, uint[] displayId, uint sdrWhiteLevel)
        {
            var config = DisplayConfig.GetConfig(cmd);
            foreach (uint id in displayId)
            {
                int index;
                try
                {
                    index = config.GetDisplayIndex(id);
                }
                catch (ArgumentException error)
                {
                    cmd.WriteError(Utils.GetInvalidDisplayIdError(error, id));
                    continue;
                }

                try
                {
                    config.ValidatePathIsActive(index);
                }
                catch (Exception error) when (!(error is PipelineStoppedException))
                {
                    cmd.WriteError(new ErrorRecord(error, "InactiveDisplay", ErrorCategory.InvalidArgument, id));
                    continue;
                }

                LUID adapterId = config.PathArray[index].targetInfo.adapterId;
                uint targetId = config.PathArray[index].targetInfo.id;

                try
                {
                    SetSdrWhiteLevel(adapterId, targetId, sdrWhiteLevel);
                }
                catch (Win32Exception error)
                {
                    var errorToShow = new ApiException($"Failed to set SDR white level on display {id} due to error: {error.Message}");
                    cmd.WriteError(new ErrorRecord(errorToShow, "SetSdrWhiteLevelError", Utils.GetErrorCategory(error), id));
                }
            }
        }

        private static void SetSdrWhiteLevel(LUID adapterId, uint targetId, uint whiteLevel)
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