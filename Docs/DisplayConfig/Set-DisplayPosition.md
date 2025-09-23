---
document type: cmdlet
external help file: DisplayConfig.dll-Help.xml
HelpUri: ''
Locale: da-DK
Module Name: DisplayConfig
ms.date: 09-23-2025
PlatyPS schema version: 2024-05-01
title: Set-DisplayPosition
---

# Set-DisplayPosition

## SYNOPSIS

Changes the position of one or more displays.

## SYNTAX

### Position (Default)

```
Set-DisplayPosition [-DisplayId] <uint> [-XPosition <int>] [-YPosition <int>] [-AsOffset]
 [-DontSave] [-AllowChanges] [-DisplayConfig <DisplayConfig>] [<CommonParameters>]
```

### OffsetFromDisplay

```
Set-DisplayPosition [-DisplayId] <uint> -RelativeDisplayId <uint> -Position <RelativePosition>
 [-DontSave] [-AllowChanges] [-DisplayConfig <DisplayConfig>] [<CommonParameters>]
```

### LeftToRight

```
Set-DisplayPosition -LeftToRightDisplayIds <uint[]> [-DontSave] [-AllowChanges]
 [-DisplayConfig <DisplayConfig>] [<CommonParameters>]
```

### SwapDisplays

```
Set-DisplayPosition -SwapDisplay <uint[]> [-DontSave] [-AllowChanges]
 [-DisplayConfig <DisplayConfig>] [<CommonParameters>]
```

## ALIASES

This cmdlet has the following aliases,
  None

## DESCRIPTION

Changes the position of one or more displays.
This does not move displays that are already in the specified position so it may be necessary to combine this command with `Get-DisplayConfig` and run this multiple times to move the displays one by one.

## EXAMPLES

### Example 1

PS C:\> Set-DisplayPosition -LeftToRightDisplayIds 2,1,3

Positions displays 1,2,3 side by side with 2 being all the way to the left, 1 being in the middle and 3 being all the way to the right.

### Example 2

PS C:\> Get-DisplayConfig | Set-DisplayPosition -DisplayId 1 -Position Above -RelativeDisplayId 2 | Set-DisplayPosition -DisplayId 3 -Position Above -RelativeDisplayId 1 | Use-DisplayConfig

Displays 1,2,3 are currently side by side.
This config stacks the displays on top of each other with 2 at the bottom, 1 in the middle and 3 at the top.

### Example 3

PS C:\> Set-DisplayPosition -DisplayId 3 -XPosition 0 -YPosition 100 -AsOffset

Moves display 3 100 pixels down from its current position.

### Example 4

PS C:\> Get-DisplayConfig | Set-DisplayPosition -DisplayId 1 -XPosition 2560 | Set-DisplayPosition -DisplayId 2 -XPosition 2560 | Use-DisplayConfig

Moves a cloned display group of displays 1 and 2 to a new X position.

## PARAMETERS

### -AllowChanges

Allow Windows to readjust positions to make the config work, in case of overlapping displays.

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

### -AsOffset

Treat the specified positions as an offset of the current position.

```yaml
Type: System.Management.Automation.SwitchParameter
DefaultValue: False
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: Position
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
- Name: (All)
  Position: Named
  IsRequired: false
  ValueFromPipeline: true
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -DisplayId

The display that should be moved.
DisplayIds in this module use a similar logic as the Windows Settings app to number the displays, but there's no guarantee that it will match on every system.
Displays are sorted by the output port on the adapter with the following priority: Internal displays (laptops), PC connectors (DVI, Displayport), HDMI and others.
When multiple displays use the same connector (eg. 2 DisplayPort monitors) Windows will assign an incrementing number for each instance, and this number is combined with the priority to determine the exact display order.
The only way to change the displayId of a display is to move it to a different port on the graphics adapter.

```yaml
Type: System.UInt32
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: Position
  Position: 0
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
- Name: OffsetFromDisplay
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

### -LeftToRightDisplayIds

Places the specified displays side by side, placing the first display as far left as possible, the second display right next to it, and so on.
Every active display needs to be specified.

```yaml
Type: System.UInt32[]
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: LeftToRight
  Position: Named
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -Position

The position where the specified display should be placed, relative to the display specified with `-RelativeDisplayId`.

```yaml
Type: MartinGC94.DisplayConfig.API.RelativePosition
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: OffsetFromDisplay
  Position: Named
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -RelativeDisplayId

Specifies the display that the `-Position` parameter refers to.

```yaml
Type: System.UInt32
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: OffsetFromDisplay
  Position: Named
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -SwapDisplay

The displayIDs of the 2 displays whose position should be swapped.
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
- Name: SwapDisplays
  Position: Named
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -XPosition

Specifies either the literal (default) or relative (when used with `-AsOffset`) X (horizontal) position where the display should be moved.

```yaml
Type: System.Int32
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: Position
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -YPosition

Specifies either the literal (default) or relative (when used with `-AsOffset`) Y (vertical) position where the display should be moved.

```yaml
Type: System.Int32
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: Position
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

### MartinGC94.DisplayConfig.API.DisplayConfig



## NOTES




## RELATED LINKS



