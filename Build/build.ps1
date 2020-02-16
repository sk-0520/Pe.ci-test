Param(
    [switch] $ProductMode,
    [parameter(mandatory=$true)][string] $Platform,
    [string] $BuildType
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

SetCommand 'git' 'BUILD_GIT_PATH' "%PROGRAMFILES%\git\bin"
SetCommand 'msbuild' 'BUILD_MSBUILD_PATH' "%PROGRAMFILES(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin"
SetCommand 'dotnet' 'BUILD_DOTNET_PATH' "%PROGRAMFILES(x86)%\dotnet\"

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

    $version = GetAppVersion
    $revision = (git rev-parse HEAD)

    function UpdateElement([string] $value, [xml] $xml, [string] $targetXpath, [string] $parentXpath, [string] $elementName) {
        $element = $xml.SelectSingleNode($targetXpath);
        if($null -eq $element) {
            $propGroup = $xml.SelectSingleNode($parentXpath)
            $element = $xml.CreateElement($elementName);
            $propGroup.AppendChild($element);
        }
        $element.InnerText = $value
    }

    function ReplaceElement([hashtable] $map, [xml] $xml, [string] $targetXpath, [string] $parentXpath, [string] $elementName) {
        $element = $xml.SelectSingleNode($targetXpath);
        if($null -ne $element) {
            $val = $element.InnerText
            foreach($key in $map.keys) {
                $val = $val.Replace($key, $map[$key])
            }
            $element.InnerText = $val
        }
    }

    $projectFiles = (Get-ChildItem -Path "Source\Pe\" -Recurse -Include *.csproj)
    foreach($projectFile in $projectFiles) {
        Write-Output $projectFile.Name
        $xml = [XML](Get-Content $projectFile  -Encoding UTF8)

        UpdateElement $version $xml '/Project/PropertyGroup[1]/Version[1]' '/Project/PropertyGroup[1]' 'Version'
        UpdateElement $revision $xml '/Project/PropertyGroup[1]/InformationalVersion[1]' '/Project/PropertyGroup[1]' 'InformationalVersion'
        ReplaceElement @{ 'YYYY' = '2020' } $xml '/Project/PropertyGroup[1]/Copyright[1]' '/Project/PropertyGroup[1]' 'Copyright'

        $xml.Save($projectFile)
    }

    # ビルド開始
    $productSwitch = if ( $ProductMode ) { '/p:DefineConstants=PRODUCT' } else { '' }

    msbuild        Source/Pe.Boot/Pe.Boot.sln                          /p:Configuration=Release                          /p:Platform=$Platform /p:DefineConstants=$BuildType $productSwitch
    dotnet build   Source/Pe/Pe.sln                  --verbosity normal --configuration Release --runtime win-$Platform  /p:Platform=$Platform /p:DefineConstants=$BuildType $productSwitch
    dotnet publish Source/Pe/Pe.Main/Pe.Main.csproj  --verbosity normal --configuration Release --runtime win-$Platform  /p:Platform=$Platform /p:DefineConstants=$BuildType $productSwitch --output Output/Release/$Platform/Pe/bin --self-contained true

    if($ProductMode) {
        $productTargets = @('etc', 'doc', 'bat')

        # 本番用データ配置のため不要ファイル破棄
        foreach($productTarget in $productTargets) {
            $target = Join-Path "Output/Release/$Platform/Pe/bin" $productTarget
            Remove-Item -Path $target -Recurse -Force
        }

        # 本番用データ配置のため必要ファイルの移送
        foreach($productTarget in $productTargets) {
            $src = Join-Path "Source/Pe/Pe.Main"           $productTarget
            $dst = Join-Path "Output/Release/$Platform/Pe" $productTarget
            robocopy /MIR /PURGE /R:3 /S "$src" "$dst"
        }
    }
} finally {
    git reset --hard
    Pop-Location
}
