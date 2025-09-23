---
document type: cmdlet
external help file: DisplayConfig.dll-Help.xml
HelpUri: ''
Locale: da-DK
Module Name: DisplayConfig
ms.date: 09-23-2025
PlatyPS schema version: 2024-05-01
title: Use-DisplayConfig
---

# Use-DisplayConfig

## SYNOPSIS

Applies the specified Display topology settings.

## SYNTAX

### __AllParameterSets

```
Use-DisplayConfig -DisplayConfig <DisplayConfig> [-AllowChanges] [-DontSave]
 [-Flags <SetDisplayConfigFlags>] [-UpdateAdapterIds] [<CommonParameters>]
```

## ALIASES

This cmdlet has the following aliases,
  None

## DESCRIPTION

Applies the specified Display topology settings.

## EXAMPLES

### Example 1

PS C:\> Import-Clixml $HOME\TvProfile.xml | Use-DisplayConfig -UpdateAdapterIds

Imports a profile that was stored on the filesystem and applies it to the current configuration.

## PARAMETERS

### -AllowChanges

Allow Windows to make slight adjustments to the configuration (readjusting desktop positions, changing the refresh rate, etc.).
If this is not set, and the configuration contains invalid data then an error will be thrown.

```yaml
Type: System.Management.Automation.SwitchParameter
DefaultValue: False
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -DisplayConfig

The DisplayConfig to apply.

```yaml
Type: MartinGC94.DisplayConfig.API.DisplayConfig
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: Named
  IsRequired: true
  ValueFromPipeline: true
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -DontSave

Skips saving this configuration change to the configuration database, allowing you to roll back the changes with: `Undo-DisplayConfigChanges`.

```yaml
Type: System.Management.Automation.SwitchParameter
DefaultValue: False
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -Flags

Only for advanced users!
You can read what each flag does, and how they interact here: https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setdisplayconfig#parameters

```yaml
Type: MartinGC94.DisplayConfig.Native.Enums.SetDisplayConfigFlags
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -UpdateAdapterIds

Gets the current display adapter IDs and updates the display config to use those before applying the config.
This is useful if the displayconfig has been imported from a file because Windows may have changed the IDs since the file was exported.

```yaml
Type: System.Management.Automation.SwitchParameter
DefaultValue: False
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: Named
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

### MartinGC94.DisplayConfig.API.DisplayConfig



## OUTPUTS





## NOTES




## RELATED LINKS



