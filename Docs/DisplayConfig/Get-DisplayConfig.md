---
document type: cmdlet
external help file: DisplayConfig.dll-Help.xml
HelpUri: ''
Locale: da-DK
Module Name: DisplayConfig
ms.date: 09-23-2025
PlatyPS schema version: 2024-05-01
title: Get-DisplayConfig
---

# Get-DisplayConfig

## SYNOPSIS

Gets the current display topology settings, which can then be exported or modified and later applied.

## SYNTAX

### __AllParameterSets

```
Get-DisplayConfig [[-Flags] <DisplayConfigFlags>] [<CommonParameters>]
```

## ALIASES

This cmdlet has the following aliases,
  None

## DESCRIPTION

Gets the current display topology settings, which can then be exported or modified and later applied.
This can be combined with Export/Import-Clixml to save various topology profiles that can later be applied.
Some commands in this module also supports modifying the topology settings so multiple settings can be applied at once, minimizing the amount of flickering that occurs whenever a setting is changed.
The compatible commands can be found like this: `Get-Command -Module DisplayConfig -ParameterName DisplayConfig`
Advanced users can also manually modify the path/mode settings before applying them.

## EXAMPLES

### Example 1

PS C:\> Get-DisplayConfig | Export-Clixml -Path $HOME\TvProfile.xml

Exports the current settings so they can later be used like this: `Import-Clixml $HOME\TvProfile.xml | Use-DisplayConfig -UpdateAdapterIds`

### Example 2

PS C:\> Get-DisplayConfig | Disable-Display -DisplayId 3 | Set-DisplayRotation -DisplayId 1 -Rotation Rotate90 | Set-DisplayPrimary -DisplayId 2 | Use-DisplayConfig

Changes various settings in the config, before applying them all at once.
This reduces the flickering and the time it takes to change multiple settings.

## PARAMETERS

### -Flags

Only for advanced users!
By default, all paths are enumerated which can be a bit slow.
To make things faster you can request info for only the active paths, or the current CCD database.
Beware that if a display is disabled it may affect the displayIds assigned to each display, since the code no longer knows about all connected displays.
More information can be found at: https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-querydisplayconfig#parameters

```yaml
Type: MartinGC94.DisplayConfig.Native.Enums.DisplayConfigFlags
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: 0
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable,
-InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable,
-ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see
[about_CommonParameters](https://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

## OUTPUTS

### MartinGC94.DisplayConfig.API.DisplayConfig



## NOTES




## RELATED LINKS



