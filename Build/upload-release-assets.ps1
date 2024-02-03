Param(
	[Parameter(mandatory = $true)][ValidateSet('github')][string] $TargetRepository,
	[Parameter(mandatory = $true)][string] $Token,
	[Parameter(mandatory = $true)][string] $ReleaseId,
	[Parameter(mandatory = $true)][string] $RepositoryOwner,
	[Parameter(mandatory = $true)][string] $RepositoryName
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$rootDirectory = Get-RootDirectory

#/*[FUNCTIONS]-------------------------------------

function Invoke-GithubAsset {
	param (
		[Parameter(mandatory = $true)][string] $Token,
		[Parameter(mandatory = $true)][string] $ReleaseId,
		[Parameter(mandatory = $true)][string] $RepositoryOwner,
		[Parameter(mandatory = $true)][string] $RepositoryName,
		[Parameter(mandatory = $true)][string] $FilePath,
		[string] $ContentType,
		[string] $FileName
	)
}

function Invoke-Asset {
	param (
		[Parameter(mandatory = $true)][ValidateSet('github')][string] $TargetRepository,
		[Parameter(mandatory = $true)][string] $Token,
		[Parameter(mandatory = $true)][string] $ReleaseId,
		[Parameter(mandatory = $true)][string] $RepositoryOwner,
		[Parameter(mandatory = $true)][string] $RepositoryName,
		[Parameter(mandatory = $true)][string] $FilePath,
		[string] $ContentType,
		[string] $FileName
	)

	switch ($TargetRepository) {
		'github' {
			Invoke-GithubAsset -Token $Token -ReleaseId $ReleaseId -RepositoryOwner $RepositoryOwner -RepositoryName $RepositoryName -FilePath $FilePath -ContentType $ContentType -FileName $FileName
		}

		Default {
			throw $TargetRepository
		}
	}

}

#*/[FUNCTIONS]-------------------------------------

Write-Host "TargetRepository: $TargetRepository"
Write-Host "Token: $Token"
Write-Host "ReleaseId: $ReleaseId"
Write-Host "RepositoryOwner: $RepositoryOwner"
Write-Host "RepositoryName: $RepositoryName"
