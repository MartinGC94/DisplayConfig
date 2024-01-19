using System;
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

        protected override void EndProcessing()
        {
            var config = API.DisplayConfig.GetConfig(DisplayConfigFlags.QDC_ALL_PATHS);
            var displaysToEnable = new HashSet<int>();
            foreach (uint id in DisplayId)
            {
                try
                {
                    _ = displaysToEnable.Add(config.GetDisplayIndex(id));
                }
                catch (ArgumentOutOfRangeException error)
                {
                    WriteError(new ErrorRecord(error, "InvalidDisplayId", ErrorCategory.InvalidArgument, id));
                }
            }

            uint nextSourceId = 0;
            foreach (int index in config.AvailablePathIndexes)
            {
                if (config.PathArray[index].sourceInfo.id > nextSourceId)
                {
                    nextSourceId = config.PathArray[index].sourceInfo.id;
                }
            }

            nextSourceId++;
            var newPathArray = new DISPLAYCONFIG_PATH_INFO[config.AvailablePathIndexes.Length];
            uint i = 0;
            foreach (int index in config.AvailablePathIndexes)
            {
                newPathArray[i] = config.PathArray[index];
                newPathArray[i].sourceInfo.modeInfoIdx = API.DisplayConfig.DISPLAYCONFIG_PATH_MODE_IDX_INVALID;
                newPathArray[i].targetInfo.modeInfoIdx = API.DisplayConfig.DISPLAYCONFIG_PATH_MODE_IDX_INVALID;
                /// On cloned displays, <see cref="DISPLAYCONFIG_PATH_TARGET_INFO.id"/> the last 8 bits are used for something (not documented) but they need to be cleared.
                newPathArray[i].targetInfo.id = (newPathArray[i].targetInfo.id << 8) >> 8;
                if (displaysToEnable.Contains(index))
                {
                    newPathArray[i].flags |= PathInfoFlags.DISPLAYCONFIG_PATH_ACTIVE;
                    newPathArray[i].sourceInfo.id = nextSourceId;
                    nextSourceId++;
                }

                i++;
            }

            config.PathArray = newPathArray;
            config.ModeArray = null;
            var flags = SetDisplayConfigFlags.SDC_APPLY | SetDisplayConfigFlags.SDC_TOPOLOGY_SUPPLIED | SetDisplayConfigFlags.SDC_ALLOW_PATH_ORDER_CHANGES;
            try
            {
                config.ApplyConfig(flags);
            }
            catch (Win32Exception error)
            {
                WriteError(new ErrorRecord(error, "FailedToApplyConfig", Utils.GetErrorCategory(error), null));
            }
        }
    }
}