using MartinGC94.DisplayConfig.Native;
using MartinGC94.DisplayConfig.Native.Enums;
using MartinGC94.DisplayConfig.Native.Structs;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Runtime.InteropServices;

namespace MartinGC94.DisplayConfig.API
{
    internal class DisplayModeCompleter : IArgumentCompleter
    {
        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters)
        {
            string cleanInput = wordToComplete is null
                ? "*"
                : wordToComplete.Trim('\'', '"') + "*";
            WildcardPattern inputPattern = WildcardPattern.Get(cleanInput, WildcardOptions.None);

            DisplayConfig displayConfig = DisplayConfig.GetConfig();
            int displayIndex;
            string deviceName;
            if (fakeBoundParameters.Contains("DisplayId") &&
                LanguagePrimitives.TryConvertTo(fakeBoundParameters["DisplayId"], out uint[] inputDisplays) &&
                inputDisplays.Length > 0)
            {
                try
                {
                    displayIndex = displayConfig.GetDisplayIndex(inputDisplays[0]);
                    deviceName = DisplaySourceInfo.GetGdiDeviceName(displayConfig.PathArray[displayIndex].sourceInfo.adapterId, displayConfig.PathArray[displayIndex].sourceInfo.id);
                }
                catch
                {
                    deviceName = @"\\.\DISPLAY1";
                    displayIndex = -1;
                }
            }
            else
            {
                deviceName = @"\\.\DISPLAY1";
                displayIndex = -1;
            }

            if (!(parameterName.Equals("Width") &&
                fakeBoundParameters.Contains("Height") &&
                LanguagePrimitives.TryConvertTo(fakeBoundParameters["Height"], out uint inputHeight)))
            {
                inputHeight = 0;
            }

            if (!(parameterName.Equals("Height") &&
                fakeBoundParameters.Contains("Width") &&
                LanguagePrimitives.TryConvertTo(fakeBoundParameters["Width"], out uint inputWidth)))
            {
                inputWidth = 0;
            }

            var completedValues = new HashSet<uint>();

            if (displayIndex != -1)
            {
                ModeInfo modeData;
                try
                {
                    modeData = displayConfig.GetPreferredMode(displayConfig.GetDisplayId(displayIndex));
                }
                catch
                {
                    modeData = null;
                }

                if (modeData != null)
                {
                    string valueToAdd;
                    switch (parameterName)
                    {
                        case "Width":
                            if (inputHeight != 0 && inputHeight != modeData.Height)
                            {
                                valueToAdd = null;
                                break;
                            }
                            _ = completedValues.Add(modeData.Width);
                            valueToAdd = modeData.Width.ToString();
                            break;

                        case "Height":
                            if (inputWidth != 0 && inputWidth != modeData.Width)
                            {
                                valueToAdd = null;
                                break;
                            }
                            _ = completedValues.Add(modeData.Height);
                            valueToAdd = modeData.Height.ToString();
                            break;

                        case "RefreshRate":
                            valueToAdd = modeData.RefreshRate.ToString(CultureInfo.InvariantCulture);
                            if (modeData.RefreshRate % 1 == 0)
                            {
                                _ = completedValues.Add((uint)modeData.RefreshRate);
                            }
                            break;

                        default:
                            valueToAdd = null;
                            break;
                    }

                    if (valueToAdd != null && inputPattern.IsMatch(valueToAdd))
                    {
                        yield return new CompletionResult(valueToAdd, valueToAdd + " (Recommended)", CompletionResultType.ParameterValue, valueToAdd);
                    }
                }
            }

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
                        if (inputHeight != 0 && inputHeight != modeInfo.dmPelsHeight)
                        {
                            continue;
                        }
                        valueToAdd = modeInfo.dmPelsWidth;
                        break;

                    case "Height":
                        if (inputWidth != 0 && inputWidth != modeInfo.dmPelsWidth)
                        {
                            continue;
                        }
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