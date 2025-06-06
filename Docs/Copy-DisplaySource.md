---
external help file: DisplayConfig.dll-Help.xml
Module Name: DisplayConfig
online version:
schema: 2.0.0
---

# Copy-DisplaySource

## SYNOPSIS
Clones 1 source display to 1 or more destination displays.

## SYNTAX

### ApplyNow (Default)
```
Copy-DisplaySource [-DisplayId] <UInt32> [-DestinationDisplayId] <UInt32[]> [-DontSave] [<CommonParameters>]
```

### Config
```
Copy-DisplaySource [-DisplayId] <UInt32> [-DestinationDisplayId] <UInt32[]> -DisplayConfig <DisplayConfig>
 [<CommonParameters>]
```

## DESCRIPTION
Clones 1 source display to 1 or more destination displays.
To stop cloning you can either change the display profile to anything other than `Clone`, or use `Enable-Display`
Another way to clone displays without specifying a source is to use `Enable-Display` with the `-AsClone` switch.

## EXAMPLES

### Example 1
```powershell
PS C:\> Copy-DisplaySource -DisplayId 2 -DestinationDisplayId 1,3
```

Clones the second display to displays 1 and 3.

## PARAMETERS

### -DestinationDisplayId
The target display(s) to clone the source to.  
DisplayIds in this module use a similar logic as the Windows Settings app to number the displays, but there's no guarantee that it will match on every system.  
Displays are sorted by the output port on the adapter with the following priority: Internal displays (laptops), PC connectors (DVI, Displayport), HDMI and others.
When multiple displays use the same connector (eg. 2 DisplayPort monitors) Windows will assign an incrementing number for each instance, and this number is combined with the priority to determine the exact display order.  
The only way to change the displayId of a display is to move it to a different port on the graphics adapter.

```yaml
Type: UInt32[]
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
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
The source display to clone output from.  
DisplayIds in this module use a similar logic as the Windows Settings app to number the displays, but there's no guarantee that it will match on every system.  
Displays are sorted by the output port on the adapter with the following priority: Internal displays (laptops), PC connectors (DVI, Displayport), HDMI and others.
When multiple displays use the same connector (eg. 2 DisplayPort monitors) Windows will assign an incrementing number for each instance, and this number is combined with the priority to determine the exact display order.  
The only way to change the displayId of a display is to move it to a different port on the graphics adapter.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases: SourceDisplayId

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### MartinGC94.DisplayConfig.API.DisplayConfig

## OUTPUTS

### MartinGC94.DisplayConfig.API.DisplayConfig

## NOTES

## RELATED LINKS
