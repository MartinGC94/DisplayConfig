---
external help file: DisplayConfig.dll-Help.xml
Module Name: DisplayConfig
online version:
schema: 2.0.0
---

# Set-DisplayScale

## SYNOPSIS
Changes the DPI for the specified display(s).

## SYNTAX

### UserDefined
```
Set-DisplayScale [-DisplayId] <UInt32[]> [-Scale] <UInt32> [<CommonParameters>]
```

### Recommended
```
Set-DisplayScale [-DisplayId] <UInt32[]> [-Recommended] [<CommonParameters>]
```

## DESCRIPTION
Changes the DPI for the specified display(s).

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DisplayScale -DisplayId 1,2 -Scale 100
```

Sets the DPI scale to 100 for displays 1 and 2.

## PARAMETERS

### -DisplayId
The display(s) where the DPI should be changed.

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

### -Recommended
Sets the DPI to whatever Windows recommends.  
The DPI recommendation by Windows depends on the screen size, resolution and expected viewing distance.

```yaml
Type: SwitchParameter
Parameter Sets: Recommended
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scale
The DPI scale to set.

```yaml
Type: UInt32
Parameter Sets: UserDefined
Aliases:
Accepted values: 100, 125, 150, 175, 200, 225, 250, 300, 350, 400, 450, 500

Required: True
Position: 1
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
