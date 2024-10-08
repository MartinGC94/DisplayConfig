﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management.Automation;
using MartinGC94.DisplayConfig.API;
using MartinGC94.DisplayConfig.Native.Enums;
using MartinGC94.DisplayConfig.Native.Structs;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsLifecycle.Enable, "Display")]
    public sealed class EnableDisplayCommand : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        [ArgumentCompleter(typeof(DisplayIdCompleter))]
        public uint[] DisplayId { get; set; }

        [Parameter()]
        public SwitchParameter AsClone { get; set; }

        private HashSet<int> displaysToEnable = new HashSet<int>();

        protected override void EndProcessing()
        {
            var config = API.DisplayConfig.GetConfig();
            foreach (uint id in DisplayId)
            {
                try
                {
                    _ = displaysToEnable.Add(config.GetDisplayIndex(id));
                }
                catch (ArgumentException error)
                {
                    WriteError(new ErrorRecord(error, "InvalidDisplayId", ErrorCategory.InvalidArgument, id));
                }
            }

            // "Enabling" a display adds inactive displays to the desktop or takes cloned displays out of clone mode.
            // This is done by invalidating all the mode indexes and setting a clone group for each display that should be active.
            // Displays with the same desktop position are considered cloned.
            // We keep track of the existing display clones so we don't accidentally stop cloning a display while enabling another.
            // If the AsClone switch is set, we add all the specified displays to Clone group 0.
            // This puts them in one clone group regardless if they are disabled or already in a different clone group.
            var cloneGroupTable = new Dictionary<POINTL, uint>();
            uint cloneGroup = AsClone ? (uint)1 : 0;
            foreach (int index in config.AvailablePathIndexes)
            {
                config.PathArray[index].targetInfo.modeInfoIdx = API.DisplayConfig.DISPLAYCONFIG_PATH_MODE_IDX_INVALID;

                if (config.PathArray[index].flags.HasFlag(PathInfoFlags.DISPLAYCONFIG_PATH_ACTIVE))
                {
                    if (displaysToEnable.Contains(index))
                    {
                        uint cloneGroupToSet = AsClone ? 0 : cloneGroup++;
                        config.PathArray[index].sourceInfo.ResetModeAndSetCloneGroup(cloneGroupToSet);
                    }
                    else
                    {
                        var desktopPosition = config.ModeArray[config.PathArray[index].sourceInfo.SourceModeInfoIdx].modeInfo.sourceMode.position;
                        if (cloneGroupTable.TryGetValue(desktopPosition, out uint savedCloneGroup))
                        {
                            config.PathArray[index].sourceInfo.ResetModeAndSetCloneGroup(savedCloneGroup);
                        }
                        else
                        {
                            cloneGroupTable.Add(desktopPosition, cloneGroup);
                            config.PathArray[index].sourceInfo.ResetModeAndSetCloneGroup(cloneGroup);
                            cloneGroup++;
                        }
                    }
                }
                else if (displaysToEnable.Contains(index))
                {
                    uint cloneGroupToSet = AsClone ? 0 : cloneGroup++;
                    config.PathArray[index].sourceInfo.CloneGroupId = cloneGroupToSet;
                    config.PathArray[index].flags |= PathInfoFlags.DISPLAYCONFIG_PATH_ACTIVE;
                }
            }

            config.ModeArray = null;
            var flags = SetDisplayConfigFlags.SDC_TOPOLOGY_SUPPLIED |
                SetDisplayConfigFlags.SDC_APPLY |
                SetDisplayConfigFlags.SDC_ALLOW_PATH_ORDER_CHANGES |
                SetDisplayConfigFlags.SDC_VIRTUAL_MODE_AWARE;
            try
            {
                config.ApplyConfig(flags);
            }
            catch (Win32Exception error)
            {
                if (error.NativeErrorCode == 31 && AsClone)
                {
                    // CCD sometimes fails to enable cloning across multiple adapters when there's no previously saved config.
                    // This alternative approach to enable cloning works in such scenarios.
                    try
                    {
                        RetryCloneWithExistingConfig();
                    }
                    catch (Win32Exception error2)
                    {
                        ThrowTerminatingError(new ErrorRecord(error2, "FailedToApplyAlternativeConfig", Utils.GetErrorCategory(error2), null));
                    }
                }
                else
                {
                    ThrowTerminatingError(new ErrorRecord(error, "FailedToApplyConfig", Utils.GetErrorCategory(error), null));
                }
                
            }
        }

        private void RetryCloneWithExistingConfig()
        {
            var config = API.DisplayConfig.GetConfig();
            foreach (int pathIndex in displaysToEnable)
            {
                config.PathArray[pathIndex].targetInfo.modeInfoIdx = API.DisplayConfig.DISPLAYCONFIG_PATH_MODE_IDX_INVALID;
                config.PathArray[pathIndex].sourceInfo.ResetModeAndSetCloneGroup(0);
                config.PathArray[pathIndex].flags |= PathInfoFlags.DISPLAYCONFIG_PATH_ACTIVE;
            }

            config.ApplyConfig(
                SetDisplayConfigFlags.SDC_USE_SUPPLIED_DISPLAY_CONFIG |
                SetDisplayConfigFlags.SDC_APPLY |
                SetDisplayConfigFlags.SDC_SAVE_TO_DATABASE |
                SetDisplayConfigFlags.SDC_VIRTUAL_MODE_AWARE);
        }
    }
}