Param(
	[Parameter(mandatory = $true)][ValidateSet('github')][string] $TargetRepository
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Version"


#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------


$version = Get-ApplicationVersion
$dot = Convert-Version -Version $version -Separator '.'
$hyphen = Convert-Version -Version $version -Separator '-'

if ($TargetRepository -eq 'github') {
	Write-Output "dot=$dot" >> $env:GITHUB_OUTPUT
	Write-Output "hyphen=$hyphen" >> $env:GITHUB_OUTPUT
} else {
	throw "TargetRepository: ${TargetRepository}"
}

