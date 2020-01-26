
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path

$scriptFileNames = @(
    'test-command.ps1',
    'buildvar-appveyor.ps1',
    'buildvar.ps1',
    'scm-git.ps1',
    'scm.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
    $scriptFilePath = Join-Path $currentDirPath $scriptFileName
    . $scriptFilePath
}
$buildVariables = Get-BuildVariable
Write-Output $buildVariables

# SCM 的に現行状態に未コミットがあれば死ぬ
Initialize-Scm

echo (Get-ChangedScm)
