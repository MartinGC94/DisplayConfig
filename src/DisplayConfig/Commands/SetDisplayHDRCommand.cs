using MartinGC94.DisplayConfig.API;
using MartinGC94.DisplayConfig.Native.Enums;
using MartinGC94.DisplayConfig.Native.Structs;
using System;
using System.ComponentModel;
using System.Management.Automation;

namespace MartinGC94.DisplayConfig.Commands
{
    [Cmdlet(VerbsCommon.Set, "DisplayHDR")]
    public sealed class SetDisplayHDRCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "DisplaySpecific")]
        [ArgumentCompleter(typeof(DisplayIdCompleter))]
        public uint[] DisplayId { get; set; }

        [Parameter(ParameterSetName = "DisplaySpecific")]
        public uint SdrWhiteLevel { get; set; }

        [Parameter(ParameterSetName = "DisplaySpecific")]
        public SwitchParameter EnableHDR { get; set; }

        [Parameter(ParameterSetName = "Global")]
        public SwitchParameter EnableHDRPlayback { get; set; }

        [Parameter(ParameterSetName = "Global")]
        public SwitchParameter EnableAutoHDR { get; set; }

        protected override void EndProcessing()
        {
            if (ParameterSetName == "Global")
            {
                if (MyInvocation.BoundParameters.ContainsKey("EnableHDRPlayback"))
                {
                    string scriptToRun = string.Format(@"$Path = 'Registry::HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\VideoSettings'
if (!(Test-Path -LiteralPath $Path))
{{
    $null = New-Item -Path $Path -Force -ErrorAction Stop
}}
New-ItemProperty -LiteralPath $Path -Name EnableHDRForPlayback -PropertyType DWord -Value {0} -Force", EnableHDRPlayback == true ? 1 : 0);
                    _ = InvokeCommand.InvokeScript(scriptToRun);
                }

                if (MyInvocation.BoundParameters.ContainsKey("EnableAutoHDR"))
                {
                    string scriptToRun = string.Format(@"$Path = 'Registry::HKEY_CURRENT_USER\SOFTWARE\Microsoft\DirectX\UserGpuPreferences'
if (!(Test-Path -LiteralPath $Path))
{{
    $null = New-Item -Path $Path -Force -ErrorAction Stop
}}

$PropertyParams = @{{
    LiteralPath = $Path
    Name        = 'DirectXUserGlobalSettings'
}}
$OldValue = Get-ItemPropertyValue @PropertyParams -ErrorAction Ignore

$NewValue = if ([string]::IsNullOrWhiteSpace($OldValue))
{{
    'AutoHDREnable={0};'
}}
elseif ($OldValue -match 'AutoHDREnable=\d;')
{{
    $OldValue -replace 'AutoHDREnable=\d;', 'AutoHDREnable={0};'
}}
else
{{
    ""AutoHDREnable={0};$OldValue""
}}

New-ItemProperty @PropertyParams -PropertyType String -Value $NewValue -Force", EnableAutoHDR == true ? 1 : 0);
                    _ = InvokeCommand.InvokeScript(scriptToRun);
                }

                return;
            }

            if (MyInvocation.BoundParameters.ContainsKey("EnableHDR"))
            {
                ColorInfo.ToggleAdvancedColor(this, DisplayId, EnableHDR);
            }

            if (MyInvocation.BoundParameters.ContainsKey("SdrWhiteLevel"))
            {
                var config = API.DisplayConfig.GetConfig(this);
                foreach (uint id in DisplayId)
                {
                    int index;
                    try
                    {
                        index = config.GetDisplayIndex(id);
                    }
                    catch (ArgumentException error)
                    {
                        WriteError(Utils.GetInvalidDisplayIdError(error, id));
                        continue;
                    }

                    try
                    {
                        config.ValidatePathIsActive(index);
                    }
                    catch (Exception error) when (!(error is PipelineStoppedException))
                    {
                        WriteError(new ErrorRecord(error, "InactiveDisplay", ErrorCategory.InvalidArgument, id));
                        continue;
                    }

                    LUID adapterId = config.PathArray[index].targetInfo.adapterId;
                    uint targetId = config.PathArray[index].targetInfo.id;

                    try
                    {
                        ColorInfo.SetSdrWhiteLevel(adapterId, targetId, SdrWhiteLevel);
                    }
                    catch (Win32Exception error)
                    {
                        WriteError(new ErrorRecord(error, "SetSdrWhiteLevelError", Utils.GetErrorCategory(error), id));
                    }
                }
            }
        }
    }
}