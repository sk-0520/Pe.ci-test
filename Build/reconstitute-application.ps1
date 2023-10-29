Param(
	[Parameter(mandatory = $true)][ValidateSet('x86', 'x64')][string] $Platform,
	[switch] $ProductMode,
	[Parameter(mandatory = $true)][string] $InputDirectory,
	[Parameter(mandatory = $true)][string] $OutputDirectory
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'project.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path $currentDirPath $scriptFileName
	. $scriptFilePath
}

#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------

Write-Output "HELLO"
