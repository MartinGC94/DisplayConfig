using MartinGC94.DisplayConfig.Native.Enums;
using MartinGC94.DisplayConfig.Native.Structs;
using System;
using System.Management.Automation;
using System.Collections;

namespace MartinGC94.DisplayConfig.API
{
    public class DisplayConfigConverter : PSTypeConverter
    {
        public override bool CanConvertFrom(object sourceValue, Type destinationType)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvertTo(object sourceValue, Type destinationType)
        {
            throw new NotImplementedException();
        }

        public override object ConvertFrom(object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase)
        {
            throw new NotImplementedException();
        }

        public override object ConvertTo(object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvertFrom(PSObject sourceValue, Type destinationType)
        {
            return destinationType == typeof(DisplayConfig) &&
                $"Deserialized.{typeof(DisplayConfig).FullName}".Equals(sourceValue.TypeNames[0], StringComparison.Ordinal);
        }

        public override object ConvertFrom(PSObject sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase)
        {
            var result = new DisplayConfig()
            {
                AvailablePathIndexes = LanguagePrimitives.ConvertTo<int[]>(sourceValue.Properties["AvailablePathIndexes"].Value),
                Flags = LanguagePrimitives.ConvertTo<DisplayConfigFlags>(sourceValue.Properties["Flags"].Value),
                TopologyID = LanguagePrimitives.ConvertTo<DISPLAYCONFIG_TOPOLOGY_ID>(sourceValue.Properties["TopologyID"].Value),
                AvailablePathNames = ConvertDISPLAYCONFIG_TARGET_DEVICE_NAME(sourceValue.Properties["AvailablePathNames"].Value),
                ModeArray = ConvertDISPLAYCONFIG_MODE_INFO(sourceValue.Properties["ModeArray"].Value),
                PathArray = ConvertDISPLAYCONFIG_PATH_INFO(sourceValue.Properties["PathArray"].Value),
                isImportedConfig = true
            };
            return result;
        }

        private static LUID ConvertLUID(PSObject obj)
        {
            var result = new LUID()
            {
                HighPart = LanguagePrimitives.ConvertTo<int>(obj.Properties["HighPart"].Value),
                LowPart = LanguagePrimitives.ConvertTo<uint>(obj.Properties["LowPart"].Value)
            };
            return result;
        }

        private static DISPLAYCONFIG_DEVICE_INFO_HEADER ConvertDISPLAYCONFIG_DEVICE_INFO_HEADER(PSObject obj)
        {
            var result = new DISPLAYCONFIG_DEVICE_INFO_HEADER()
            {
                adapterId = ConvertLUID((PSObject)obj.Properties["adapterId"].Value),
                id = LanguagePrimitives.ConvertTo<uint>(obj.Properties["id"].Value),
                size = LanguagePrimitives.ConvertTo<uint>(obj.Properties["size"].Value),
                type = LanguagePrimitives.ConvertTo<DISPLAYCONFIG_DEVICE_INFO_TYPE>(obj.Properties["type"].Value)
            };
            return result;
        }

        private static DISPLAYCONFIG_TARGET_DEVICE_NAME_FLAGS ConvertDISPLAYCONFIG_TARGET_DEVICE_NAME_FLAGS(PSObject obj)
        {
            var result = new DISPLAYCONFIG_TARGET_DEVICE_NAME_FLAGS()
            {
                value = LanguagePrimitives.ConvertTo<uint>(obj.Properties["value"].Value)
            };
            return result;
        }

        private static DISPLAYCONFIG_TARGET_DEVICE_NAME[] ConvertDISPLAYCONFIG_TARGET_DEVICE_NAME(object obj)
        {
            var collection = LanguagePrimitives.ConvertTo<ArrayList>(obj);
            var result = new DISPLAYCONFIG_TARGET_DEVICE_NAME[collection.Count];
            for (int i = 0; i < collection.Count; i++)
            {
                var item = collection[i] as PSObject;
                result[i].connectorInstance = LanguagePrimitives.ConvertTo<uint>(item.Properties["connectorInstance"].Value);
                result[i].edidManufactureId = LanguagePrimitives.ConvertTo<ushort>(item.Properties["edidManufactureId"].Value);
                result[i].edidProductCodeId = LanguagePrimitives.ConvertTo<ushort>(item.Properties["edidProductCodeId"].Value);
                result[i].flags = ConvertDISPLAYCONFIG_TARGET_DEVICE_NAME_FLAGS((PSObject)item.Properties["flags"].Value);
                result[i].header = ConvertDISPLAYCONFIG_DEVICE_INFO_HEADER((PSObject)item.Properties["header"].Value);
                result[i].monitorDevicePath = LanguagePrimitives.ConvertTo<string>(item.Properties["monitorDevicePath"].Value);
                result[i].monitorFriendlyDeviceName = LanguagePrimitives.ConvertTo<string>(item.Properties["monitorFriendlyDeviceName"].Value);
                result[i].outputTechnology = LanguagePrimitives.ConvertTo<DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY>(item.Properties["outputTechnology"].Value);
            }
            return result;
        }

        private static DISPLAYCONFIG_MODE_INFO[] ConvertDISPLAYCONFIG_MODE_INFO(object obj)
        {
            var collection = LanguagePrimitives.ConvertTo<ArrayList>(obj);
            var result = new DISPLAYCONFIG_MODE_INFO[collection.Count];
            for (int i = 0; i < collection.Count; i++)
            {
                var item = collection[i] as PSObject;
                result[i].adapterId = ConvertLUID((PSObject)item.Properties["adapterId"].Value);
                result[i].id = LanguagePrimitives.ConvertTo<uint>(item.Properties["id"].Value);
                result[i].infoType = LanguagePrimitives.ConvertTo<DISPLAYCONFIG_MODE_INFO_TYPE>(item.Properties["infoType"].Value);
                var modeInfo = (PSObject)item.Properties["modeInfo"].Value;
                switch (result[i].infoType)
                {
                    case DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_SOURCE:
                        result[i].modeInfo.sourceMode = ConvertDISPLAYCONFIG_SOURCE_MODE((PSObject)modeInfo.Properties["sourceMode"].Value);
                        break;

                    case DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_TARGET:
                        result[i].modeInfo.targetMode = ConvertDISPLAYCONFIG_TARGET_MODE((PSObject)modeInfo.Properties["targetMode"].Value);
                        break;

                    case DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_DESKTOP_IMAGE:
                        result[i].modeInfo.desktopImageInfo = ConvertDISPLAYCONFIG_DESKTOP_IMAGE_INFO((PSObject)modeInfo.Properties["desktopImageInfo"].Value);
                        break;

                    default:
                        break;
                }
            }

            return result;
        }

        private static DISPLAYCONFIG_SOURCE_MODE ConvertDISPLAYCONFIG_SOURCE_MODE(PSObject obj)
        {
            var result = new DISPLAYCONFIG_SOURCE_MODE()
            {
                height = LanguagePrimitives.ConvertTo<uint>(obj.Properties["height"].Value),
                width = LanguagePrimitives.ConvertTo<uint>(obj.Properties["width"].Value),
                pixelFormat = LanguagePrimitives.ConvertTo<DISPLAYCONFIG_PIXELFORMAT>(obj.Properties["pixelFormat"].Value),
                position = ConvertPOINTL((PSObject)obj.Properties["position"].Value)
            };
            return result;
        }

        private static POINTL ConvertPOINTL(PSObject obj)
        {
            var result = new POINTL()
            {
                x = LanguagePrimitives.ConvertTo<int>(obj.Properties["x"].Value),
                y = LanguagePrimitives.ConvertTo<int>(obj.Properties["y"].Value)
            };
            return result;
        }

        private static DISPLAYCONFIG_TARGET_MODE ConvertDISPLAYCONFIG_TARGET_MODE(PSObject obj)
        {
            var result = new DISPLAYCONFIG_TARGET_MODE()
            {
                targetVideoSignalInfo = ConvertDISPLAYCONFIG_VIDEO_SIGNAL_INFO((PSObject)obj.Properties["targetVideoSignalInfo"].Value)
            };
            return result;
        }

        private static DISPLAYCONFIG_VIDEO_SIGNAL_INFO ConvertDISPLAYCONFIG_VIDEO_SIGNAL_INFO(PSObject obj)
        {
            var result = new DISPLAYCONFIG_VIDEO_SIGNAL_INFO()
            {
                activeSize = ConvertDISPLAYCONFIG_2DREGION((PSObject)obj.Properties["activeSize"].Value),
                hSyncFreq = ConvertDISPLAYCONFIG_RATIONAL((PSObject)obj.Properties["hSyncFreq"].Value),
                pixelRate = LanguagePrimitives.ConvertTo<ulong>(obj.Properties["pixelRate"].Value),
                scanLineOrdering = LanguagePrimitives.ConvertTo<DISPLAYCONFIG_SCANLINE_ORDERING>(obj.Properties["scanLineOrdering"].Value),
                totalSize = ConvertDISPLAYCONFIG_2DREGION((PSObject)obj.Properties["totalSize"].Value),
                videoStandardRaw = LanguagePrimitives.ConvertTo<uint>(obj.Properties["videoStandardRaw"].Value),
                vSyncFreq = ConvertDISPLAYCONFIG_RATIONAL((PSObject)obj.Properties["vSyncFreq"].Value)
            };
            return result;
        }

        private static DISPLAYCONFIG_2DREGION ConvertDISPLAYCONFIG_2DREGION(PSObject obj)
        {
            var result = new DISPLAYCONFIG_2DREGION()
            {
                cx = LanguagePrimitives.ConvertTo<uint>(obj.Properties["cx"].Value),
                cy = LanguagePrimitives.ConvertTo<uint>(obj.Properties["cy"].Value),
            };
            return result;
        }

        private static DISPLAYCONFIG_RATIONAL ConvertDISPLAYCONFIG_RATIONAL(PSObject obj)
        {
            var result = new DISPLAYCONFIG_RATIONAL()
            {
                Denominator = LanguagePrimitives.ConvertTo<uint>(obj.Properties["Denominator"].Value),
                Numerator = LanguagePrimitives.ConvertTo<uint>(obj.Properties["Numerator"].Value)
            };
            return result;
        }

        private static DISPLAYCONFIG_DESKTOP_IMAGE_INFO ConvertDISPLAYCONFIG_DESKTOP_IMAGE_INFO(PSObject obj)
        {
            var result = new DISPLAYCONFIG_DESKTOP_IMAGE_INFO()
            {
                desktopImageClip = ConvertRECTL((PSObject)obj.Properties["desktopImageClip"].Value),
                desktopImageRegion = ConvertRECTL((PSObject)obj.Properties["desktopImageRegion"].Value),
                pathSourceSize = ConvertPOINTL((PSObject)obj.Properties["pathSourceSize"].Value)
            };
            return result;
        }

        private static RECTL ConvertRECTL(PSObject obj)
        {
            var result = new RECTL()
            {
                bottom = LanguagePrimitives.ConvertTo<int>(obj.Properties["bottom"].Value),
                left = LanguagePrimitives.ConvertTo<int>(obj.Properties["left"].Value),
                right = LanguagePrimitives.ConvertTo<int>(obj.Properties["right"].Value),
                top = LanguagePrimitives.ConvertTo<int>(obj.Properties["top"].Value)
            };
            return result;
        }

        private static DISPLAYCONFIG_PATH_INFO[] ConvertDISPLAYCONFIG_PATH_INFO(object obj)
        {
            var collection = LanguagePrimitives.ConvertTo<ArrayList>(obj);
            var result = new DISPLAYCONFIG_PATH_INFO[collection.Count];
            for (int i = 0; i < collection.Count; i++)
            {
                var item = collection[i] as PSObject;
                result[i].flags = LanguagePrimitives.ConvertTo<PathInfoFlags>(item.Properties["flags"].Value);
                result[i].sourceInfo = ConvertDISPLAYCONFIG_PATH_SOURCE_INFO((PSObject)item.Properties["sourceInfo"].Value);
                result[i].targetInfo = ConvertDISPLAYCONFIG_PATH_TARGET_INFO((PSObject)item.Properties["targetInfo"].Value);
            }
            return result;
        }

        private static DISPLAYCONFIG_PATH_SOURCE_INFO ConvertDISPLAYCONFIG_PATH_SOURCE_INFO(PSObject obj)
        {
            var result = new DISPLAYCONFIG_PATH_SOURCE_INFO()
            {
                adapterId = ConvertLUID((PSObject)obj.Properties["adapterId"].Value),
                id = LanguagePrimitives.ConvertTo<uint>(obj.Properties["id"].Value),
                modeInfoIdx = LanguagePrimitives.ConvertTo<uint>(obj.Properties["modeInfoIdx"].Value),
                statusFlags = LanguagePrimitives.ConvertTo<uint>(obj.Properties["statusFlags"].Value)
            };
            return result;
        }

        private static DISPLAYCONFIG_PATH_TARGET_INFO ConvertDISPLAYCONFIG_PATH_TARGET_INFO(PSObject obj)
        {
            var result = new DISPLAYCONFIG_PATH_TARGET_INFO()
            {
                adapterId = ConvertLUID((PSObject)obj.Properties["adapterId"].Value),
                id = LanguagePrimitives.ConvertTo<uint>(obj.Properties["id"].Value),
                modeInfoIdx = LanguagePrimitives.ConvertTo<uint>(obj.Properties["modeInfoIdx"].Value),
                outputTechnology = LanguagePrimitives.ConvertTo<DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY>(obj.Properties["outputTechnology"].Value),
                refreshRate = ConvertDISPLAYCONFIG_RATIONAL((PSObject)obj.Properties["refreshRate"].Value),
                rotation = LanguagePrimitives.ConvertTo<DISPLAYCONFIG_ROTATION>(obj.Properties["rotation"].Value),
                scaling = LanguagePrimitives.ConvertTo<DISPLAYCONFIG_SCALING>(obj.Properties["scaling"].Value),
                scanlineOrdering = LanguagePrimitives.ConvertTo<DISPLAYCONFIG_SCANLINE_ORDERING>(obj.Properties["scanlineOrdering"].Value),
                statusFlags = LanguagePrimitives.ConvertTo<DisplayConfigStatusFlags>(obj.Properties["statusFlags"].Value),
                targetAvailable = LanguagePrimitives.ConvertTo<bool>(obj.Properties["targetAvailable"].Value)
            };
            return result;
        }
    }
}