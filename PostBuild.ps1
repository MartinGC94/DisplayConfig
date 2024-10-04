#Requires -Version 5.1 -Modules platyPS
param
(
    [Parameter(Mandatory)]
    [string]
    $OutputFile
)

$ModuleName = [System.IO.Path]::GetFileNameWithoutExtension($OutputFile)
$OutputDir = Split-Path -LiteralPath $OutputFile
$FormatFiles = Get-ChildItem -LiteralPath "$PSScriptRoot\PowerShellFormatFiles" -File | Copy-Item -Destination $OutputDir -Force -PassThru
$TypeFiles = Get-ChildItem -LiteralPath "$PSScriptRoot\PowerShellTypeFiles" -File | Copy-Item -Destination $OutputDir -Force -PassThru

$ModuleInfo = Import-Module -Name $OutputFile -PassThru -ErrorAction Stop
$CmdletsToExport = $ModuleInfo.ExportedCmdlets.Keys.ForEach({"'$_'"}) -join ','
Remove-Module -ModuleInfo $ModuleInfo

#region Create help files
$DocFiles = Get-ChildItem -LiteralPath (Join-Path -Path $PSScriptRoot -ChildPath Docs) -ErrorAction Ignore
if ($DocFiles)
{
    $null = New-Item -Path "$OutputDir\en-US" -ItemType Directory -Force
    $HelpFile = New-ExternalHelp -Path "$PSScriptRoot\Docs" -OutputPath "$OutputDir\en-US" -ErrorAction Stop -Force
    $HelpContent = [xml]::new()
    $HelpContent.Load($HelpFile.FullName)
    $HelpExamples = Select-Xml -Xml $HelpContent.helpItems -XPath //command:example -Namespace @{command = "http://schemas.microsoft.com/maml/dev/command/2004/10"}
    foreach ($Example in $HelpExamples)
    {
        # Adds 2 linebreaks between each example
        for ($i = 0; $i -lt 2; $i++)
        {
            $Element = $HelpContent.CreateElement('maml', 'para', 'http://schemas.microsoft.com/maml/2004/10')
            $null = $Example.Node.remarks.AppendChild($Element)
        }
    }
    $HelpContent.Save($HelpFile.FullName)
}
#endregion

#region Update module manifest
$null = New-Item -Path "$OutputDir\$ModuleName.psd1" -ItemType File -Force
$FormatList = ($FormatFiles | ForEach-Object -Process {
    "'$($_.Name)'"
}) -join ','
$TypeList = ($TypeFiles | ForEach-Object -Process {
    "'$($_.Name)'"
}) -join ','
$FileList = (Get-ChildItem -LiteralPath $OutputDir -File -Recurse | ForEach-Object -Process {
    "'$($_.FullName.Replace("$OutputDir\", ''))'"
}) -join ','
$ReleaseNotes = Get-Content -LiteralPath "$PSScriptRoot\Release notes.txt" -Raw
$Version = $ReleaseNotes.Split(':')[0]

((Get-Content -LiteralPath "$PSScriptRoot\ModuleManifest.psd1" -Raw) -replace '{(?=[^\d])','{{' -replace '(?<!\d)}','}}') -f @(
    "'$Version'"
    $TypeList
    $FormatList
    $CmdletsToExport
    $FileList
    $ReleaseNotes
) | Set-Content -LiteralPath "$OutputDir\$ModuleName.psd1" -Force
#endregion

$ReleaseDir = Join-Path -Path $PSScriptRoot -ChildPath "Releases\$ModuleName\$Version"
Get-Item -LiteralPath $ReleaseDir -Force -ErrorAction Ignore | Remove-Item -Recurse -Force -ErrorAction Stop
$null = New-Item -Path $ReleaseDir -ItemType Directory -Force
Get-ChildItem -LiteralPath $OutputDir | Copy-Item -Destination $ReleaseDir -Force -Recurse -Container -ErrorAction Stop