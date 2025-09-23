---
document type: cmdlet
external help file: DisplayConfig.dll-Help.xml
HelpUri: ''
Locale: da-DK
Module Name: DisplayConfig
ms.date: 09-23-2025
PlatyPS schema version: 2024-05-01
title: Set-DisplayResolution
---

# Set-DisplayResolution

## SYNOPSIS

Sets the resolution of the specified displays.

## SYNTAX

### ApplyNow (Default)

```
Set-DisplayResolution [-DisplayId] <uint[]> [-Width] <uint> [-Height] <uint> [-DontSave]
 [-AllowChanges] [<CommonParameters>]
```

### Config

```
Set-DisplayResolution [-DisplayId] <uint[]> [-Width] <uint> [-Height] <uint>
 -DisplayConfig <DisplayConfig> [-ChangeAspectRatio] [<CommonParameters>]
```

## ALIASES

This cmdlet has the following aliases,
  None

## DESCRIPTION

Sets the resolution of the specified displays.

## EXAMPLES

### Example 1

PS C:\> Set-DisplayResolution -DisplayId 3 -Width 1600 -Height 900

Changes the resolution of display 3 to 1600 x 900.

## PARAMETERS

### -AllowChanges

Allows Windows to slightly adjust the mode (resolution and refresh rate) this can be necessary if certain refresh rates are only available at certain resolutions.

```yaml
Type: System.Management.Automation.SwitchParameter
DefaultValue: False
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: ApplyNow
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -ChangeAspectRatio

Fixes an issue where the aspect ratio is not changed when changing to a resolution with a different aspect ratio.
Due to API limitations, no further changes can be made to a DisplayConfig after this parameter has been used, so this has to be used as a last step before applying it.

```yaml
Type: System.Management.Automation.SwitchParameter
DefaultValue: False
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: Config
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

The displayConfig where this setting should be applied.
See the help for `Get-DisplayConfig` for more info.

```yaml
Type: MartinGC94.DisplayConfig.API.DisplayConfig
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: Config
  Position: Named
  IsRequired: true
  ValueFromPipeline: true
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -DisplayId

The display(s) where the resolution should be changed.

```yaml
Type: System.UInt32[]
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: Config
  Position: 0
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
- Name: ApplyNow
  Position: 0
  IsRequired: true
  ValueFromPipeline: false
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
- Name: ApplyNow
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -Height

Specifies the height of the resolution, meaning the second number in the common "1920 x 1080" format.

```yaml
Type: System.UInt32
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: Config
  Position: 2
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
- Name: ApplyNow
  Position: 2
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -Width

Specifies the width of the resolution, meaning the first number in the common "1920 x 1080" format.

```yaml
Type: System.UInt32
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: Config
  Position: 1
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
- Name: ApplyNow
  Position: 1
  IsRequired: true
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

### MartinGC94.DisplayConfig.API.DisplayConfig



## NOTES




## RELATED LINKS



