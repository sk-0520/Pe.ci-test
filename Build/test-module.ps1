Param(
	[Parameter(mandatory = $true)][ValidateSet('boot', 'main')][string] $Module,
	[Parameter(mandatory = $true)][ValidateSet('x86', 'x64')][string] $Platform,
	[Parameter(mandatory = $true)][string] $Configuration
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
		$testDirPath = Join-Path $projectDirItem.FullName "bin" | Join-Path -ChildPath $Configuration | Join-Path -ChildPath $platform
		$testFileName = $projectDirItem.BaseName + '.dll'
		$testFilePath = Join-Path $testDirPath $testFileName

		VSTest.Console $testFilePath /InIsolation /Platform:$platform
		if (-not $?) {
			throw "test error: $Module"
		}
	}
}
elseif ($Module -eq 'main') {

}
else {
	throw 'うわわわわ'
}
