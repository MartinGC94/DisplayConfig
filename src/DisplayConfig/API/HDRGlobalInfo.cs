using System.Management.Automation;

namespace MartinGC94.DisplayConfig.API
{
    public sealed class HDRGlobalInfo
    {
        public bool HDRPlaybackEnabled { get; }
        public bool AutoHDREnabled { get; }

        private HDRGlobalInfo(bool playbackEnabled, bool autoHdrEnabled)
        {
            HDRPlaybackEnabled = playbackEnabled;
            AutoHDREnabled = autoHdrEnabled;
        }

        internal static HDRGlobalInfo GetGlobalSettings(CommandInvocationIntrinsics invokeCommand)
        {
            var result = invokeCommand.InvokeScript(@"$PlaybackEnabled = & {
    $Path = 'Registry::HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\VideoSettings'
    if (!(Test-Path -LiteralPath $Path))
    {
        return $false
    }

    $Value = Get-ItemPropertyValue -LiteralPath $Path -Name EnableHDRForPlayback -ErrorAction Ignore
    return $Value -eq 1
}
$AutoHDREnabled = & {
    $Path = 'Registry::HKEY_CURRENT_USER\SOFTWARE\Microsoft\DirectX\UserGpuPreferences'
    if (!(Test-Path -LiteralPath $Path))
    {
        return $false
    }

    $Value = Get-ItemPropertyValue -LiteralPath $Path -Name DirectXUserGlobalSettings -ErrorAction Ignore
    if ([string]::IsNullOrWhiteSpace($Value) -or $Value -notmatch ""AutoHDREnable=(\d)"")
    {
        return $false
    }

    return $Matches[1] -eq ""1""
}
$PlaybackEnabled
$AutoHDREnabled");

            bool playbackEnabled = LanguagePrimitives.ConvertTo<bool>(result[0]);
            bool autoHDREnabled = LanguagePrimitives.ConvertTo<bool>(result[1]);
            return new HDRGlobalInfo(playbackEnabled, autoHDREnabled);
        }
    }
}