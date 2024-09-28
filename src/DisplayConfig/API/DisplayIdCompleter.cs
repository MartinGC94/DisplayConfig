using MartinGC94.DisplayConfig.Native.Structs;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace MartinGC94.DisplayConfig.API
{
    internal sealed class DisplayIdCompleter : IArgumentCompleter
    {
        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters)
        {
            DisplayConfig config;
            try
            {
                config = DisplayConfig.GetConfig();
            }
            catch
            {
                yield break;
            }

            var inputPattern = WildcardPattern.Get(wordToComplete.Trim('\'', '"') + "*", WildcardOptions.None);
            foreach (int index in config.AvailablePathIndexes)
            {
                uint displayId = config.GetDisplayId(index);
                string displayIdAsString = displayId.ToString();
                if (!inputPattern.IsMatch(displayIdAsString))
                {
                    continue;
                }

                DISPLAYCONFIG_TARGET_DEVICE_NAME displayInfo = config.GetDeviceNameInfo(index);
                string displayName = string.IsNullOrEmpty(displayInfo.monitorFriendlyDeviceName) ? "Unknown Display" : displayInfo.monitorFriendlyDeviceName;
                string listItemText = $"{displayIdAsString} {displayName} ({(ConnectionType)displayInfo.outputTechnology})";
                string toolTip = $"{displayName} ({(ConnectionType)displayInfo.outputTechnology})";

                yield return new CompletionResult(displayIdAsString, listItemText, CompletionResultType.ParameterValue, toolTip);
            }
        }
    }
}