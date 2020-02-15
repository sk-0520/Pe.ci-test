Param(
    [parameter(mandatory = $true)][ValidateSet("bitbucket")][string] $TargetRepository,
    [parameter(mandatory = $true)][version] $MinimumVersion,
    [parameter(mandatory = $true)][string] $ArchiveBaseUrl,
    [parameter(mandatory = $true)][string] $NoteBaseUrl,
    [parameter(mandatory = $true)][string] $OutputDirectory,
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

SetCommand 'git' 'BUILD_GIT_PATH' "%PROGRAMFILES%\git\bin"

$version = GetAppVersion
$hashAlgorithm = "SHA256"
$releaseTimestamp = (Get-Date).ToUniversalTime()
$revision = (git rev-parse HEAD)

# リリースノートの作成

# アップデート情報の作成
$updateJson = Get-Content -Path (Join-Path $currentDirPath "update.json") | ConvertFrom-Json
foreach ($platform in $Platforms) {
    $targetPath = Join-Path $ReleaseDirectory ("Pe_" + $version + "_" + $platform + ".zip")

    $item = @{
        release            = $releaseTimestamp.ToString("s")
        version            = (GetAppVersion)
        revision           = $revision
        platform           = $platform
        minimum_version    = $MinimumVersion
        note_uri           = $NoteBaseUrl.Replace("@NOTENAME@", "Pe_${vesion}.html")
        archive_uri        = $ArchiveBaseUrl.Replace("@ARCHIVEAME@", "Pe_${vesion}_${platform}.zip")
        archive_size       = (Get-Item -Path $targetPath).Length
        archive_hash_kind  = $hashAlgorithm
        archive_hash_value = (Get-FileHash -Path $targetPath -Algorithm $hashAlgorithm).Hash
    }

    $updateJson.items += $item
}

$outputUpdateFile = Join-Path $OutputDirectory 'update.json'
ConvertTo-Json -InputObject $updateJson | Out-File $outputUpdateFile -Encoding utf8 -Force
Get-Content $outputUpdateFile

switch ($TargetRepository) {
    'bitbucket' {
        $tagJson = @{
            name   = $version
            target = @{
                hash = $revision
            }
        }
        $bitbucketTagApiFile = Join-Path $OutputDirectory 'bitbucket-tag.json'
        $utf8n = New-Object 'System.Text.UTF8Encoding' -ArgumentList @($false)
        [System.IO.File]::WriteAllLines($bitbucketTagApiFile, @((ConvertTo-Json -InputObject $tagJson)), $utf8n)
        Get-Content $bitbucketTagApiFile
    }
}
