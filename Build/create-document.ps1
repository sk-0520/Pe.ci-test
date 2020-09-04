Param(
	[switch] $NoInstall
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

	# ビルド工程にドキュメントをのせる
	$outputDirectoryPath = Join-Path $rootDirectoryPath "Source\Pe\Pe.Main\doc\help"
	robocopy /MIR /PURGE /R:3 /S "$buildOutputDirectoryPath" "$outputDirectoryPath"
}
finally {
	Pop-Location
}
