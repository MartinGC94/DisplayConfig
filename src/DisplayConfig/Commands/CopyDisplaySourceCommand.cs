using MartinGC94.DisplayConfig.API;
using MartinGC94.DisplayConfig.Native.Enums;
using System;
using System.ComponentModel;
using System.Management.Automation;
using System.Collections.Generic;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsCommon.Copy, "DisplaySource", DefaultParameterSetName = "ApplyNow")]
    [OutputType(typeof(API.DisplayConfig), ParameterSetName = new string[] { "Config" })]
    public sealed class CopyDisplaySourceCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "Config")]
        public API.DisplayConfig DisplayConfig { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Config")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "ApplyNow")]
        [ArgumentCompleter(typeof(DisplayIdCompleter))]
        [Alias("SourceDisplayId")]
        public uint DisplayId { get; set; }

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = "Config")]
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = "ApplyNow")]
        [ArgumentCompleter(typeof(DisplayIdCompleter))]
        public uint[] DestinationDisplayId { get; set; }

        [Parameter(ParameterSetName = "ApplyNow")]
        public SwitchParameter DontSave { get; set; }

        protected override void ProcessRecord()
        {
            bool isConfigParamSet = ParameterSetName.Equals("Config", StringComparison.Ordinal);
            API.DisplayConfig configToModify = isConfigParamSet
                ? DisplayConfig
                : API.DisplayConfig.GetConfig(DisplayConfigFlags.QDC_ALL_PATHS);

            try
            {
                configToModify.CloneDisplay(DisplayId, new HashSet<uint>(DestinationDisplayId));
            }
            catch (Exception error) when (!(error is PipelineStoppedException))
            {
                ThrowTerminatingError(new ErrorRecord(error, "CloneConfigError", Utils.GetErrorCategory(error), configToModify));
            }

            if (isConfigParamSet)
            {
                WriteObject(configToModify);
                return;
            }

            var flags = SetDisplayConfigFlags.SDC_APPLY | SetDisplayConfigFlags.SDC_USE_SUPPLIED_DISPLAY_CONFIG | SetDisplayConfigFlags.SDC_SAVE_TO_DATABASE;

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
