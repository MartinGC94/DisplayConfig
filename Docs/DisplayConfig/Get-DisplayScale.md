---
document type: cmdlet
external help file: DisplayConfig.dll-Help.xml
HelpUri: ''
Locale: da-DK
Module Name: DisplayConfig
ms.date: 09-23-2025
PlatyPS schema version: 2024-05-01
title: Get-DisplayScale
---

# Get-DisplayScale

## SYNOPSIS

Gets information about the current, recommended and max DPI for the specified displays.

## SYNTAX

### __AllParameterSets

```
Get-DisplayScale [-DisplayId] <uint[]> [<CommonParameters>]
```

## ALIASES

This cmdlet has the following aliases,
  None

## DESCRIPTION

Gets information about the current, recommended and max DPI for the specified displays.

## EXAMPLES

### Example 1

PS C:\> Get-DisplayScale 1,2

Retrieves the DPI scaling information for displays 1 and 2.

## PARAMETERS

### -DisplayId

Specifies the displays to get the scaling information from.

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

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable,
-InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable,
-ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see
[about_CommonParameters](https://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

## OUTPUTS

### MartinGC94.DisplayConfig.API.DpiScale



## NOTES




## RELATED LINKS



