---
external help file: DisplayConfig.dll-Help.xml
Module Name: DisplayConfig
online version:
schema: 2.0.0
---

# Set-DisplayHDR

## SYNOPSIS
Manages global and display specific HDR settings.

## SYNTAX

### DisplaySpecific
```
Set-DisplayHDR [-DisplayId] <UInt32[]> [-SdrWhiteLevel <UInt32>] [-EnableHDR] [<CommonParameters>]
```

### Global
```
Set-DisplayHDR [-EnableHDRPlayback] [-EnableAutoHDR] [<CommonParameters>]
```

## DESCRIPTION
Manages global and display specific HDR settings.

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DisplayHDR -DisplayId 3 -SdrWhiteLevel 3000 -EnableHDR
```

Enables HDR and sets the Sdr white level to 3000

## PARAMETERS

### -DisplayId
Specifies the displays where display specific settings should be applied.

```yaml
Type: UInt32[]
Parameter Sets: DisplaySpecific
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -EnableAutoHDR
Enables/disables the auto HDR feature which tries to convert SDR content into HDR. This feature was introduced in Windows 11.

```yaml
Type: SwitchParameter
Parameter Sets: Global
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -EnableHDR
Enables/disables HDR on the specified display. This is the same as running `Enable-DisplayAdvancedColor` or `Disable-DisplayAdvancedColor`.

```yaml
Type: SwitchParameter
Parameter Sets: DisplaySpecific
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -EnableHDRPlayback
Enables/disables the "Stream HDR-videos" option globally.

```yaml
Type: SwitchParameter
Parameter Sets: Global
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SdrWhiteLevel
Sets the SDR white level for the specified displays. This is equivalent to the HDR/SDR slider in the settings app.

```yaml
Type: UInt32
Parameter Sets: DisplaySpecific
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
