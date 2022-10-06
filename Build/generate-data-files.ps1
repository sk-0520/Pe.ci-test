$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'command.ps1',
	'version.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path $currentDirPath $scriptFileName
	. $scriptFilePath
}
$rootDirPath = Split-Path -Parent $currentDirPath
$outputDirectory = Join-Path $rootDirPath 'Output'

$utf8nEncoding = New-Object System.Text.UTF8Encoding $False

[System.IO.File]::WriteAllLines((Join-Path $outputDirectory 'gen-version-dot.txt'), (GetAppVersion), $utf8nEncoding)
[System.IO.File]::WriteAllLines((Join-Path $outputDirectory 'gen-version-hyphen.txt'), (ConvertVersion (GetAppVersion) '-'), $utf8nEncoding)
