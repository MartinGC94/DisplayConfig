using MartinGC94.DisplayConfig.API;
using MartinGC94.DisplayConfig.Native.Structs;
using System.ComponentModel;
using System;
using System.Management.Automation;
using MartinGC94.DisplayConfig.API.ParamAttributes;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsCommon.Get, "DisplayScale")]
    [OutputType(typeof(DpiScale))]
    public sealed class GetDisplayScaleCommand : Cmdlet
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
                int index;
                try
                {
                    index = config.GetDisplayIndex(id);
                }
                catch (ArgumentException error)
                {
                    WriteError(Utils.GetInvalidDisplayIdError(error, id));
                    continue;
                }

                try
                {
                    config.ValidatePathIsActive(index);
                }
                catch (Exception error) when (!(error is PipelineStoppedException))
                {
                    WriteError(new ErrorRecord(error, "InactiveDisplay", ErrorCategory.InvalidArgument, id));
                    continue;
                }

                LUID adapterId = config.PathArray[index].sourceInfo.adapterId;
                uint sourceId = config.PathArray[index].sourceInfo.id;
                DpiConfigGet dpiConfig;
                try
                {
                    dpiConfig = DpiScale.GetDpiInfo(adapterId, sourceId);
                    WriteObject(new DpiScale(dpiConfig, id));
                }
                catch (Win32Exception error)
                {
                    WriteError(new ErrorRecord(error, "GetDpiInfoError", Utils.GetErrorCategory(error), id));
                    continue;
                }
            }
        }
    }
}