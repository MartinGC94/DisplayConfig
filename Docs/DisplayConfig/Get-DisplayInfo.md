---
document type: cmdlet
external help file: DisplayConfig.dll-Help.xml
HelpUri: ''
Locale: da-DK
Module Name: DisplayConfig
ms.date: 09-23-2025
PlatyPS schema version: 2024-05-01
title: Get-DisplayInfo
---

# Get-DisplayInfo

## SYNOPSIS

Gets various details about the connected displays. Information includes: Display ID, display model name, connection type, and more.

## SYNTAX

### __AllParameterSets

```
Get-DisplayInfo [[-DisplayId] <uint[]>] [-DisplayConfig <DisplayConfig>] [<CommonParameters>]
```

## ALIASES

This cmdlet has the following aliases,
  None

## DESCRIPTION

Gets various details about the connected displays.
Information includes: Display ID, display model name, connection type, and more.

## EXAMPLES

### Example 1

PS C:\> Get-DisplayInfo

Gets basic display info about all connected displays.

## PARAMETERS

### -DisplayConfig

The displayConfig to get displayinfo from.
See the help for `Get-DisplayConfig` for more info.

```yaml
Type: MartinGC94.DisplayConfig.API.DisplayConfig
DefaultValue: ''
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: Named
  IsRequired: false
  ValueFromPipeline: true
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -DisplayId

Specifies the display(s) to get information about.

```yaml
Type: System.UInt32[]
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: 0
  IsRequired: false
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

### MartinGC94.DisplayConfig.API.DisplayConfig



## OUTPUTS

### MartinGC94.DisplayConfig.API.DisplayInfo



## NOTES




## RELATED LINKS



