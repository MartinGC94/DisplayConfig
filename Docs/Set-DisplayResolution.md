---
external help file: DisplayConfig.dll-Help.xml
Module Name: DisplayConfig
online version:
schema: 2.0.0
---

# Set-DisplayResolution

## SYNOPSIS
Sets the resolution of the specified displays.

## SYNTAX

### ApplyNow (Default)
```
Set-DisplayResolution [-DisplayId] <UInt32[]> [-Width] <UInt32> [-Height] <UInt32> [-DontSave] [-AllowChanges]
 [<CommonParameters>]
```

### Config
```
Set-DisplayResolution [-DisplayId] <UInt32[]> [-Width] <UInt32> [-Height] <UInt32> [-ChangeAspectRatio]
 -DisplayConfig <DisplayConfig> [<CommonParameters>]
```

## DESCRIPTION
Sets the resolution of the specified displays.

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DisplayResolution -DisplayId 3 -Width 1600 -Height 900
```

Changes the resolution of display 3 to 1600 x 900.

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
The display(s) where the resolution should be changed.

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

### -Height
Specifies the height of the resolution, meaning the second number in the common "1920 x 1080" format.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Width
Specifies the width of the resolution, meaning the first number in the common "1920 x 1080" format.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -AllowChanges
Allows Windows to slightly adjust the mode (resolution and refresh rate) this can be necessary if certain refresh rates are only available at certain resolutions.

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

### -ChangeAspectRatio
Fixes an issue where the aspect ratio is not changed when changing to a resolution with a different aspect ratio.
Due to API limitations, no further changes can be made to a DisplayConfig after this parameter has been used, so this has to be used as a last step before applying it.

```yaml
Type: SwitchParameter
Parameter Sets: Config
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
