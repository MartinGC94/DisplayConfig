---
document type: cmdlet
external help file: DisplayConfig.dll-Help.xml
HelpUri: ''
Locale: da-DK
Module Name: DisplayConfig
ms.date: 09-23-2025
PlatyPS schema version: 2024-05-01
title: Set-DisplayScale
---

# Set-DisplayScale

## SYNOPSIS

Changes the DPI for the specified display(s).

## SYNTAX

### UserDefined

```
Set-DisplayScale [-DisplayId] <uint[]> [-Scale] <uint> [<CommonParameters>]
```

### Recommended

```
Set-DisplayScale [-DisplayId] <uint[]> -Recommended [<CommonParameters>]
```

## ALIASES

This cmdlet has the following aliases,
  None

## DESCRIPTION

Changes the DPI for the specified display(s).

## EXAMPLES

### Example 1

PS C:\> Set-DisplayScale -DisplayId 1,2 -Scale 100

Sets the DPI scale to 100 for displays 1 and 2.

## PARAMETERS

### -DisplayId

The display(s) where the DPI should be changed.

```yaml
Type: System.UInt32[]
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: 0
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -Recommended

Sets the DPI to whatever Windows recommends.
The DPI recommendation by Windows depends on the screen size, resolution and expected viewing distance.

```yaml
Type: System.Management.Automation.SwitchParameter
DefaultValue: False
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: Recommended
  Position: Named
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -Scale

The DPI scale to set.

```yaml
Type: System.UInt32
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: UserDefined
  Position: 1
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable,
-InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable,
-ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see
[about_CommonParameters](https://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

## OUTPUTS



## NOTES




## RELATED LINKS



