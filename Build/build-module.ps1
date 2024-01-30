Param(
	[Parameter(mandatory = $true)][ValidateSet('boot', 'main', 'plugins')][string] $Module,
	[switch] $ProductMode,
	[string] $BuildType,
	[Parameter(mandatory = $true)][ValidateSet('x86', 'x64')][string] $Platform,
	[switch] $Test
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Project"


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

Write-Information "define: $define"

if ($Module -eq 'boot') {
	$configuration = 'Release'
	if ($Test) {
		$configuration = 'CI_TEST'
	}
	msbuild (Join-Path -Path (Get-SourceDirectory -Kind 'boot') -ChildPath 'Pe.Boot.sln') /m /p:Configuration=$configuration /p:Platform=$Platform /p:DefineConstants=$define
	if (-not $?) {
		throw "build error: $Module"
	}
} elseif ($Module -eq 'main') {
	dotnet publish (Join-Path -Path (Get-SourceDirectory -Kind $Module) -ChildPath 'Pe.Main/Pe.Main.csproj') /m --verbosity normal --configuration Release /p:Platform=$Platform /p:DefineConstants=$define --runtime win-$Platform --output Output/Release/$Platform/Pe/bin --self-contained true
	if (-not $?) {
		throw "build error: $Module"
	}
} elseif ($Module -eq 'plugins') {
	# プラグイン参考実装
	$pluginProjectFiles = Get-ApplicationProjectDirectories -Kind $Module |
		Get-ChildItem -File -Recurse -Include '*.csproj'

	$buildCount = 0;

	foreach ($pluginProjectFile in $pluginProjectFiles) {
		$name = $pluginProjectFile.BaseName

		dotnet publish $pluginProjectFile /m --verbosity normal --configuration Release /p:Platform=$Platform /p:DefineConstants=$define --runtime win-$Platform --output Output/Release/$Platform/Plugins/$name --self-contained false
		if (-not $?) {
			throw "build error: $Module - $name"
		}
		$buildCount += 1
	}

	if ($buildCount -eq 0) {
		throw "build error: $Module - 0 build - " + (Get-ProjectDirectories -Kind $Module)
	}
} else {
	throw "module error: $Module"
}
#$projectFiles = (Get-ChildItem -Path "Source\Pe\" -Recurse -Include *.csproj)


