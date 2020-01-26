
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path

$scriptFileNames = @(
    'command.ps1',
    'buildvar-appveyor.ps1',
    'buildvar.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
    $scriptFilePath = Join-Path $currentDirPath $scriptFileName
    . $scriptFilePath
}

Set-Command 'git' 'BUILD_GIT_PATH' "%PROGRAMFILES%\git\bin"
Set-Command 'msbuild' 'BUILD_MSBUILD_PATH' "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin"

Write-Output ("git: " + (git --version))
Write-Output ("msbuild: " + (msbuild -version -noLogo ))

$buildVariables = Get-BuildVariable
Write-Output $buildVariables

# SCM 的に現行状態に未コミットがあれば死ぬ


