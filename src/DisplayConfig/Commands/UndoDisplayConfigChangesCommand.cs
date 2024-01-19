using MartinGC94.DisplayConfig.Native;
using System.Management.Automation;
using MartinGC94.DisplayConfig.Native.Enums;
using System.ComponentModel;
using MartinGC94.DisplayConfig.API;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsCommon.Undo, "DisplayConfigChanges")]
    public class UndoDisplayConfigChangesCommand : Cmdlet
    {
        protected override void EndProcessing()
        {
            ReturnCode res = NativeMethods.SetDisplayConfig(0, null, 0, null, SetDisplayConfigFlags.SDC_APPLY | SetDisplayConfigFlags.SDC_USE_DATABASE_CURRENT);
            if (res != ReturnCode.ERROR_SUCCESS)
            {
                var error = new Win32Exception((int)res);
                ThrowTerminatingError(new ErrorRecord(error, "UndoFailure", Utils.GetErrorCategory(error), null));
            }
        }
    }
}