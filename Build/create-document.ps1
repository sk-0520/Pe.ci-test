Param(
	[parameter(mandatory = $true)][string] $Platform
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
$outputDirectoryPath = Join-Path $rootDirectoryPath "Output\Release\$Platform\Pe\doc\help"

try{
	Push-Location $documentDirectoryPath
	Write-Output install
	npm install --loglevel=error
	Write-Output build
	npm run build

	robocopy /MIR /PURGE /R:3 /S "$buildOutputDirectoryPath" "$outputDirectoryPath"

} finally {
	Pop-Location
}
