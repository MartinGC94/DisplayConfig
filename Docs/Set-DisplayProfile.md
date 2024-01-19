---
external help file: DisplayConfig.dll-Help.xml
Module Name: DisplayConfig
online version:
schema: 2.0.0
---

# Set-DisplayProfile

## SYNOPSIS
Changes the active display profile. This works the same way the Windows key + P does.

## SYNTAX

```
Set-DisplayProfile [-Profile] <TopologyProfile> [<CommonParameters>]
```

## DESCRIPTION
Changes the active display profile. This works the same way the Windows key + P does.

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DisplayProfile Clone
```

Switches to the most recent clone configuration.

## PARAMETERS

### -Profile
The profile to use.

```yaml
Type: TopologyProfile
Parameter Sets: (All)
Aliases:
Accepted values: Internal, Clone, Extend, External

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

### System.Object
## NOTES

## RELATED LINKS
