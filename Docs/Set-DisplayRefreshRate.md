---
external help file: DisplayConfig.dll-Help.xml
Module Name: DisplayConfig
online version:
schema: 2.0.0
---

# Set-DisplayRefreshRate

## SYNOPSIS
Changes the refresh rate of the specified display.

## SYNTAX

### ApplyNow (Default)
```
Set-DisplayRefreshRate [-DisplayId] <UInt32[]> [-RefreshRate] <Double> [-DontSave] [-AllowChanges]
 [<CommonParameters>]
```

### Config
```
Set-DisplayRefreshRate -DisplayConfig <DisplayConfig> [-DisplayId] <UInt32[]> [-RefreshRate] <Double>
 [<CommonParameters>]
```

## DESCRIPTION
Changes the refresh rate of the specified display.

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DisplayRefreshRate -DisplayId 2 -RefreshRate 165
```

Sets the refresh rate to 165 Hz for display 2.

## PARAMETERS

### -AllowChanges
Allows Windows to slightly adjust the specified refresh rate so it matches the monitor.  
This is highly recommended because displays often require decimal numbers like "59.951" rather than round numbers like "60".

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
The display(s) where the refresh rate should be changed.  
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

### -RefreshRate
The refresh rate to set.

```yaml
Type: Double
Parameter Sets: (All)
Aliases:

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
