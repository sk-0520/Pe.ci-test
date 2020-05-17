Param(
	[parameter(mandatory = $true)][string[]] $Platforms
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'command.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path $currentDirPath $scriptFileName
	. $scriptFilePath
}
$rootDirectoryPath = Split-Path -Parent $currentDirPath

$documentDirectoryPath = Join-Path $rootDirectoryPath 'Source\Documents'
$buildOutputDirectoryPath = Join-Path $documentDirectoryPath 'build'

$documentScriptDirectoryPath = Join-Path $documentDirectoryPath 'source\script'
$changelogCurrentFilePath = Join-Path $documentScriptDirectoryPath 'changelogs.ts'
$changelogArchiveFilePath = Join-Path $documentScriptDirectoryPath 'changelogs-archive.json'

$changelogCurrentContent = Get-Content -Path $changelogCurrentFilePath -Raw -Encoding UTF8
$changelogArchiveContent = Get-Content -Path $changelogArchiveFilePath -Raw -Encoding UTF8

$embeddedMark = '/*--------BUILD-EMBEDDED-JSON--------*/'

$embeddedScript = "changelogs.push($changelogArchiveContent);"

$changelogNewContent = $changelogCurrentContent.Replace($embeddedMark, $embeddedScript)
Set-Content $changelogCurrentFilePath -Value $changelogNewContent -Encoding UTF8

try{
	Push-Location $documentDirectoryPath
	Write-Output install
	npm install --loglevel=error
	Write-Output build
	npm run build

	foreach($platform in $Platforms) {
		$outputDirectoryPath = Join-Path $rootDirectoryPath "Output\Release\$platform\Pe\doc\help"
		robocopy /MIR /PURGE /R:3 /S "$buildOutputDirectoryPath" "$outputDirectoryPath"
	}
} finally {
	Pop-Location
}
