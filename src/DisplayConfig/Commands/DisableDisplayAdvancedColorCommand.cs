using MartinGC94.DisplayConfig.API;
using System.Management.Automation;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsLifecycle.Disable, "DisplayAdvancedColor")]
    public sealed class DisableDisplayAdvancedColorCommand : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        [ArgumentCompleter(typeof(DisplayIdCompleter))]
        public uint[] DisplayId { get; set; }

        protected override void EndProcessing()
        {
            ColorInfo.ToggleAdvancedColor(this, DisplayId, enabled: false);
        }
    }
}