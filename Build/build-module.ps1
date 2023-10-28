Param(
	[Parameter(mandatory = $true)][ValidateSet('boot', 'main', 'plugins')][string] $Module,
	[switch] $ProductMode,
	[string] $BuildType,
	[Parameter(mandatory = $true)][ValidateSet('x86', 'x64')][string] $Platform,
	[switch] $Test
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'project.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path $currentDirPath $scriptFileName
	. $scriptFilePath
}

#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------

# ビルド開始
$defines = @()
if ( $BuildType ) {
	$defines += $BuildType
}
if ( $ProductMode ) {
	$defines += 'PRODUCT'
}
# ; を扱う https://docs.microsoft.com/ja-jp/visualstudio/msbuild/msbuild-special-characters?view=vs-2015&redirectedfrom=MSDN
$define = $defines -join '%3B'

if ($Module -eq 'boot') {
	$configuration = 'Release'
	if ($Test) {
		$configuration = 'CI_TEST'
	}
	msbuild (Join-Path -Path (GetSourceDirectory 'boot') -ChildPath 'Pe.Boot.sln') /m /p:Configuration=$configuration /p:Platform=$Platform /p:DefineConstants=$define
	if (-not $?) {
		throw "build error: $Module"
	}
}
elseif ($Module -eq 'main') {
	if ($Test) {
		$testDirectories = GetTestProjectDirectories $Module

		foreach ($testDirectory in $testDirectories) {
			$testProjectFilePath = (Join-Path $testDirectory.FullName $testDirectory.Name) + ".csproj"
			dotnet build $testProjectFilePath /m --verbosity normal --configuration Release /p:Platform=$Platform /p:DefineConstants=$define --runtime win10-$Platform --no-self-contained
			if (-not $?) {
				throw "build error: $Module"
			}
		}
	}
	else {
		dotnet publish (Join-Path -Path (GetSourceDirectory $Module) -ChildPath 'Pe.Main/Pe.Main.csproj') /m --verbosity normal --configuration Release /p:Platform=$Platform /p:DefineConstants=$define --runtime win10-$Platform --output Output/Release/$Platform/Pe/bin --self-contained true
		if (-not $?) {
			throw "build error: $Module"
		}

	}
}
elseif ($Module -eq 'plugins') {
	# プラグイン参考実装
	$pluginProjectFiles = GetApplicationProjectDirectories $Module `
	| Get-ChildItem -File -Recurse -Include *.csproj

	foreach ($pluginProjectFile in $pluginProjectFiles) {
		$name = $pluginProjectFile.BaseName

		dotnet publish $pluginProjectFile /m --verbosity normal --configuration Release /p:Platform=$Platform /p:DefineConstants=$define --runtime win10-$Platform --output Output/Release/$Platform/Plugins/$name --self-contained false
		if (-not $?) {
			throw "build error: $Module - $name"
		}
	}
}
else {
	throw 'うわわわわ'
}
#$projectFiles = (Get-ChildItem -Path "Source\Pe\" -Recurse -Include *.csproj)


