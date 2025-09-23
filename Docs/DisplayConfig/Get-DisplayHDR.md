---
document type: cmdlet
external help file: DisplayConfig.dll-Help.xml
HelpUri: ''
Locale: da-DK
Module Name: DisplayConfig
ms.date: 09-23-2025
PlatyPS schema version: 2024-05-01
title: Get-DisplayHDR
---

# Get-DisplayHDR

## SYNOPSIS

Gets global and display specific HDR settings.

## SYNTAX

### DisplaySpecific (Default)

```
Get-DisplayHDR [-DisplayId] <uint[]> [<CommonParameters>]
```

### Global

```
Get-DisplayHDR -Global [<CommonParameters>]
```

## ALIASES

This cmdlet has the following aliases,
  None

## DESCRIPTION

Gets global and display specific HDR settings.

## EXAMPLES

### Example 1

PS C:\> Get-DisplayHDR -Global

HDRPlaybackEnabled AutoHDREnabled
------------------ --------------
             False          False

Lists global HDR settings.

## PARAMETERS

### -DisplayId

Specifies the displays to get HDR info from.

```yaml
Type: System.UInt32[]
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: DisplaySpecific
  Position: 0
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -Global

Specifies that the global HDR settings should be retrieved.

```yaml
Type: System.Management.Automation.SwitchParameter
DefaultValue: False
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: Global
  Position: Named
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

### MartinGC94.DisplayConfig.API.HDRDisplayInfo



### MartinGC94.DisplayConfig.API.HDRGlobalInfo



## NOTES




## RELATED LINKS



