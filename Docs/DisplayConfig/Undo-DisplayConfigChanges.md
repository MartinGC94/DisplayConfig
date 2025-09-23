---
document type: cmdlet
external help file: DisplayConfig.dll-Help.xml
HelpUri: ''
Locale: da-DK
Module Name: DisplayConfig
ms.date: 09-23-2025
PlatyPS schema version: 2024-05-01
title: Undo-DisplayConfigChanges
---

# Undo-DisplayConfigChanges

## SYNOPSIS

Reverts any temporary display settings by resetting the display settings to the ones saved in the configuration database.

## SYNTAX

### __AllParameterSets

```
Undo-DisplayConfigChanges [<CommonParameters>]
```

## ALIASES

This cmdlet has the following aliases,
  None

## DESCRIPTION

Reverts any temporary display settings by resetting the display settings to the ones saved in the configuration database.
Temporary display settings can be set by using the `DontSave` parameter on supported commands.
For a list of supported commands, see: `Get-Command -Module DisplayConfig -ParameterName DontSave`

## EXAMPLES

### Example 1

PS C:\> Undo-DisplayConfigChanges

Reverts any temporary display settings by resetting the display settings to the ones saved in the configuration database.

## PARAMETERS

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable,
-InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable,
-ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see
[about_CommonParameters](https://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

## OUTPUTS





## NOTES




## RELATED LINKS



