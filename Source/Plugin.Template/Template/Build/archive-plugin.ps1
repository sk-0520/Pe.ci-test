Param(
	[Parameter(mandatory = $true)][string] $ProjectName,
	[Parameter(mandatory = $true)][string] $Version,
	[Parameter(mandatory = $true)][string[]] $Platforms
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
