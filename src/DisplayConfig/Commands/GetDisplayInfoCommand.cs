using System;
using System.Management.Automation;
using MartinGC94.DisplayConfig.API;

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

        [Parameter(ValueFromPipeline = true)]
        [ValidateNotNull()]
        public API.DisplayConfig DisplayConfig { get; set; }

        protected override void ProcessRecord()
        {
            var config = DisplayConfig is null
                ? API.DisplayConfig.GetConfig(this)
                : DisplayConfig;

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
                        WriteError(Utils.GetInvalidDisplayIdError(error, id));
                        continue;
                    }
                }
            }
        }
    }
}