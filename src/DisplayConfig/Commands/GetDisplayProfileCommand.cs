using MartinGC94.DisplayConfig.API;
using System.Management.Automation;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsCommon.Get, "DisplayProfile")]
    [OutputType(typeof(TopologyProfile))]
    public sealed class GetDisplayProfileCommand : Cmdlet
    {
        protected override void EndProcessing()
        {
            var config = API.DisplayConfig.GetConfig(Native.Enums.DisplayConfigFlags.QDC_DATABASE_CURRENT);
            WriteObject((TopologyProfile)config.TopologyID);
        }
    }
}