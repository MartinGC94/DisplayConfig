﻿using System.ComponentModel;
using System.Management.Automation;
using MartinGC94.DisplayConfig.API;
using MartinGC94.DisplayConfig.Native.Enums;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsCommon.Get, "DisplayConfig")]
    [OutputType(typeof(API.DisplayConfig))]
    public sealed class GetDisplayConfigCommand : Cmdlet
    {
        [Parameter(Position = 0)]
        public DisplayConfigFlags Flags { get; set; } = DisplayConfigFlags.QDC_ALL_PATHS;

        protected override void EndProcessing()
        {
            try
            {
                WriteObject(API.DisplayConfig.GetConfig(Flags));
            }
            catch (Win32Exception error)
            {
                ThrowTerminatingError(new ErrorRecord(error, "GetDisplayConfigError", Utils.GetErrorCategory(error), null));
            }
        }
    }
}