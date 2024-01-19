[ValidateNotNull()]
$SolutionFile = Get-ChildItem -Path $PSScriptRoot -Filter *.sln -File | Select-Object -First 1
MSBuild.exe $SolutionFile.FullName