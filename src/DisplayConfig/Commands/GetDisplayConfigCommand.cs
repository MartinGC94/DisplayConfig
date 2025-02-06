using System.Management.Automation;
using MartinGC94.DisplayConfig.Native.Enums;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsCommon.Get, "DisplayConfig")]
    [OutputType(typeof(API.DisplayConfig))]
    public sealed class GetDisplayConfigCommand : Cmdlet
    {
        [Parameter(Position = 0)]
        public DisplayConfigFlags Flags { get; set; } = DisplayConfigFlags.QDC_ALL_PATHS | DisplayConfigFlags.QDC_VIRTUAL_MODE_AWARE;

        protected override void EndProcessing()
        {
            WriteObject(API.DisplayConfig.GetConfig(this, Flags));
        }
    }
}