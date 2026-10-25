---
document type: cmdlet
external help file: DisplayConfig.dll-Help.xml
HelpUri: ''
Locale: da-DK
Module Name: DisplayConfig
ms.date: 10-25-2026
PlatyPS schema version: 2024-05-01
title: Set-DisplayColor
---

# Set-DisplayColor

## SYNOPSIS

Configures various advanced color settings, such as toggling HDR and automatic color management, or setting the SDR white level.

## SYNTAX

### __AllParameterSets

```
Set-DisplayColor [-DisplayId] <uint[]> [-WideColorEnabled <bool>] [-AdvancedColorActive <bool>]
 [-HDREnabled <bool>] [-SDRWhiteLevel <uint>] [<CommonParameters>]
```

## ALIASES

This cmdlet has the following aliases,
  None


## DESCRIPTION

Configures various advanced color settings, such as toggling HDR and automatic color management, or setting the SDR white level.

## EXAMPLES

### Example 1

Set-DisplayColor -DisplayId 1 -HDREnabled $true -SDRWhiteLevel 600

This enables HDR on display 1, and sets the SDR white level to 600.

## PARAMETERS

### -AdvancedColorActive

Enables or disables Advanced colors in Windows (Umbrella term for HDR and Wide Color Gamut).
This is equivalent to the commands Enable-DisplayAdvancedColor and Disable-DisplayAdvancedColor.

```yaml
Type: System.Boolean
DefaultValue: ''
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: true
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -DisplayId

Specifies the display where the color settings should be modified.

```yaml
Type: System.UInt32[]
DefaultValue: ''
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: 0
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: true
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -HDREnabled

Specifies whether or not HDR is enabled on the display. This is equivalent to the command Set-DisplayHDR -EnableHDR
Specifies whether or not HDR is enabled on the display.
This is equivalent to the command Set-DisplayHDR -EnableHDR

```yaml
Type: System.Boolean
DefaultValue: ''
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: true
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -SDRWhiteLevel

Sets the SDR white level for the specified displays.
This is equivalent to the HDR/SDR slider in the settings app, and the command Set-DisplayHDR -SdrWhiteLevel.

```yaml
Type: System.UInt32
DefaultValue: ''
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: true
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -WideColorEnabled

Sets the Wide Color Gamut state for the display.
This is the same as toggling the "Automatically manage color for apps" switch in the display color profile settings.

```yaml
Type: System.Boolean
DefaultValue: ''
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: true
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

### System.UInt32[]

{{ Fill in the Description }}

### System.Boolean

{{ Fill in the Description }}

### System.UInt32

{{ Fill in the Description }}

## OUTPUTS

## NOTES

## RELATED LINKS

{{ Fill in the related links here }}

