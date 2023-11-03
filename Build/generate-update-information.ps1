Param(
	[Parameter(mandatory = $true)][string] $Revision,
	[Parameter(mandatory = $true)][version] $MinimumVersion,
	[Parameter(mandatory = $true)][string] $ArchiveBaseUrl,
	[Parameter(mandatory = $true)][string] $NoteBaseUrl,
	[Parameter(mandatory = $true)][string] $OutputDirectory,
	[Parameter(mandatory = $true)][ValidateSet('application', 'plugins')][string] $Module,
	[Parameter(mandatory = $true)][ValidateSet('zip', '7z', 'tar')][string] $Archive,
	[Parameter(mandatory = $true)] $ArtifactDirectory,
	[Parameter(mandatory = $true)][string[]] $Platforms
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'version.ps1',
	'project.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path -Path $currentDirPath -ChildPath $scriptFileName
	. $scriptFilePath
}

$version = GetAppVersion
$hashAlgorithm = "SHA256"
$releaseTimestamp = (Get-Date).ToUniversalTime()

#/*[FUNCTIONS]-------------------------------------

function OutputJson([object] $json, [string] $outputPath) {
	$value = ConvertTo-Json -InputObject $json -Depth 100

	$utf8nEncoding = New-Object System.Text.UTF8Encoding $False
	[System.IO.File]::WriteAllLines($outputPath, $value, $utf8nEncoding)
}

function ReplaceValues([string] $source) {
	$work = $source

	$work = $work.Replace("@VERSION@", $version)
	$work = $work.Replace("@REVISION@", $Revision)

	return $work
}

function CreateUpdateItem([string] $archive, [string] $platform, [string] $archiveFilePath, [uri] $noteUri, [version] $minimumVersion) {
	return @{
		release            = $releaseTimestamp.ToString("s")
		version            = $version
		revision           = $Revision
		platform           = $platform
		minimum_version    = ConvertVersion $minimumVersion '.'
		note_uri           = $noteUri
		archive_uri        = (ReplaceValues $ArchiveBaseUrl).Replace("@ARCHIVENAME@", (Split-Path $archiveFilePath -Leaf))
		archive_size       = (Get-Item -Path $archiveFilePath).Length
		archive_kind       = $archive
		archive_hash_kind  = $hashAlgorithm
		archive_hash_value = (Get-FileHash -Path $archiveFilePath -Algorithm $hashAlgorithm).Hash
	}
}

#*/[FUNCTIONS]-------------------------------------

if ($Module -eq 'application') {
	$updateJson = Get-Content -Path (Join-Path -Path $currentDirPath -ChildPath "update.json") | ConvertFrom-Json
	foreach ($platform in $Platforms) {
		$targetName = 'Pe_'  + $platform + '.' + $Archive
		$targetPath = Join-Path -Path $ArtifactDirectory -ChildPath $targetName

		$targetPath

		$noteName = (ConvertReleaseNoteFileName $version 'html')
		$noteUri = (ReplaceValues $NoteBaseUrl).Replace("@NOTENAME@", $noteName)
		$item = CreateUpdateItem $Archive $platform $targetPath $noteUri $MinimumVersion

		$updateJson.items += $item
	}

	$outputUpdateFile = Join-Path -Path $OutputDirectory -ChildPath 'update.json'
	OutputJson $updateJson $outputUpdateFile

}
elseif ($Module -eq 'plugins') {
	$pluginProjectDirs = GetProjectDirectories 'plugins'

	foreach($pluginProjectDirectory in $pluginProjectDirs) {
		$items = @()
		foreach ($platform in $Platforms) {
			$pluginFileName = $pluginProjectDirectory.Name + '_' + $platform + '.' + $Archive
			$pluginFilePath = Join-Path -Path $ArtifactDirectory -ChildPath $pluginFileName

			$noteName = $pluginProjectDirectory.Name + '.html'
			$noteUri = (ReplaceValues $NoteBaseUrl).Replace("@NOTENAME@", $noteName)
			$item = CreateUpdateItem $Archive $platform $pluginFilePath $noteUri $version

			$items += $item
		}

		if(0 -lt $items.Count) {
			$pluginFiles = @{
				items = $items
			}
			$outputUpdateFile = Join-Path -Path $OutputDirectory -ChildPath ('update-' + $pluginProjectDirectory.Name + '.json')
			OutputJson $pluginFiles $outputUpdateFile
		}
	}
}
else {
	throw "error module: $Module"
}
