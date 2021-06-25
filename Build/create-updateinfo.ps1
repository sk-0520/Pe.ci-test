Param(
	[Parameter(mandatory = $true)][ValidateSet("bitbucket")][string] $TargetRepository,
	[Parameter(mandatory = $true)][version] $MinimumVersion,
	[Parameter(mandatory = $true)][string] $ArchiveBaseUrl,
	[Parameter(mandatory = $true)][string] $NoteBaseUrl,
	[Parameter(mandatory = $true)][string] $ReleaseDirectory,
	[Parameter(mandatory = $true)][ValidateSet("zip", "7z")][string] $Archive,
	[Parameter(mandatory = $true)][string[]] $Platforms
)
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

$version = GetAppVersion
$hashAlgorithm = "SHA256"
$releaseTimestamp = (Get-Date).ToUniversalTime()
$revision = (git rev-parse HEAD)

# アップデート情報の作成
$updateJson = Get-Content -Path (Join-Path $currentDirPath "update.json") | ConvertFrom-Json
foreach ($platform in $Platforms) {
	$targetName = ConvertAppArchiveFileName $version $platform $Archive
	$targetPath = Join-Path $ReleaseDirectory $targetName

	$item = @{
		release            = $releaseTimestamp.ToString("s")
		version            = $version
		revision           = $revision
		platform           = $platform
		minimum_version    = $MinimumVersion
		note_uri           = $NoteBaseUrl.Replace("@NOTENAME@", (ConvertReleaseNoteFileName $version))
		archive_uri        = $ArchiveBaseUrl.Replace("@ARCHIVEAME@", $targetName)
		archive_size       = (Get-Item -Path $targetPath).Length
		archive_kind       = $Archive
		archive_hash_kind  = $hashAlgorithm
		archive_hash_value = (Get-FileHash -Path $targetPath -Algorithm $hashAlgorithm).Hash
	}

	$updateJson.items += $item
}
$outputUpdateFile = Join-Path $outputDirectory 'update.json'
ConvertTo-Json -InputObject $updateJson `
| ForEach-Object { [Text.Encoding]::UTF8.GetBytes($_) } `
| Set-Content -Path $outputUpdateFile -Encoding Byte
Get-Content $outputUpdateFile

switch ($TargetRepository) {
	'bitbucket' {
		$tagJson = @{
			name   = $version
			target = @{
				hash = $revision
			}
		}
		$bitbucketTagApiFile = Join-Path $outputDirectory "bitbucket-tag.json"
		ConvertTo-Json -InputObject $tagJson `
		| ForEach-Object { [Text.Encoding]::UTF8.GetBytes($_) } `
		| Set-Content -Path $bitbucketTagApiFile -Encoding Byte
		Get-Content $bitbucketTagApiFile
	}
}
