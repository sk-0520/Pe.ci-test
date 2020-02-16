Param(
    [parameter(mandatory=$true)][string] $SourceDirectory,
    [parameter(mandatory=$true)][string] $DestinationDirectory,
    [parameter(mandatory=$true)][string] $Platform,
    [switch] $Diet
)
$ErrorActionPreference = 'Stop'
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
    'command.ps1',
    'version.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
    $scriptFilePath = Join-Path $currentDirPath $scriptFileName
    . $scriptFilePath
}


$vesion = GetAppVersion

$destinationPath = Join-Path $DestinationDirectory ("Pe_" + $vesion + "_" + $Platform + ".zip")

Compress-Archive -Force -Path (Join-Path $SourceDirectory "*") -DestinationPath $destinationPath

