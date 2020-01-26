Param(
    [parameter(mandatory=$true)][string] $platform
)
$ErrorActionPreference = 'Stop'

$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path

$scriptFileNames = @(
    'command.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
    $scriptFilePath = Join-Path $currentDirPath $scriptFileName
    . $scriptFilePath
}

Set-Command 'git' 'BUILD_GIT_PATH' "%PROGRAMFILES%\git\bin"
Set-Command 'msbuild' 'BUILD_MSBUILD_PATH' "%PROGRAMFILES(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin"
Set-Command 'dotnet' 'BUILD_DOTNET_PATH' "%PROGRAMFILES(x86)%\dotnet\"

Write-Output ("git: " + (git --version))
Write-Output ("msbuild: " + (msbuild -version -noLogo))
Write-Output ("dotnet: " + (dotnet --version))

# SCM 的に現行状態に未コミットがあれば死ぬ
if((git status -s | Measure-Object).Count -ne 0) {
    #throw "変更あり"
}

$rootDirectory = Split-Path -Path $currentDirPath -Parent
try {
    Push-Location $rootDirectory

    msbuild Source/Pe.Boot/Pe.Boot.sln                                 /p:Configuration=Release                         /p:Platform=%PLATFORM% /p:DefineConstants="%BUILD_TYPE%"

} finally {
    Pop-Location
}
