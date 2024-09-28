---
external help file: DisplayConfig.dll-Help.xml
Module Name: DisplayConfig
online version:
schema: 2.0.0
---

# Enable-Display

## SYNOPSIS
Reenables a display, this is the same as picking "Extend desktop to this display" in the settings app.  
If the display is already active, but is used for display cloning then this will stop the cloning.

## SYNTAX

```
Enable-Display [-DisplayId] <UInt32[]> [-AsClone] [<CommonParameters>]
```

## DESCRIPTION
Reenables a display, this is the same as picking "Extend desktop to this display" in the settings app.  
If the display is already active, but is used for display cloning then this will stop the cloning.

## EXAMPLES

### Example 1
```powershell
PS C:\> Enable-Display -DisplayId 3
```

Reenables the third display, adding it back to the desktop if it was previously turned off or used for cloning.

## PARAMETERS

### -DisplayId
The display to enable.  
DisplayIds in this module use a similar logic as the Windows Settings app to number the displays, but there's no guarantee that it will match on every system.  
Displays are sorted by the output port on the adapter with the following priority: Internal displays (laptops), PC connectors (DVI, Displayport), HDMI and others.
When multiple displays use the same connector (eg. 2 DisplayPort monitors) Windows will assign an incrementing number for each instance, and this number is combined with the priority to determine the exact display order.  
The only way to change the displayId of a display is to move it to a different port on the graphics adapter.

```yaml
Type: UInt32[]
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -AsClone
When specified, the specified displays are put into one clone group.
This allows you to turn on displays and add them to a clone group in one go.
You can use this over Copy-DisplaySource when you want to let Windows determine the best mode, rather than copying it from an existing display.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
