using MartinGC94.DisplayConfig.API;
using MartinGC94.DisplayConfig.Native.Enums;
using System;
using System.ComponentModel;
using System.Management.Automation;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsCommon.Get, "DisplayColorInfo")]
    [OutputType(typeof(ColorInfo))]
    public sealed class GetDisplayColorInfo : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        [ArgumentCompleter(typeof(DisplayIdCompleter))]
        public uint[] DisplayId { get; set; }

        protected override void EndProcessing()
        {
            var config = API.DisplayConfig.GetConfig();
            foreach (uint id in DisplayId)
            {
                try
                {
                    WriteObject(ColorInfo.GetColorInfo(config, id));
                }
                catch (ArgumentException error)
                {
                    WriteError(new ErrorRecord(error, "InvalidDisplayId", ErrorCategory.InvalidArgument, id));
                }
                catch (Win32Exception error)
                {
                    WriteError(new ErrorRecord(error, "APIError", Utils.GetErrorCategory(error), null));
                }
            }
        }
    }
}