using MartinGC94.DisplayConfig.API;
using MartinGC94.DisplayConfig.Native.Enums;
using System.ComponentModel;
using System;
using System.Management.Automation;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsCommon.Get, "DisplayHDR", DefaultParameterSetName = "DisplaySpecific")]
    [OutputType(typeof(HDRDisplayInfo), ParameterSetName = new string[] { "DisplaySpecific" })]
    [OutputType(typeof(HDRGlobalInfo), ParameterSetName = new string[] {"Global" })]
    public sealed class GetDisplayHDRCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "DisplaySpecific")]
        [ArgumentCompleter(typeof(DisplayIdCompleter))]
        public uint[] DisplayId { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Global")]
        public SwitchParameter Global { get; set; }

        protected override void EndProcessing()
        {
            if (Global)
            {
                WriteObject(HDRGlobalInfo.GetGlobalSettings(InvokeCommand));
            }

            if (ParameterSetName == "DisplaySpecific")
            {
                var config = API.DisplayConfig.GetConfig();
                foreach (uint id in DisplayId)
                {
                    try
                    {
                        var colorInfo = ColorInfo.GetColorInfo(config, id);
                        WriteObject(new HDRDisplayInfo(colorInfo, id));
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
}