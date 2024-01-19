---
external help file: DisplayConfig.dll-Help.xml
Module Name: DisplayConfig
online version:
schema: 2.0.0
---

# Undo-DisplayConfigChanges

## SYNOPSIS
Reverts any temporary display settings by resetting the display settings to the ones saved in the configuration database.

## SYNTAX

```
Undo-DisplayConfigChanges [<CommonParameters>]
```

## DESCRIPTION
Reverts any temporary display settings by resetting the display settings to the ones saved in the configuration database.  
Temporary display settings can be set by using the `DontSave` parameter on supported commands.  
For a list of supported commands, see: `Get-Command -Module DisplayConfig -ParameterName DontSave`

## EXAMPLES

### Example 1
```powershell
PS C:\> Undo-DisplayConfigChanges
```

Reverts any temporary display settings by resetting the display settings to the ones saved in the configuration database.

## PARAMETERS

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
