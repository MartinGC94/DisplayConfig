---
external help file: DisplayConfig.dll-Help.xml
Module Name: DisplayConfig
online version:
schema: 2.0.0
---

# Set-DisplayPosition

## SYNOPSIS
Changes the position of one or more displays.

## SYNTAX

### PositionConfig
```
Set-DisplayPosition -DisplayConfig <DisplayConfig> [-DisplayId] <UInt32> -XPosition <Int32> -YPosition <Int32>
 [-AsOffset] [<CommonParameters>]
```

### OffsetFromDisplayConfig
```
Set-DisplayPosition -DisplayConfig <DisplayConfig> [-DisplayId] <UInt32> -RelativeDisplayId <UInt32>
 -Position <RelativePosition> [<CommonParameters>]
```

### LeftToRightConfig
```
Set-DisplayPosition -DisplayConfig <DisplayConfig> -LeftToRightDisplayIds <UInt32[]> [<CommonParameters>]
```

### Position
```
Set-DisplayPosition [-DisplayId] <UInt32> -XPosition <Int32> -YPosition <Int32> [-AsOffset] [-DontSave]
 [-AllowChanges] [-ApplyNow] [<CommonParameters>]
```

### OffsetFromDisplay
```
Set-DisplayPosition [-DisplayId] <UInt32> -RelativeDisplayId <UInt32> -Position <RelativePosition> [-DontSave]
 [-AllowChanges] [-ApplyNow] [<CommonParameters>]
```

### LeftToRight
```
Set-DisplayPosition -LeftToRightDisplayIds <UInt32[]> [-DontSave] [-AllowChanges] [-ApplyNow]
 [<CommonParameters>]
```

## DESCRIPTION
Changes the position of one or more displays.  
This does not move displays that are already in the specified position so it may be necessary to combine this command with `Get-DisplayConfig` and run this multiple times to move the displays one by one.

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DisplayPosition -LeftToRightDisplayIds 2,1,3 -ApplyNow
```

Positions displays 1,2,3 side by side with 2 being all the way to the left, 1 being in the middle and 3 being all the way to the right.

### Example 1
```powershell
PS C:\> Set-DisplayPosition -LeftToRightDisplayIds 2,1,3 -ApplyNow
```

Positions displays 1,2,3 side by side with 2 being all the way to the left, 1 being in the middle and 3 being all the way to the right.

### Example 2
```powershell
PS C:\> Get-DisplayConfig | Set-DisplayPosition -DisplayId 1 -Position Above -RelativeDisplayId 2 | Set-DisplayPosition -DisplayId 3 -Position Above -RelativeDisplayId 1 | Use-DisplayConfig
```

Displays 1,2,3 are currently side by side. This config stacks the displays on top of each other with 2 at the bottom, 1 in the middle and 3 at the top.

### Example 3
```powershell
PS C:\> Set-DisplayPosition -DisplayId 3 -XPosition 0 -YPosition 100 -AsOffset
```

Moves display 3 100 pixels down from its current position.

## PARAMETERS

### -AllowChanges
Allow Windows to readjust positions to make the config work, in case of overlapping displays.

```yaml
Type: SwitchParameter
Parameter Sets: Position, OffsetFromDisplay, LeftToRight
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ApplyNow
Helps PowerShell resolve the correct parameter set to apply the configuration changes immediately. The actual value is not used.

```yaml
Type: SwitchParameter
Parameter Sets: Position, OffsetFromDisplay, LeftToRight
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -AsOffset
Treat the specified positions as an offset of the current position.

```yaml
Type: SwitchParameter
Parameter Sets: PositionConfig, Position
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DisplayConfig
The displayConfig where this setting should be applied. See the help for `Get-DisplayConfig` for more info.

```yaml
Type: DisplayConfig
Parameter Sets: PositionConfig, OffsetFromDisplayConfig, LeftToRightConfig
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -DisplayId
The display that should be moved.  
DisplayIds in this module use a similar logic as the Windows Settings app to number the displays, but there's no guarantee that it will match on every system.  
Displays are sorted by the output port on the adapter with the following priority: Internal displays (laptops), PC connectors (DVI, Displayport), HDMI and others.
When multiple displays use the same connector (eg. 2 DisplayPort monitors) Windows will assign an incrementing number for each instance, and this number is combined with the priority to determine the exact display order.  
The only way to change the displayId of a display is to move it to a different port on the graphics adapter.

```yaml
Type: UInt32
Parameter Sets: PositionConfig, OffsetFromDisplayConfig, Position, OffsetFromDisplay
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DontSave
Skips saving this configuration change to the configuration database, allowing you to roll back the changes with: `Undo-DisplayConfigChanges`.

```yaml
Type: SwitchParameter
Parameter Sets: Position, OffsetFromDisplay, LeftToRight
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -LeftToRightDisplayIds
Places the specified displays side by side, placing the first display as far left as possible, the second display right next to it, and so on.
Every active display needs to be specified.

```yaml
Type: UInt32[]
Parameter Sets: LeftToRightConfig, LeftToRight
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Position
The position where the specified display should be placed, relative to the display specified with `-RelativeDisplayId`.

```yaml
Type: RelativePosition
Parameter Sets: OffsetFromDisplayConfig, OffsetFromDisplay
Aliases:
Accepted values: Left, Right, Above, Under

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RelativeDisplayId
Specifies the display that the `-Position` parameter refers to.

```yaml
Type: UInt32
Parameter Sets: OffsetFromDisplayConfig, OffsetFromDisplay
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -XPosition
Specifies either the literal (default) or relative (when used with `-AsOffset`) X (horizontal) position where the display should be moved.

```yaml
Type: Int32
Parameter Sets: PositionConfig, Position
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -YPosition
Specifies either the literal (default) or relative (when used with `-AsOffset`) Y (vertical) position where the display should be moved.

```yaml
Type: Int32
Parameter Sets: PositionConfig, Position
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### MartinGC94.DisplayConfig.API.DisplayConfig

## OUTPUTS

### MartinGC94.DisplayConfig.API.DisplayConfig

## NOTES

## RELATED LINKS
