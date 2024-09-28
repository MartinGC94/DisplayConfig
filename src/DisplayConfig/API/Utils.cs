using System;
using System.ComponentModel;
using System.IO;
using System.Management.Automation;
using System.Security;

namespace MartinGC94.DisplayConfig.API
{
    internal static class Utils
    {
        internal const string AllowChangesRecommendation = "Run the command again with the -AllowChanges parameter.";
        internal const string UpdateAdapterIdsRecommendation = "Run the command again with the -UpdateAdapterIds parameter.";
        /// <summary>Gets a relevant error category based on the exception input.</summary>
        internal static ErrorCategory GetErrorCategory(Exception exception)
        {
            switch (exception)
            {
                case Win32Exception nativeError:
                    switch (nativeError.NativeErrorCode)
                    {
                        case 2:
                            return ErrorCategory.ObjectNotFound;

                        case 5:
                        case 1314:
                            return ErrorCategory.PermissionDenied;

                        case 6:
                        case 50:
                            return ErrorCategory.InvalidOperation;

                        case 87:
                        case 1610:
                            return ErrorCategory.InvalidArgument;

                        case -1071241844:
                            return ErrorCategory.ResourceUnavailable;

                        case -2147467259:
                        case 31:
                            return ErrorCategory.DeviceError;

                        default:
                            return ErrorCategory.NotSpecified;
                    }

                case ArgumentNullException _:
                case ArgumentException _:
                    return ErrorCategory.InvalidArgument;

                case IOException _:
                case ObjectDisposedException _:
                    return ErrorCategory.ResourceUnavailable;

                case InvalidOperationException _:
                    return ErrorCategory.InvalidOperation;

                case UnauthorizedAccessException _:
                case SecurityException _:
                    return ErrorCategory.PermissionDenied;

                case InvalidCastException _:
                    return ErrorCategory.InvalidType;

                default:
                    return ErrorCategory.NotSpecified;
            }
        }
    }
}