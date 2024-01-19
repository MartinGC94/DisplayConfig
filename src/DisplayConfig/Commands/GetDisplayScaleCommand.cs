using MartinGC94.DisplayConfig.API;
using MartinGC94.DisplayConfig.Native.Enums;
using MartinGC94.DisplayConfig.Native.Structs;
using System.ComponentModel;
using System;
using System.Management.Automation;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsCommon.Get, "DisplayScale")]
    [OutputType(typeof(DpiScale))]
    public sealed class GetDisplayScaleCommand : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        [ArgumentCompleter(typeof(DisplayIdCompleter))]
        public uint[] DisplayId { get; set; }

        protected override void EndProcessing()
        {
            var config = API.DisplayConfig.GetConfig(DisplayConfigFlags.QDC_ALL_PATHS);
            foreach (uint id in DisplayId)
            {
                int index;
                try
                {
                    index = config.GetDisplayIndex(id);
                }
                catch (Exception error) when (!(error is PipelineStoppedException))
                {
                    WriteError(new ErrorRecord(error, "InvalidDisplayId", ErrorCategory.InvalidArgument, id));
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
                    WriteObject(new DpiScale(dpiConfig));
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