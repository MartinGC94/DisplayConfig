---
document type: cmdlet
external help file: DisplayConfig.dll-Help.xml
HelpUri: ''
Locale: da-DK
Module Name: DisplayConfig
ms.date: 09-23-2025
PlatyPS schema version: 2024-05-01
title: Enable-Display
---

# Enable-Display

## SYNOPSIS

Reenables a display, this is the same as picking "Extend desktop to this display" in the settings app.
If the display is already active, but is used for display cloning then this will stop the cloning.

## SYNTAX

### __AllParameterSets

```
Enable-Display [-DisplayId] <uint[]> [-DisplayIdToDisable <uint[]>] [-AsClone] [<CommonParameters>]
```

## ALIASES

This cmdlet has the following aliases,
  None

## DESCRIPTION

Reenables a display, this is the same as picking "Extend desktop to this display" in the settings app.
If the display is already active, but is used for display cloning then this will stop the cloning.
The -AsClone switch can be used to set up a new clone group with the specified displays.

## EXAMPLES

### Example 1

PS C:\> Enable-Display -DisplayId 3

Reenables the third display, adding it back to the desktop if it was previously turned off or used for cloning.

## PARAMETERS

### -AsClone

When specified, the specified displays are put into one clone group.
This allows you to turn on displays and add them to a clone group in one go.
You can use this over Copy-DisplaySource when you want to let Windows determine the best mode, rather than copying it from an existing display.

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

### -DisplayId

The display to enable.
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
- Name: (All)
  Position: 0
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -DisplayIdToDisable

The display to disable.
This can be used to enable and disable 2 or more displays in one go which is equivalent to the "Show only on X" option in the display settings app.
DisplayIds in this module use a similar logic as the Windows Settings app to number the displays, but there's no guarantee that it will match on every system.
Displays are sorted by the output port on the adapter with the following priority: Internal displays (laptops), PC connectors (DVI, Displayport), HDMI and others.
When multiple displays use the same connector (eg.
2 DisplayPort monitors) Windows will assign an incrementing number for each instance, and this number is combined with the priority to determine the exact display order.
The only way to change the displayId of a display is to move it to a different port on the graphics adapter.

```yaml
Type: System.UInt32[]
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

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable,
-InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable,
-ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see
[about_CommonParameters](https://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

## OUTPUTS


## NOTES




## RELATED LINKS


