$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootDirPath = Split-Path -Parent $currentDirPath

$platforms = @(
	'x86'
	'x64'
)

$outputDir = Join-Path $rootDirPath 'abc'
New-Item -Path $outputDir -ItemType Directory

foreach ($platform in $platforms) {
	Move-Item -Path (Join-Path -Path 'artifacts' -ChildPath "application-${platform}") -Destination (Join-Path -Path $outputDir -ChildPath "application-$platform")

	foreach ($pluginDir in Get-ChildItem -Path (Join-Path -Path 'artifacts' -ChildPath "plugins-${platform}") -Directory) {
		Move-Item -Path $pluginDir -Destination (Join-Path -Path $outputDir -ChildPath "plugins-$($pluginDir.Name)-${platform}")
	}
}






