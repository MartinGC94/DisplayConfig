---
external help file: DisplayConfig.dll-Help.xml
Module Name: DisplayConfig
online version:
schema: 2.0.0
---

# Get-DisplayScale

## SYNOPSIS
Gets information about the current, recommended and max DPI for the specified displays.

## SYNTAX

```
Get-DisplayScale [-DisplayId] <UInt32[]> [<CommonParameters>]
```

## DESCRIPTION
Gets information about the current, recommended and max DPI for the specified displays.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DisplayScale 1,2
```

Retrieves the DPI scaling information for displays 1 and 2.

## PARAMETERS

### -DisplayId
Specifies the displays to get the scaling information from.

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

### MartinGC94.DisplayConfig.API.DpiScale

## NOTES

## RELATED LINKS
