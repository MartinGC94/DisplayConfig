using MartinGC94.DisplayConfig.Native;
using MartinGC94.DisplayConfig.Native.Enums;
using MartinGC94.DisplayConfig.Native.Structs;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.API
{
    internal class DisplayModeCompleter : IArgumentCompleter
    {
        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters)
        {
            var inputPattern = WildcardPattern.Get(wordToComplete.Trim('\'', '"') + "*", WildcardOptions.None);
            string deviceName;
            if (fakeBoundParameters.Contains("DisplayId") &&
                LanguagePrimitives.TryConvertTo(fakeBoundParameters["DisplayId"], out uint[] inputDisplays) &&
                inputDisplays.Length > 0)
            {
                try
                {
                    var config = DisplayConfig.GetConfig(DisplayConfigFlags.QDC_ALL_PATHS);
                    var displayIndex = config.GetDisplayIndex(inputDisplays[0]);
                    deviceName = DisplaySourceInfo.GetGdiDeviceName(config.PathArray[displayIndex].sourceInfo.adapterId, config.PathArray[displayIndex].sourceInfo.id);
                }
                catch
                {
                    deviceName = @"\\.\DISPLAY1";
                }
            }
            else
            {
                deviceName = @"\\.\DISPLAY1";
            }

            var completedValues = new HashSet<uint>();
            uint mode = 0;
            while (true)
            {
                var modeInfo = new DEVMODEW();
                modeInfo.dmSize = (ushort)Marshal.SizeOf(modeInfo);
                int exitCode = NativeMethods.EnumDisplaySettingsExW(deviceName, mode++, ref modeInfo, EnumDisplaySettingsFlags.None);
                if (exitCode == 0)
                {
                    yield break;
                }

                uint valueToAdd;
                switch (parameterName)
                {
                    case "Width":
                        valueToAdd = modeInfo.dmPelsWidth;
                        break;

                    case "Height":
                        valueToAdd = modeInfo.dmPelsHeight;
                        break;

                    case "RefreshRate":
                        valueToAdd = modeInfo.dmDisplayFrequency;
                        break;

                    default:
                        yield break;
                }

                if (completedValues.Add(valueToAdd))
                {
                    string valueAsString = valueToAdd.ToString();
                    if (inputPattern.IsMatch(valueAsString))
                    {
                        yield return new CompletionResult(valueAsString, valueAsString, CompletionResultType.ParameterValue, valueAsString);
                    }
                }
            }
        }
    }
}