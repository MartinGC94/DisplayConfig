using MartinGC94.DisplayConfig.API;
using MartinGC94.DisplayConfig.Native.Enums;
using System.Management.Automation;
using MartinGC94.DisplayConfig.Native;
using System.ComponentModel;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsCommon.Set, "DisplayProfile")]
    public sealed class SetDisplayProfileCommand : Cmdlet
    {
        #region Parameters
        [Parameter(Mandatory = true, Position = 0)]
        public TopologyProfile Profile { get; set; }
        #endregion

        protected override void EndProcessing()
        {
            SetDisplayConfigFlags flags = SetDisplayConfigFlags.SDC_APPLY;
            switch (Profile)
            {
                case TopologyProfile.Internal:
                    flags |= SetDisplayConfigFlags.SDC_TOPOLOGY_INTERNAL;
                    break;

                case TopologyProfile.Clone:
                    flags |= SetDisplayConfigFlags.SDC_TOPOLOGY_CLONE;
                    break;

                case TopologyProfile.Extend:
                    flags |= SetDisplayConfigFlags.SDC_TOPOLOGY_EXTEND;
                    break;

                case TopologyProfile.External:
                    flags |= SetDisplayConfigFlags.SDC_TOPOLOGY_EXTERNAL;
                    break;

                default:
                    break;
            }

            ReturnCode result = NativeMethods.SetDisplayConfig(0, null, 0, null, flags);
            if (result != ReturnCode.ERROR_SUCCESS)
            {
                var error = new Win32Exception((int)result);
                ThrowTerminatingError(new ErrorRecord(error, "SetDisplayProfileError", Utils.GetErrorCategory(error), null));
            }
        }
    }
}