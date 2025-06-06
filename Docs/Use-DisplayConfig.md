---
external help file: DisplayConfig.dll-Help.xml
Module Name: DisplayConfig
online version:
schema: 2.0.0
---

# Use-DisplayConfig

## SYNOPSIS
Applies the specified Display topology settings.

## SYNTAX

```
Use-DisplayConfig -DisplayConfig <DisplayConfig> [-AllowChanges] [-DontSave] [-Flags <SetDisplayConfigFlags>]
 [-UpdateAdapterIds] [<CommonParameters>]
```

## DESCRIPTION
Applies the specified Display topology settings.

## EXAMPLES

### Example 1
```powershell
PS C:\> Import-Clixml $HOME\TvProfile.xml | Use-DisplayConfig -UpdateAdapterIds
```

Imports a profile that was stored on the filesystem and applies it to the current configuration.

## PARAMETERS

### -AllowChanges
Allow Windows to make slight adjustments to the configuration (readjusting desktop positions, changing the refresh rate, etc.).  
If this is not set, and the configuration contains invalid data then an error will be thrown.

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

### -DisplayConfig
The DisplayConfig to apply.

```yaml
Type: DisplayConfig
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -DontSave
Skips saving this configuration change to the configuration database, allowing you to roll back the changes with: `Undo-DisplayConfigChanges`.

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

### -Flags
Only for advanced users!  
You can read what each flag does, and how they interact here: https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setdisplayconfig#parameters

```yaml
Type: SetDisplayConfigFlags
Parameter Sets: (All)
Aliases:
Accepted values: SDC_TOPOLOGY_INTERNAL, SDC_TOPOLOGY_CLONE, SDC_TOPOLOGY_EXTEND, SDC_TOPOLOGY_EXTERNAL, SDC_USE_DATABASE_CURRENT, SDC_TOPOLOGY_SUPPLIED, SDC_USE_SUPPLIED_DISPLAY_CONFIG, SDC_VALIDATE, SDC_APPLY, SDC_NO_OPTIMIZATION, SDC_SAVE_TO_DATABASE, SDC_ALLOW_CHANGES, SDC_PATH_PERSIST_IF_REQUIRED, SDC_FORCE_MODE_ENUMERATION, SDC_ALLOW_PATH_ORDER_CHANGES, SDC_VIRTUAL_MODE_AWARE, SDC_VIRTUAL_REFRESH_RATE_AWARE

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -UpdateAdapterIds
Gets the current display adapter IDs and updates the display config to use those before applying the config.
This is useful if the displayconfig has been imported from a file because Windows may have changed the IDs since the file was exported.

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

### MartinGC94.DisplayConfig.API.DisplayConfig

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
