using MartinGC94.DisplayConfig.API;
using MartinGC94.DisplayConfig.Native.Structs;
using System;
using System.Management.Automation;
using System.ComponentModel;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsCommon.Set, "DisplayScale")]
    public sealed class SetDisplayScaleCommand : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        [ArgumentCompleter(typeof(DisplayIdCompleter))]
        public uint[] DisplayId { get; set; }

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = "UserDefined")]
        [ValidateSet("100", "125", "150", "175", "200", "225", "250", "300", "350", "400", "450", "500")]
        public uint Scale { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Recommended")]
        public SwitchParameter Recommended { get; set; }

        protected override void EndProcessing()
        {
            int dpiIndex = Array.IndexOf(DpiScale.DpiValues, Scale);
            if (dpiIndex == -1 && !Recommended)
            {
                ThrowTerminatingError(new ErrorRecord(
                    new ArgumentException("The Recommended switch cannot be set to false"),
                    "RecommendedSetToFalse",
                    ErrorCategory.InvalidArgument,
                    null));
            }

            var config = API.DisplayConfig.GetConfig(this);
            foreach (uint id in DisplayId)
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
                }
                catch (Win32Exception error)
                {
                    WriteError(new ErrorRecord(error, "GetDpiInfoError", Utils.GetErrorCategory(error), id));
                    continue;
                }

                try
                {
                    if (Recommended)
                    {
                        DpiScale.SetDpiScale(adapterId, sourceId, 0);
                    }
                    else
                    {
                        DpiScale.SetDpiScale(adapterId, sourceId, dpiIndex + dpiConfig.minRelativeScale);
                    }
                }
                catch (Win32Exception error)
                {
                    WriteError(new ErrorRecord(error, "SetDpiError", Utils.GetErrorCategory(error), id));
                }
            }
        }
    }
}