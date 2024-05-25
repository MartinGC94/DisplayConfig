---
external help file: DisplayConfig.dll-Help.xml
Module Name: DisplayConfig
online version:
schema: 2.0.0
---

# Get-DisplayHDR

## SYNOPSIS
Gets global and display specific HDR settings.

## SYNTAX

### DisplaySpecific (Default)
```
Get-DisplayHDR [-DisplayId] <UInt32[]> [<CommonParameters>]
```

### Global
```
Get-DisplayHDR [-Global] [<CommonParameters>]
```

## DESCRIPTION
Gets global and display specific HDR settings.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DisplayHDR -Global

HDRPlaybackEnabled AutoHDREnabled
------------------ --------------
             False          False
```

Lists global HDR settings.

## PARAMETERS

### -DisplayId
Specifies the displays to get HDR info from.

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

### -Global
Specifies that the global HDR settings should be retrieved.

```yaml
Type: SwitchParameter
Parameter Sets: Global
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

### None

## OUTPUTS

### MartinGC94.DisplayConfig.API.HDRDisplayInfo

### MartinGC94.DisplayConfig.API.HDRGlobalInfo

## NOTES

## RELATED LINKS
