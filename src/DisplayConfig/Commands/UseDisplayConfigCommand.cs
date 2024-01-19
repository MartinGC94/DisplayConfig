using MartinGC94.DisplayConfig.API;
using MartinGC94.DisplayConfig.Native.Enums;
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

        protected override void ProcessRecord()
        {
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
                ThrowTerminatingError(new ErrorRecord(error, "UseDisplayConfigError", Utils.GetErrorCategory(error), DisplayConfig));
            }
        }
    }
}