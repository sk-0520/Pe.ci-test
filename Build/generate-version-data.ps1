Param(
	[Parameter(mandatory = $true)][ValidateSet('github')][string] $TargetRepository
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'version.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path -Path $currentDirPath -ChildPath $scriptFileName
	. $scriptFilePath
}

#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------


$dot = GetAppVersion
$hyphen = (ConvertVersion (GetAppVersion) '-')

if ($TargetRepository -eq 'github') {
	Write-Output "dot=$dot" >> $env:GITHUB_OUTPUT
	Write-Output "hyphen=$hyphen" >> $env:GITHUB_OUTPUT
} else {
	throw "TargetRepository: ${TargetRepository}"
}

