Param(
    [parameter(mandatory=$true)][string] $platform,
    [string] $buildType
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
    throw "変更あり"
}


$rootDirectory = Split-Path -Path $currentDirPath -Parent
try {
    Push-Location $rootDirectory

    $projectXml = [XML](Get-Content -Path Source/Pe/Pe.Main/Pe.Main.csproj)
    $projectNav = $projectXml.CreateNavigator()
    $vesion = $projectNav.Select('//PropertyGroup/Version').Value
    $revision = (git rev-parse HEAD)


    # ビルド開始
    # msbuild        Source/Pe.Boot/Pe.Boot.sln                          /p:Configuration=Release                          /p:Platform=$platform /p:DefineConstants=$buildType
    # dotnet build   Source/Pe/Pe.sln                  --verbosity normal --configuration Release --runtime win-$platform  /p:Platform=$platform /p:DefineConstants=$buildType
    # dotnet publish Source/Pe/Pe.Main/Pe.Main.csproj  --verbosity normal --configuration Release --runtime win-$platform  /p:Platform=$platform /p:DefineConstants=$buildType --output Output/Release/$platform/Pe/bin --self-contained true
} finally {
    git reset --hard
    Pop-Location
}
