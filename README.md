# DisplayConfig
This PowerShell module allows you to manage various display settings like: Resolution, scaling, desktop positions and more.
It's built on top of the CCD (Connecting and Configuring Display) APIs. For more details, see: https://learn.microsoft.com/en-us/windows-hardware/drivers/display/ccd-apis

# Getting started
Install the module from the PowerShell gallery: `Install-Module DisplayConfig`  
Then check the available commands in the module: `Get-Command -Module DisplayConfig`  
There are 2 ways to use the commands in this module. First there's the simple way where you just call each individual command and the settings are immediately applied when each command finishes. For example:
```
Set-DisplayResolution -DisplayId 1 -Width 2560 -Height 1440
Set-DisplayRefreshRate -DisplayId 1 -RefreshRate 165
```
The other way is to generate a display configuration with all the desired settings, which can then be used to apply all the specified settings at once. For example:
```
Get-DisplayConfig |
    Set-DisplayResolution -DisplayId 1 -Width 2560 -Height 1440 |
    Set-DisplayRefreshRate -DisplayId 1 -RefreshRate 165 |
    Use-DisplayConfig
```
The benefit of the second approach is that it reduces the amount of time it takes to change all the settings and it reduces the amount of flickering that would normally occur whenever a display setting is changed.  
Another benefit is that a configuration can be backed up to a file and later restored:
```
Get-DisplayConfig | Export-Clixml $home\TVGamingProfile.xml
# Note that you need to import the module in the new session before importing the XML otherwise PowerShell will fail to convert the object correctly.
Import-Module DisplayConfig
Import-Clixml $home\TVGamingProfile.xml | Use-DisplayConfig -UpdateAdapterIds
```
Due to API limitations, not all commands support the display config approach. You can view the list of commands that do by running: `Get-Command -Module DisplayConfig -ParameterName DisplayConfig`.
