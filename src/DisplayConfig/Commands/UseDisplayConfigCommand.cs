using MartinGC94.DisplayConfig.API;
using MartinGC94.DisplayConfig.Native.Enums;
using System;
using System.ComponentModel;
using System.Management.Automation;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsOther.Use, "DisplayConfig")]
    public sealed class UseDisplayConfigCommand : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public API.DisplayConfig DisplayConfig { get; set; }

        [Parameter()]
        public SwitchParameter AllowChanges { get; set; }

        [Parameter()]
        public SwitchParameter DontSave { get; set; }

        [Parameter()]
        public SetDisplayConfigFlags Flags { get; set; } =
            SetDisplayConfigFlags.SDC_APPLY |
            SetDisplayConfigFlags.SDC_USE_SUPPLIED_DISPLAY_CONFIG |
            SetDisplayConfigFlags.SDC_SAVE_TO_DATABASE;

        [Parameter()]
        public SwitchParameter UpdateAdapterIds { get; set; }

        protected override void ProcessRecord()
        {

            if (UpdateAdapterIds)
            {
                try
                {
                    DisplayConfig.UpdateAdapterIds();
                }
                catch (Exception error)
                {
                    ThrowTerminatingError(new ErrorRecord(error, "AdapterUpdateError", ErrorCategory.NotSpecified, DisplayConfig));
                }
            }

            if (AllowChanges)
            {
                Flags |= SetDisplayConfigFlags.SDC_ALLOW_CHANGES;
            }

            if (DontSave)
            {
                Flags &= ~SetDisplayConfigFlags.SDC_SAVE_TO_DATABASE;
            }

            try
            {
                DisplayConfig.ApplyConfig(Flags);
            }
            catch (Win32Exception error)
            {
                var errorToShow = new ErrorRecord(error, "ConfigApplyError", Utils.GetErrorCategory(error), DisplayConfig);
                if (!AllowChanges && error.NativeErrorCode == 1610)
                {
                    errorToShow.ErrorDetails = new ErrorDetails(string.Empty)
                    {
                        RecommendedAction = Utils.AllowChangesRecommendation
                    };
                }
                else if (!UpdateAdapterIds && error.NativeErrorCode == 87)
                {
                    errorToShow.ErrorDetails = new ErrorDetails(string.Empty)
                    {
                        RecommendedAction = Utils.UpdateAdapterIdsRecommendation
                    };
                }

                ThrowTerminatingError(errorToShow);
            }
        }
    }
}