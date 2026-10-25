using MartinGC94.DisplayConfig.API;
using MartinGC94.DisplayConfig.API.ParamAttributes;
using System.Management.Automation;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsCommon.Set, "DisplayColor")]
    public sealed class SetDisplayColorCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true)]
        [ArgumentCompleter(typeof(DisplayIdCompleter))]
        public uint[] DisplayId { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public bool WideColorEnabled { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public bool AdvancedColorActive { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public bool HDREnabled { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public uint SDRWhiteLevel { get; set; }

        protected override void ProcessRecord()
        {
            if (MyInvocation.BoundParameters.ContainsKey(nameof(WideColorEnabled)))
            {
                ColorInfo.ToggleAdvancedColor(this, DisplayId, WideColorEnabled, ColorToggleKind.WCG);
            }

            if (MyInvocation.BoundParameters.ContainsKey(nameof(AdvancedColorActive)))
            {
                ColorInfo.ToggleAdvancedColor(this, DisplayId, AdvancedColorActive, ColorToggleKind.AdvancedColor);
            }

            if (MyInvocation.BoundParameters.ContainsKey(nameof(HDREnabled)))
            {
                ColorInfo.ToggleAdvancedColor(this, DisplayId, HDREnabled, ColorToggleKind.HDR);
            }

            if (MyInvocation.BoundParameters.ContainsKey(nameof(SDRWhiteLevel)))
            {
                ColorInfo.SetSdrWhiteLevel(this, DisplayId, SDRWhiteLevel);
            }
        }
    }
}