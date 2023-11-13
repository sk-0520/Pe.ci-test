Param(
	[Parameter(mandatory = $true)][ValidateSet('github')][string] $TargetRepository
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Version"


#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------


$dot = Get-ApplicationVersion
$hyphen = (ConvertVersion (Get-ApplicationVersion) '-')

if ($TargetRepository -eq 'github') {
	Write-Output "dot=$dot" >> $env:GITHUB_OUTPUT
	Write-Output "hyphen=$hyphen" >> $env:GITHUB_OUTPUT
} else {
	throw "TargetRepository: ${TargetRepository}"
}

