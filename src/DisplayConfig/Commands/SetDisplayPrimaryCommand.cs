using MartinGC94.DisplayConfig.API;
using MartinGC94.DisplayConfig.Native.Enums;
using System;
using System.ComponentModel;
using System.Management.Automation;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsCommon.Set, "DisplayPrimary", DefaultParameterSetName = "ApplyNow")]
    [OutputType(typeof(API.DisplayConfig), ParameterSetName = new string[] { "Config" })]
    public sealed class SetDisplayPrimaryCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Config")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "ApplyNow")]
        [ArgumentCompleter(typeof(DisplayIdCompleter))]
        public uint DisplayId { get; set; }

        [Parameter(ParameterSetName = "ApplyNow")]
        public SwitchParameter DontSave { get; set; }

        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "Config")]
        public API.DisplayConfig DisplayConfig { get; set; }

        protected override void ProcessRecord()
        {
            bool isConfigParamSet = ParameterSetName.Equals("Config", StringComparison.Ordinal);
            API.DisplayConfig configToModify = isConfigParamSet
                ? DisplayConfig
                : API.DisplayConfig.GetConfig(this);

            configToModify.SetPrimaryDisplay(DisplayId);

            if (isConfigParamSet)
            {
                WriteObject(configToModify);
                return;
            }

            var flags = SetDisplayConfigFlags.SDC_APPLY |
                SetDisplayConfigFlags.SDC_USE_SUPPLIED_DISPLAY_CONFIG |
                SetDisplayConfigFlags.SDC_SAVE_TO_DATABASE |
                SetDisplayConfigFlags.SDC_VIRTUAL_MODE_AWARE;

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