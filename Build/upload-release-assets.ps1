Param(
	[Parameter(mandatory = $true)][ValidateSet('github')][string] $TargetRepository,
	[Parameter(mandatory = $true)][string] $Token,
	[Parameter(mandatory = $true)][string] $ReleaseId,
	[Parameter(mandatory = $true)][string] $RepositoryOwner,
	[Parameter(mandatory = $true)][string] $RepositoryName
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Project"


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

	Write-Debug "FilePath: $FilePath"

	$headers = @{
		'Accept' = 'application/vnd.github+json'
		'Authorization' = "Bearer $Token"
		'X-GitHub-Api-Version' = '2022-11-28'
	}
	$contentType = 'application/octet-stream'

	$name = $FileName
	if ([string]::IsNullOrEmpty($name)) {
		$name = Split-Path -Path $FilePath -Leaf
	}

	$uri = [uri]"https://uploads.github.com/repos/${RepositoryOwner}/${RepositoryName}/releases/${ReleaseId}/assets?name=${name}"
	Write-Debug "uri: $uri"

	Invoke-WebRequest `
		-Uri $uri `
		-Method Post `
		-Headers $headers `
		-ContentType $contentType `
		-InFile $FilePath
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
			Invoke-GithubAsset `
				-Token $Token `
				-ReleaseId $ReleaseId `
				-RepositoryOwner $RepositoryOwner `
				-RepositoryName $RepositoryName `
				-FilePath $FilePath `
				-ContentType $ContentType `
				-FileName $FileName
		}

		Default {
			throw $TargetRepository
		}
	}

}

#*/[FUNCTIONS]-------------------------------------

Write-Verbose "TargetRepository: $TargetRepository"
Write-Verbose "Token: $Token"
Write-Verbose "ReleaseId: $ReleaseId"
Write-Verbose "RepositoryOwner: $RepositoryOwner"
Write-Verbose "RepositoryName: $RepositoryName"

$assetItems = @(
	@{
		path = 'artifacts/Pe.Plugins.Reference/Pe.Plugins.Reference.*.*'
		literalPath = $false
	},
	@{
		path = 'artifacts/Pe/Pe_*'
		literalPath = $false
	},
	@{
		path = 'artifacts/history/*.html'
		literalPath = $false
	},
	@{
		path = 'artifacts/info/plugins/update-*.json'
		literalPath = $false
	},
	@{
		path = 'artifacts/info/pe/Output/update.json'
		literalPath = $true
	}
)

foreach ($assetItem in $assetItems) {
	if ($assetItem.literalPath) {
		Invoke-Asset `
			-TargetRepository $TargetRepository `
			-Token $Token `
			-ReleaseId $ReleaseId `
			-RepositoryOwner $RepositoryOwner `
			-RepositoryName $RepositoryName `
			-FilePath (Join-Path -Path $rootDirectory -ChildPath $assetItem.path)
	} else {
		$files = Get-ChildItem -Path (Join-Path -Path $rootDirectory -ChildPath $assetItem.path)
		foreach ($file in $files) {
			Invoke-Asset `
				-TargetRepository $TargetRepository `
				-Token $Token `
				-ReleaseId $ReleaseId `
				-RepositoryOwner $RepositoryOwner `
				-RepositoryName $RepositoryName `
				-FilePath $file.FullName
		}
	}
}

