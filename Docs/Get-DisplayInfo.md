---
external help file: DisplayConfig.dll-Help.xml
Module Name: DisplayConfig
online version:
schema: 2.0.0
---

# Get-DisplayInfo

## SYNOPSIS
Gets various details about the connected displays. Information includes: Display ID, display model name, connection type, and more.

## SYNTAX

```
Get-DisplayInfo [[-DisplayId] <UInt32[]>] [<CommonParameters>]
```

## DESCRIPTION
Gets various details about the connected displays. Information includes: Display ID, display model name, connection type, and more.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DisplayInfo
```

Gets basic display info about all connected displays.

## PARAMETERS

### -DisplayId
Specifies the display(s) to get information about.

```yaml
Type: UInt32[]
Parameter Sets: (All)
Aliases:

Required: False
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

### MartinGC94.DisplayConfig.API.DisplayInfo

## NOTES

## RELATED LINKS
