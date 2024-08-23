using System;
using System.Management.Automation;
using MartinGC94.DisplayConfig.API;
using MartinGC94.DisplayConfig.Native.Enums;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsCommon.Get, "DisplayInfo")]
    [OutputType(typeof(DisplayInfo))]
    public sealed class GetDisplayInfoCommand : Cmdlet
    {
        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty()]
        [ArgumentCompleter(typeof(DisplayIdCompleter))]
        public uint[] DisplayId { get; set; }

        protected override void EndProcessing()
        {
            var config = API.DisplayConfig.GetConfig(DisplayConfigFlags.QDC_ALL_PATHS);
            if (DisplayId is null)
            {
                foreach (int index in config.AvailablePathIndexes)
                {
                    WriteObject(DisplayInfo.GetDisplayInfo(config, index));
                }
            }
            else
            {
                foreach (uint id in DisplayId)
                {
                    try
                    {
                        WriteObject(DisplayInfo.GetDisplayInfo(config, id));
                    }
                    catch (ArgumentException error)
                    {
                        WriteError(new ErrorRecord(error, "InvalidDisplayId", ErrorCategory.InvalidArgument, id));
                        continue;
                    }
                }
            }
        }
    }
}