using MartinGC94.DisplayConfig.API;
using System;
using System.Management.Automation;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsLifecycle.Disable, "Display", DefaultParameterSetName = "ApplyNow")]
    [OutputType(typeof(API.DisplayConfig), ParameterSetName = new string[] { "Config" })]
    public sealed class DisableDisplayCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "ApplyNow")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Config")]
        [ArgumentCompleter(typeof(DisplayIdCompleter))]
        public uint[] DisplayId { get; set; }

        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "Config")]
        public API.DisplayConfig DisplayConfig { get; set; }

        protected override void ProcessRecord()
        {
            if (ParameterSetName.Equals("ApplyNow", StringComparison.Ordinal))
            {
                API.DisplayConfig.EnableDisableDisplay(this, displaysToEnable: null, DisplayId);
                return;
            }

            foreach (uint id in DisplayId)
            {
                try
                {
                    DisplayConfig.DisableDisplay(id);
                }
                catch (Exception error) when (!(error is PipelineStoppedException))
                {
                    WriteError(new ErrorRecord(error, "ConfigUpdateFailure", Utils.GetErrorCategory(error), id));
                }
            }

            WriteObject(DisplayConfig);
        }
    }
}