$ErrorActionPreference = 'Stop'
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
    'command.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
    $scriptFilePath = Join-Path $currentDirPath $scriptFileName
    . $scriptFilePath
}

if(TestAliasExists curl) {
    Remove-Item curl
}

