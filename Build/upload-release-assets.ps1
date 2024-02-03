Param(
	[Parameter(mandatory = $true)][ValidateSet('github')][string] $TargetRepository,
	[Parameter(mandatory = $true)][string] $Token,
	[Parameter(mandatory = $true)][string] $ReleaseId,
	[Parameter(mandatory = $true)][string] $RepositoryOwner,
	[Parameter(mandatory = $true)][string] $RepositoryName
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------

Write-Host "TargetRepository: $TargetRepository"
Write-Host "Token: $Token"
Write-Host "ReleaseId: $ReleaseId"
Write-Host "RepositoryOwner: $RepositoryOwner"
Write-Host "RepositoryName: $RepositoryName"
