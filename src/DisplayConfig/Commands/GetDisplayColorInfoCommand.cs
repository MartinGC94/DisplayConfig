using MartinGC94.DisplayConfig.API;
using MartinGC94.DisplayConfig.API.ParamAttributes;
using System;
using System.ComponentModel;
using System.Management.Automation;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsCommon.Get, "DisplayColorInfo")]
    [OutputType(typeof(ColorInfo))]
    public sealed class GetDisplayColorInfoCommand : Cmdlet
    {
        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNull]
        [ArgumentCompleter(typeof(DisplayIdCompleter))]
        public uint[] DisplayId { get; set; }

        protected override void ProcessRecord()
        {
            var config = API.DisplayConfig.GetConfig(this);
            uint[] displayIdsToCheck = DisplayId is null
                ? config.GetActiveDisplayIds().ToArray()
                : DisplayId;

            foreach (uint id in displayIdsToCheck)
            {
                try
                {
                    WriteObject(ColorInfo.GetColorInfo(config, id));
                }
                catch (ArgumentException error)
                {
                    WriteError(Utils.GetInvalidDisplayIdError(error, id));
                }
                catch (Win32Exception error)
                {
                    WriteError(new ErrorRecord(error, "APIError", Utils.GetErrorCategory(error), null));
                }
            }
        }
    }
}