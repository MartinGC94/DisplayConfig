---
external help file: DisplayConfig.dll-Help.xml
Module Name: DisplayConfig
online version:
schema: 2.0.0
---

# Get-DisplayColorInfo

## SYNOPSIS
Gets various color and HDR details about a display.

## SYNTAX

```
Get-DisplayColorInfo [-DisplayId] <UInt32[]> [<CommonParameters>]
```

## DESCRIPTION
Gets various color and HDR details about a display.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DisplayColorInfo -DisplayId 3
```

Gets color and HDR info for the third display.

## PARAMETERS

### -DisplayId
The display to get color data for.  
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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### MartinGC94.DisplayConfig.API.ColorInfo

## NOTES

## RELATED LINKS
