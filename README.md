# DisplayConfig
This PowerShell module allows you to manage various display settings like: Resolution, scaling, desktop positions and more.
It's built on top of the CCD (Connecting and Configuring Display) APIs, which can be read about here: https://learn.microsoft.com/en-us/windows-hardware/drivers/display/ccd-apis

# Getting started
First, install the module from the PowerShell gallery: `Install-Module DisplayConfig`  
Then check the available commands in the module: `Get-Command -Module DisplayConfig`  
There are 2 ways to use the commands in this module, first there's the simple approach where you just call each individual command, and the settings are immediately applied when each command finishes for example:
```
Set-DisplayResolution -DisplayId 1 -Width 2560 -Height 1440
Set-DisplayRefreshRate -DisplayId 1 -RefreshRate 165
```
The second approach is to generate a display configuration with all the desired settings, which then applies all the specified settings at once, for example:
```
Get-DisplayConfig |
    Set-DisplayResolution -DisplayId 1 -Width 2560 -Height 1440 |
    Set-DisplayRefreshRate -DisplayId 1 -RefreshRate 165 |
    Use-DisplayConfig
```
The benefit of the second approach is that it reduces the amount of time it takes to change all the settings, and reduces the amount of flickering that would normally occur whenever a display setting is changed.  
Another benefit is that a configuration can be backed up to a file, and later restored:
```
Get-DisplayConfig | Export-Clixml $home\TVGamingProfile.xml
Import-Clixml $home\TVGamingProfile.xml | Use-DisplayConfig -UpdateAdapterIds
```
Due to API limitations, not all commands support the display config approach, you can view the list of commands that do by running: `Get-Command -Module DisplayConfig -ParameterName DisplayConfig`.
