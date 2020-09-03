Param(
	[switch] $NoInstall,
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

try {
	Push-Location $documentDirectoryPath
	if (! $NoInstall) {
		Write-Output install
		npm install --loglevel=error
	}
	Write-Output build
	npm run build

	foreach ($platform in $Platforms) {
		$outputDirectoryPath = Join-Path $rootDirectoryPath "Output\Release\$platform\Pe\doc\help"
		robocopy /MIR /PURGE /R:3 /S "$buildOutputDirectoryPath" "$outputDirectoryPath"
	}
}
finally {
	Pop-Location
}
