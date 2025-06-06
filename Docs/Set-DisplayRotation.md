---
external help file: DisplayConfig.dll-Help.xml
Module Name: DisplayConfig
online version:
schema: 2.0.0
---

# Set-DisplayRotation

## SYNOPSIS
Rotates the image of a display so it matches the physical setup.

## SYNTAX

### ApplyNow (Default)
```
Set-DisplayRotation [-DisplayId] <UInt32[]> [-Rotation] <DisplayRotation> [-DontSave] [<CommonParameters>]
```

### Config
```
Set-DisplayRotation [-DisplayId] <UInt32[]> [-Rotation] <DisplayRotation> -DisplayConfig <DisplayConfig>
 [<CommonParameters>]
```

## DESCRIPTION
Rotates the image of a display so it matches the physical setup.

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DisplayRotation -DisplayId 3 -Rotation Rotate90
```

Rotates the third display 90 degrees counterclockwise.

## PARAMETERS

### -DisplayConfig
The displayConfig where this setting should be applied. See the help for `Get-DisplayConfig` for more info.

```yaml
Type: DisplayConfig
Parameter Sets: Config
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -DisplayId
The display(s) that should be rotated.  
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

### -DontSave
Skips saving this configuration change to the configuration database, allowing you to roll back the changes with: `Undo-DisplayConfigChanges`.

```yaml
Type: SwitchParameter
Parameter Sets: ApplyNow
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Rotation
The amount of rotation in degrees (counterclockwise) that the display(s) should be rotated.

```yaml
Type: DisplayRotation
Parameter Sets: (All)
Aliases:
Accepted values: None, Rotate90, Rotate180, Rotate270

Required: True
Position: 1
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
