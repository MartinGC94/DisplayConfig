---
document type: cmdlet
external help file: DisplayConfig.dll-Help.xml
HelpUri: ''
Locale: da-DK
Module Name: DisplayConfig
ms.date: 09-23-2025
PlatyPS schema version: 2024-05-01
title: Disable-Display
---

# Disable-Display

## SYNOPSIS

Removes a display from the active desktop.

## SYNTAX

### ApplyNow (Default)

```
Disable-Display [-DisplayId] <uint[]> [<CommonParameters>]
```

### Config

```
Disable-Display [-DisplayId] <uint[]> -DisplayConfig <DisplayConfig> [<CommonParameters>]
```

## ALIASES

This cmdlet has the following aliases,
  None

## DESCRIPTION

Removes a display from the active desktop.

## EXAMPLES

### Example 1

PS C:\> Disable-Display -DisplayId 3

Disables the third display.

## PARAMETERS

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

The display(s) that should be disabled.
DisplayIds in this module use a similar logic as the Windows Settings app to number the displays, but there's no guarantee that it will match on every system.
Displays are sorted by the output port on the adapter with the following priority: Internal displays (laptops), PC connectors (DVI, Displayport), HDMI and others.
When multiple displays use the same connector (eg. 2 DisplayPort monitors) Windows will assign an incrementing number for each instance, and this number is combined with the priority to determine the exact display order.
The only way to change the displayId of a display is to move it to a different port on the graphics adapter.

```yaml
Type: System.UInt32[]
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: ApplyNow
  Position: 0
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
- Name: Config
  Position: 0
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


