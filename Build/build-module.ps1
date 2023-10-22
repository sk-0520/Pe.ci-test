Param(
	[Parameter(mandatory = $true)][ValidateSet('boot', 'main')][string] $Module,
	[switch] $ProductMode,
	[string] $BuildType,
	[Parameter(mandatory = $true)][ValidateSet('x86', 'x64')][string] $Platform,
	[switch] $Test
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootDirPath = Split-Path -Parent $currentDirPath

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
	if($Test) {
		$configuration = 'CI_TEST'
	}
	msbuild Source/Pe.Boot/Pe.Boot.sln /m /p:Configuration=$configuration /p:Platform=$Platform /p:DefineConstants=$define
	if (-not $?) {
		throw "build error: $Module"
	}
}
elseif ($Module -eq 'main') {
	dotnet publish Source/Pe/Pe.Main/Pe.Main.csproj /m --verbosity normal --configuration Release /p:Platform=$Platform /p:DefineConstants=$define --runtime win10-$Platform --output Output/Release/$Platform/Pe/bin --self-contained true
	if (-not $?) {
		throw "build error: $Module"
	}
}
else {
	throw 'うわわわわ'
}
#$projectFiles = (Get-ChildItem -Path "Source\Pe\" -Recurse -Include *.csproj)


