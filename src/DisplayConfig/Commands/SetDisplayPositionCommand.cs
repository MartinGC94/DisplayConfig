using MartinGC94.DisplayConfig.API;
using MartinGC94.DisplayConfig.Native.Enums;
using System;
using System.ComponentModel;
using System.Management.Automation;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsCommon.Set, "DisplayPosition")]
    [OutputType(typeof(API.DisplayConfig), ParameterSetName = new string[] { "PositionConfig", "OffsetFromDisplayConfig", "LeftToRightConfig", "SwapDisplaysConfig" })]
    public sealed class SetDisplayPositionCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "PositionConfig")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "OffsetFromDisplayConfig")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "LeftToRightConfig")]
        public API.DisplayConfig DisplayConfig { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Position")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "PositionConfig")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "OffsetFromDisplay")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "OffsetFromDisplayConfig")]
        [ArgumentCompleter(typeof(DisplayIdCompleter))]
        public uint DisplayId { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Position")]
        [Parameter(Mandatory = true, ParameterSetName = "PositionConfig")]
        public int XPosition { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Position")]
        [Parameter(Mandatory = true, ParameterSetName = "PositionConfig")]
        public int YPosition { get; set; }

        [Parameter(ParameterSetName = "Position")]
        [Parameter(ParameterSetName = "PositionConfig")]
        public SwitchParameter AsOffset { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "OffsetFromDisplay")]
        [Parameter(Mandatory = true, ParameterSetName = "OffsetFromDisplayConfig")]
        public uint RelativeDisplayId { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "OffsetFromDisplay")]
        [Parameter(Mandatory = true, ParameterSetName = "OffsetFromDisplayConfig")]
        public RelativePosition Position { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "LeftToRight")]
        [Parameter(Mandatory = true, ParameterSetName = "LeftToRightConfig")]
        public uint[] LeftToRightDisplayIds { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "SwapDisplays")]
        [Parameter(Mandatory = true, ParameterSetName = "SwapDisplaysConfig")]
        [ValidateCount(2, 2)]
        public uint[] SwapDisplay { get; set; }

        [Parameter(ParameterSetName = "Position")]
        [Parameter(ParameterSetName = "OffsetFromDisplay")]
        [Parameter(ParameterSetName = "LeftToRight")]
        [Parameter(ParameterSetName = "SwapDisplays")]
        public SwitchParameter DontSave { get; set; }

        [Parameter(ParameterSetName = "Position")]
        [Parameter(ParameterSetName = "OffsetFromDisplay")]
        [Parameter(ParameterSetName = "LeftToRight")]
        [Parameter(ParameterSetName = "SwapDisplays")]
        public SwitchParameter AllowChanges { get; set; }

        [Parameter(ParameterSetName = "Position")]
        [Parameter(ParameterSetName = "OffsetFromDisplay")]
        [Parameter(ParameterSetName = "LeftToRight")]
        [Parameter(ParameterSetName = "SwapDisplays")]
        public SwitchParameter ApplyNow { get; set; }

        protected override void ProcessRecord()
        {
            API.DisplayConfig configToModify;
            bool isConfigParamSet;
            if (DisplayConfig is null)
            {
                configToModify = API.DisplayConfig.GetConfig(DisplayConfigFlags.QDC_ALL_PATHS);
                isConfigParamSet = false;
            }
            else
            {
                configToModify = DisplayConfig;
                isConfigParamSet = true;
            }

            try
            {
                switch (ParameterSetName)
                {
                    case "Position":
                    case "PositionConfig":
                        if (AsOffset)
                        {
                            configToModify.MoveDisplayPosition(DisplayId, XPosition, YPosition);
                        }
                        else
                        {
                            configToModify.SetDisplayPosition(DisplayId, XPosition, YPosition);
                        }
                        break;

                    case "OffsetFromDisplay":
                    case "OffsetFromDisplayConfig":
                        configToModify.MoveDisplayPosition(DisplayId, RelativeDisplayId, Position);
                        break;

                    case "LeftToRight":
                    case "LeftToRightConfig":
                        configToModify.SetDisplayPositionLeftToRight(LeftToRightDisplayIds);
                        break;

                    case "SwapDisplays":
                    case "SwapDisplaysConfig":
                        configToModify.SwapDisplayPosition(SwapDisplay[0], SwapDisplay[1]);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception error) when (!(error is PipelineStoppedException))
            {
                WriteError(new ErrorRecord(error, "PositionConfigError", Utils.GetErrorCategory(error), configToModify));
            }

            if (isConfigParamSet)
            {
                WriteObject(configToModify);
                return;
            }

            var flags = SetDisplayConfigFlags.SDC_APPLY | SetDisplayConfigFlags.SDC_USE_SUPPLIED_DISPLAY_CONFIG | SetDisplayConfigFlags.SDC_SAVE_TO_DATABASE;
            if (AllowChanges)
            {
                flags |= SetDisplayConfigFlags.SDC_ALLOW_CHANGES;
            }

            if (DontSave)
            {
                flags &= ~SetDisplayConfigFlags.SDC_SAVE_TO_DATABASE;
            }

            try
            {
                configToModify.ApplyConfig(flags);
            }
            catch (Win32Exception error)
            {
                ThrowTerminatingError(new ErrorRecord(error, "ConfigApplyError", Utils.GetErrorCategory(error), configToModify));
            }
        }
    }
}