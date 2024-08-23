using MartinGC94.DisplayConfig.Native.Enums;
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
                config = DisplayConfig.GetConfig(DisplayConfigFlags.QDC_ALL_PATHS);
            }
            catch
            {
                yield break;
            }

            var inputPattern = WildcardPattern.Get(wordToComplete.Trim('\'', '"') + "*", WildcardOptions.None);
            foreach (int index in config.AvailablePathIndexes)
            {
                string displayIdAsString;
                string toolTip;
                try
                {
                    uint displayId = config.GetDisplayId(index);
                    displayIdAsString = displayId.ToString();
                    if (!inputPattern.IsMatch(displayIdAsString))
                    {
                        continue;
                    }

                    toolTip = config.GetDeviceNameInfo(index).monitorFriendlyDeviceName;
                    if (string.IsNullOrEmpty(toolTip))
                    {
                        toolTip = "Unknown Display";
                    }
                }
                catch
                {
                    continue;
                }

                yield return new CompletionResult(displayIdAsString, displayIdAsString, CompletionResultType.ParameterValue, toolTip);
            }
        }
    }
}