---
document type: cmdlet
external help file: DisplayConfig.dll-Help.xml
HelpUri: ''
Locale: da-DK
Module Name: DisplayConfig
ms.date: 09-23-2025
PlatyPS schema version: 2024-05-01
title: Set-DisplayProfile
---

# Set-DisplayProfile

## SYNOPSIS

Changes the active display profile. This works the same way the Windows key + P does.

## SYNTAX

### __AllParameterSets

```
Set-DisplayProfile [-Profile] <TopologyProfile> [<CommonParameters>]
```

## ALIASES

This cmdlet has the following aliases,
  None

## DESCRIPTION

Changes the active display profile.
This works the same way the Windows key + P does.

## EXAMPLES

### Example 1

PS C:\> Set-DisplayProfile Clone

Switches to the most recent clone configuration.

## PARAMETERS

### -Profile

The profile to use.

```yaml
Type: MartinGC94.DisplayConfig.API.TopologyProfile
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



## NOTES




## RELATED LINKS



