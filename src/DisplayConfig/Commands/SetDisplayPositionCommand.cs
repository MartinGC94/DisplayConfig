using MartinGC94.DisplayConfig.API;
using MartinGC94.DisplayConfig.Native.Enums;
using System;
using System.ComponentModel;
using System.Management.Automation;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsCommon.Set, "DisplayPosition", DefaultParameterSetName = "Position")]
    [OutputType(typeof(API.DisplayConfig))]
    public sealed class SetDisplayPositionCommand : PSCmdlet
    {
        [Parameter(ValueFromPipeline = true)]
        [ValidateNotNull()]
        public API.DisplayConfig DisplayConfig { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Position")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "OffsetFromDisplay")]
        [ArgumentCompleter(typeof(DisplayIdCompleter))]
        public uint DisplayId { get; set; }

        [Parameter(ParameterSetName = "Position")]
        public int XPosition { get; set; } = int.MaxValue;

        [Parameter(ParameterSetName = "Position")]
        public int YPosition { get; set; } = int.MaxValue;

        [Parameter(ParameterSetName = "Position")]
        public SwitchParameter AsOffset { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "OffsetFromDisplay")]
        public uint RelativeDisplayId { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "OffsetFromDisplay")]
        public RelativePosition Position { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "LeftToRight")]
        public uint[] LeftToRightDisplayIds { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "SwapDisplays")]
        [ArgumentCompleter(typeof(DisplayIdCompleter))]
        [ValidateCount(2, 2)]
        public uint[] SwapDisplay { get; set; }

        [Parameter()]
        public SwitchParameter DontSave { get; set; }

        [Parameter()]
        public SwitchParameter AllowChanges { get; set; }

        protected override void ProcessRecord()
        {
            API.DisplayConfig configToModify;
            bool isConfigParamSet;
            if (DisplayConfig is null)
            {
                configToModify = API.DisplayConfig.GetConfig();
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
                        configToModify.MoveDisplayPosition(DisplayId, RelativeDisplayId, Position);
                        break;

                    case "LeftToRight":
                        configToModify.SetDisplayPositionLeftToRight(LeftToRightDisplayIds);
                        break;

                    case "SwapDisplays":
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

            var flags = SetDisplayConfigFlags.SDC_APPLY |
                SetDisplayConfigFlags.SDC_USE_SUPPLIED_DISPLAY_CONFIG |
                SetDisplayConfigFlags.SDC_SAVE_TO_DATABASE |
                SetDisplayConfigFlags.SDC_VIRTUAL_MODE_AWARE;
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