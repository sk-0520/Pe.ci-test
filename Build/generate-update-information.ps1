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

Import-Module "${PSScriptRoot}/Modules/Project"


$version = Get-ApplicationVersion
$hashAlgorithm = 'SHA256'
$releaseTimestamp = (Get-Date).ToUniversalTime()

#/*[FUNCTIONS]-------------------------------------

function New-Json {
	[CmdletBinding(SupportsShouldProcess)]
	Param(
		[Parameter(mandatory = $true)][object] $InputObject,
		[Parameter(mandatory = $true)][string] $OutputFilePath
	)

	$json = ConvertTo-Json -InputObject $InputObject -Depth 100

	$utf8nEncoding = New-Object System.Text.UTF8Encoding $False

	if ($PSCmdlet.ShouldProcess('OutputFilePath', "$OutputFilePath の出力")) {
		[System.IO.File]::WriteAllLines($OutputFilePath, $json, $utf8nEncoding)
	} else {
		Write-Verbose "`[DRY`] OutputFilePath: $OutputFilePath -> $json"
	}
}

function Convert-Template {
	[OutputType([string])]
	Param(
		[Parameter(mandatory = $true)][string] $Source
	)

	$work = $Source

	$work = $work.Replace('@VERSION@', $version)
	$work = $work.Replace('@REVISION@', $Revision)

	return $work
}

function Get-UpdateItem {
	Param(
		[Parameter(mandatory = $true)][ValidateSet('x86', 'x64')][string] $Platform,
		[Parameter(mandatory = $true)][string] $ArchiveFilePath,
		[Parameter(mandatory = $true)][uri] $NoteUri
	)

	return @{
		release = $releaseTimestamp.ToString('s')
		version = $version
		revision = $Revision
		platform = $Platform
		minimum_version = ConvertVersion $MinimumVersion '.'
		note_uri = $NoteUri
		archive_uri = (Convert-Template -Source $ArchiveBaseUrl).Replace('@ARCHIVENAME@', (Split-Path $ArchiveFilePath -Leaf))
		archive_size = (Get-Item -Path $ArchiveFilePath).Length
		archive_kind = $Archive
		archive_hash_kind = $hashAlgorithm
		archive_hash_value = (Get-FileHash -Path $ArchiveFilePath -Algorithm $hashAlgorithm).Hash
	}
}

#*/[FUNCTIONS]-------------------------------------

if ($Module -eq 'application') {
	$updateJson = Get-Content -Path (Join-Path -Path $currentDirPath -ChildPath 'update.json') | ConvertFrom-Json
	foreach ($platform in $Platforms) {
		$targetName = 'Pe_' + $platform + '.' + $Archive
		$targetPath = Join-Path -Path $ArtifactDirectory -ChildPath $targetName

		$noteName = 'Pe.html'
		$noteUri = (Convert-Template -Source $NoteBaseUrl).Replace('@NOTENAME@', $noteName)
		$item = Get-UpdateItem -Platform $platform -ArchiveFilePath $targetPath -NoteUri $noteUri

		$updateJson.items += $item
	}

	$outputUpdateFile = Join-Path -Path $OutputDirectory -ChildPath 'update.json'
	New-Json -InputObject $updateJson -OutputFilePath $outputUpdateFile

} elseif ($Module -eq 'plugins') {
	$pluginProjectDirs = Get-ProjectDirectories -Kind 'plugins'

	foreach ($pluginProjectDirectory in $pluginProjectDirs) {
		$items = @()
		foreach ($platform in $Platforms) {
			$pluginFileName = $pluginProjectDirectory.Name + '_' + $platform + '.' + $Archive
			$pluginFilePath = Join-Path -Path $ArtifactDirectory -ChildPath $pluginFileName

			$noteName = $pluginProjectDirectory.Name + '.html'
			$noteUri = (Convert-Template -Source $NoteBaseUrl).Replace('@NOTENAME@', $noteName)
			$item = Get-UpdateItem -Platform $platform -ArchiveFilePath $pluginFilePath -NoteUri $noteUri

			$items += $item
		}

		if (0 -lt $items.Count) {
			$pluginFiles = @{
				items = $items
			}
			$outputUpdateFile = Join-Path -Path $OutputDirectory -ChildPath ('update-' + $pluginProjectDirectory.Name + '.json')
			New-Json -InputObject $pluginFiles -OutputFilePath $outputUpdateFile
		}
	}
} else {
	throw "error module: $Module"
}
