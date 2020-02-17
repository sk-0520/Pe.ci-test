Param(
	[parameter(mandatory = $true)][ValidateSet("bitbucket")][string] $TargetRepository,
	[parameter(mandatory = $true)][version] $MinimumVersion,
	[parameter(mandatory = $true)][string] $ArchiveBaseUrl,
	[parameter(mandatory = $true)][string] $NoteBaseUrl,
	[parameter(mandatory = $true)][string] $ReleaseDirectory,
	[parameter(mandatory = $true)][string[]] $Platforms
)
$ErrorActionPreference = 'Stop'
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

SetCommand 'git' 'BUILD_GIT_PATH' "%PROGRAMFILES%\git\bin"

$version = GetAppVersion
$hashAlgorithm = "SHA256"
$releaseTimestamp = (Get-Date).ToUniversalTime()
$revision = (git rev-parse HEAD)

# アップデート情報の作成
$updateJson = Get-Content -Path (Join-Path $currentDirPath "update.json") | ConvertFrom-Json
foreach ($platform in $Platforms) {
	$targetPath = Join-Path $ReleaseDirectory (ConvertAppArchiveFileName $version $platform)

	$item = @{
		release            = $releaseTimestamp.ToString("s")
		version            = (GetAppVersion)
		revision           = $revision
		platform           = $platform
		minimum_version    = $MinimumVersion
		note_uri           = $NoteBaseUrl.Replace("@NOTENAME@", "Pe_${version}.html")
		archive_uri        = $ArchiveBaseUrl.Replace("@ARCHIVEAME@", "Pe_${version}_${platform}.zip")
		archive_size       = (Get-Item -Path $targetPath).Length
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
			name   = $version.ToString(3)
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
