$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path

$scriptFileNames = @(
    'set-appenv-appveyor.ps1',
    'set-appenv.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
    $scriptFilePath = Join-Path $currentDirPath $scriptFileName
    . $scriptFilePath
}
$buildVariables = Get-BuildVariable
Write-Output $buildVariables


# SCM 的に現行状態に未コミットがあれば死ぬ

