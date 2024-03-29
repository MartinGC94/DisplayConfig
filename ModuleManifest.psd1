@{
    RootModule             = "DisplayConfig.dll"
    ModuleVersion          = {0}
    CompatiblePSEditions   = @("Core", "Desktop")
    GUID                   = '869da43f-8ec3-4ee1-8820-f531dbc455d6'
    Author                 = 'MartinGC94'
    CompanyName            = 'Unknown'
    Copyright              = '(c) 2024 MartinGC94. All rights reserved.'
    Description            = 'Manage Windows display settings like resolution, DPI scale, HDR and more.'
    PowerShellVersion      = '5.1'
    RequiredAssemblies     = if ($PSEdition -eq "Desktop") {"DisplayConfig.dll"} else {$null}
    TypesToProcess         = @({1})
    FormatsToProcess       = @({2})
    FunctionsToExport      = @()
    CmdletsToExport        = @({3})
    VariablesToExport      = @()
    AliasesToExport        = @()
    DscResourcesToExport   = @()
    FileList               = @({4})
    PrivateData            = @{
        PSData = @{
             Tags         = @("Display", "Monitor", "Settings","Options","Configuration", "Resolution", "DPI", "Scale", "HDR", "Rotation")
             ProjectUri   = 'https://github.com/MartinGC94/DisplayConfig'
             ReleaseNotes = @'
{5}
'@
        }
    }
}