---
document type: cmdlet
external help file: DisplayConfig.dll-Help.xml
HelpUri: ''
Locale: da-DK
Module Name: DisplayConfig
ms.date: 09-23-2025
PlatyPS schema version: 2024-05-01
title: Set-DisplayHDR
---

# Set-DisplayHDR

## SYNOPSIS

Manages global and display specific HDR settings.

## SYNTAX

### DisplaySpecific

```
Set-DisplayHDR [-DisplayId] <uint[]> [-SdrWhiteLevel <uint>] [-EnableHDR] [<CommonParameters>]
```

### Global

```
Set-DisplayHDR [-EnableHDRPlayback] [-EnableAutoHDR] [<CommonParameters>]
```

## ALIASES

This cmdlet has the following aliases,
  None

## DESCRIPTION

Manages global and display specific HDR settings.

## EXAMPLES

### Example 1

PS C:\> Set-DisplayHDR -DisplayId 3 -SdrWhiteLevel 3000 -EnableHDR

Enables HDR and sets the Sdr white level to 3000

## PARAMETERS

### -DisplayId

Specifies the displays where display specific settings should be applied.

```yaml
Type: System.UInt32[]
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: DisplaySpecific
  Position: 0
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -EnableAutoHDR

Enables/disables the auto HDR feature which tries to convert SDR content into HDR.
This feature was introduced in Windows 11.

```yaml
Type: System.Management.Automation.SwitchParameter
DefaultValue: False
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: Global
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -EnableHDR

Enables/disables HDR on the specified display.
This is the same as running `Enable-DisplayAdvancedColor` or `Disable-DisplayAdvancedColor`.

```yaml
Type: System.Management.Automation.SwitchParameter
DefaultValue: False
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: DisplaySpecific
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -EnableHDRPlayback

Enables/disables the "Stream HDR-videos" option globally.

```yaml
Type: System.Management.Automation.SwitchParameter
DefaultValue: False
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: Global
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -SdrWhiteLevel

Sets the SDR white level for the specified displays.
This is equivalent to the HDR/SDR slider in the settings app.

```yaml
Type: System.UInt32
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: DisplaySpecific
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

## OUTPUTS





## NOTES




## RELATED LINKS



