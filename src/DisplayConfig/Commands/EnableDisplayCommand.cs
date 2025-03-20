using System.Management.Automation;
using MartinGC94.DisplayConfig.API;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsLifecycle.Enable, "Display")]
    public sealed class EnableDisplayCommand : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        [ArgumentCompleter(typeof(DisplayIdCompleter))]
        public uint[] DisplayId { get; set; }

        [Parameter()]
        [ArgumentCompleter(typeof(DisplayIdCompleter))]
        public uint[] DisplayIdToDisable { get; set; }

        [Parameter()]
        public SwitchParameter AsClone { get; set; }

        protected override void EndProcessing()
        {
            API.DisplayConfig.EnableDisableDisplay(this, DisplayId, DisplayIdToDisable, AsClone);
        }
    }
}