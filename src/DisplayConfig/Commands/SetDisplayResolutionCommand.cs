using MartinGC94.DisplayConfig.API;
using MartinGC94.DisplayConfig.Native.Enums;
using System;
using System.ComponentModel;
using System.Management.Automation;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsCommon.Set, "DisplayResolution", DefaultParameterSetName = "ApplyNow")]
    [OutputType(typeof(API.DisplayConfig), ParameterSetName = new string[] { "Config" })]
    public sealed class SetDisplayResolutionCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Config")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "ApplyNow")]
        [ArgumentCompleter(typeof(DisplayIdCompleter))]
        public uint[] DisplayId { get; set; }

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = "Config")]
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = "ApplyNow")]
        [ArgumentCompleter(typeof(DisplayModeCompleter))]
        public uint Width { get; set; }

        [Parameter(Mandatory = true, Position = 2, ParameterSetName = "Config")]
        [Parameter(Mandatory = true, Position = 2, ParameterSetName = "ApplyNow")]
        [ArgumentCompleter(typeof(DisplayModeCompleter))]
        public uint Height { get; set; }

        [Parameter(ParameterSetName = "Config")]
        public SwitchParameter ChangeAspectRatio { get; set; }

        [Parameter(ParameterSetName = "ApplyNow")]
        public SwitchParameter DontSave { get; set; }

        [Parameter(ParameterSetName = "ApplyNow")]
        public SwitchParameter AllowChanges { get; set; }

        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "Config")]
        public API.DisplayConfig DisplayConfig { get; set; }

        protected override void ProcessRecord()
        {
            bool isConfigParamSet = ParameterSetName.Equals("Config", StringComparison.Ordinal);
            API.DisplayConfig configToModify = isConfigParamSet
                ? DisplayConfig
                : API.DisplayConfig.GetConfig(this);

            foreach (uint id in DisplayId)
            {
                try
                {
                    configToModify.SetDisplayResolution(id, Width, Height);
                }
                catch (Exception error) when (!(error is PipelineStoppedException))
                {
                    WriteError(new ErrorRecord(error, "SetDisplayResolutionFailure", Utils.GetErrorCategory(error), id));
                    continue;
                }

                if (!isConfigParamSet || ChangeAspectRatio)
                {
                    // The display settings app invalidates the modeInfoIdx when changing the resolution.
                    // If this is not done the aspect ratio will be stuck to whatever it was before the change and stretch the image.
                    // When processing a DisplayConfig we generally don't want to invalidate indexes because that prevents further changes
                    // However, the user can opt into this if they know it's the last changes they'll make to ensure the Aspect ratio is properly changed.
                    int displayIndex = configToModify.GetDisplayIndex(id);
                    configToModify.PathArray[displayIndex].targetInfo.modeInfoIdx = API.DisplayConfig.DISPLAYCONFIG_PATH_MODE_IDX_INVALID;
                }
            }

            if (isConfigParamSet)
            {
                WriteObject(configToModify);
                return;
            }

            var flags = SetDisplayConfigFlags.SDC_APPLY |
                SetDisplayConfigFlags.SDC_USE_SUPPLIED_DISPLAY_CONFIG |
                SetDisplayConfigFlags.SDC_SAVE_TO_DATABASE |
                SetDisplayConfigFlags.SDC_VIRTUAL_MODE_AWARE;

            if (AllowChanges)
            {
                flags |= SetDisplayConfigFlags.SDC_ALLOW_CHANGES;
            }

            if (DontSave)
            {
                flags &= ~SetDisplayConfigFlags.SDC_SAVE_TO_DATABASE;
            }

            try
            {
                configToModify.ApplyConfig(flags);
            }
            catch (Win32Exception error)
            {
                var errorToShow = new ErrorRecord(error, "ConfigApplyError", Utils.GetErrorCategory(error), configToModify);
                if (!AllowChanges && error.NativeErrorCode == 1610)
                {
                    errorToShow.ErrorDetails = new ErrorDetails(string.Empty)
                    {
                        RecommendedAction = Utils.AllowChangesRecommendation
                    };
                }

                ThrowTerminatingError(errorToShow);
            }
        }
    }
}