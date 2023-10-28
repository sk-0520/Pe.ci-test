Param(
	[Parameter(mandatory = $true)][ValidateSet('boot', 'main', 'plugins')][string] $Module,
	[Parameter(mandatory = $true)][ValidateSet('x86', 'x64')][string] $Platform,
	[Parameter(mandatory = $true)][string] $Configuration,
	[string] $MainLogger
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootDirPath = Split-Path -Parent $currentDirPath

$sourceMainDirectoryPath = Join-Path $rootDirPath "Source/Pe"
$sourceBootDirectoryPath = Join-Path $rootDirPath "Source/Pe.Boot"

if ($Module -eq 'boot') {
	$bootProjectDirItems = Get-ChildItem -Path $sourceBootDirectoryPath -Filter "*.Test" -Directory
	foreach ($projectDirItem in $bootProjectDirItems) {
		$testDirPath = Join-Path $projectDirItem.FullName "bin" | Join-Path -ChildPath $Configuration | Join-Path -ChildPath $Platform
		$testFileName = $projectDirItem.BaseName + '.dll'
		$testFilePath = Join-Path $testDirPath $testFileName

		VSTest.Console $testFilePath /InIsolation /Platform:$Platform
		if (-not $?) {
			throw "test error: $Module"
		}
	}
}
elseif ($Module -eq 'main') {
	$mainLoggerArg = ''
	if (![string]::IsNullOrEmpty($MainLogger)) {
		$mainLoggerArg = "--logger:$MainLogger"
	}
	$projectDirItems = Get-ChildItem -Path $sourceMainDirectoryPath -Filter "*.Test" -Directory -Exclude 'Pe.Plugins.Reference.*'

	foreach ($projectDirItem in $projectDirItems) {
		$testDirPath = Join-Path $projectDirItem.FullName "bin" | Join-Path -ChildPath $Platform | Join-Path -ChildPath $Configuration
		$testFileName = $projectDirItem.BaseName + '.dll'
		$testFilePath = Join-Path $testDirPath (Get-ChildItem -LiteralPath $testDirPath -Recurse -Name -File -Include $testFileName)

		dotnet test $testFilePath --test-adapter-path:. $mainLoggerArg
		if (-not $?) {
			throw "test error: $Module - $testFileName"
		}
	}
}
elseif ($Module -eq 'plugins') {
	$mainLoggerArg = ''
	if (![string]::IsNullOrEmpty($MainLogger)) {
		$mainLoggerArg = "--logger:$MainLogger"
	}
	$projectDirItems = Get-ChildItem -Path $sourceMainDirectoryPath -Filter "*.Test" -Directory -Include 'Pe.Plugins.Reference.*'

	foreach ($projectDirItem in $projectDirItems) {
		$testDirPath = Join-Path $projectDirItem.FullName "bin" | Join-Path -ChildPath $Platform | Join-Path -ChildPath $Configuration
		$testFileName = $projectDirItem.BaseName + '.dll'
		$testFilePath = Join-Path $testDirPath (Get-ChildItem -LiteralPath $testDirPath -Recurse -Name -File -Include $testFileName)

		dotnet test $testFilePath --test-adapter-path:. $mainLoggerArg
		if (-not $?) {
			throw "test error: $Module - $testFileName"
		}
	}
}
else {
	throw 'うわわわわ'
}
