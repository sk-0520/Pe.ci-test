Param()

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path

$pluginName = 'TEMPLATE_PluginName'
$pluginShortName = 'TEMPLATE_PluginShortName'
$pluginId = 'TEMPLATE_PluginId'
$repositoryOwner = '<RepositoryOwner>'

$minimumVersion = [version]"0.0.0"
$archiveBaseUrl = "https://github.com/${repositoryOwner}/${pluginName}/releases/download/@VERSION@/@ARCHIVENAME@"
$releaseNoteUrl = "https://github.com/${repositoryOwner}/${pluginName}/releases/download/@VERSION@/update-${pluginName}.json"
$archive = 'zip'

$scriptDirPath = Join-Path -Path $currentDirPath -ChildPath 'Build'
$sourceDirPath = Join-Path -Path $currentDirPath -ChildPath 'Source'
$propsFilePath = Join-Path -Path $sourceDirPath -ChildPath 'Directory.Build.props'
$outputRootDirPath = Join-Path -Path $currentDirPath -ChildPath 'Output'

function Get-OutputDirectoryPath {
	param (
		[Parameter(mandatory = $true)][ValidateSet("x86", "x64")][string] $Platform
	)
	return Join-Path -Path $outputRootDirPath -ChildPath "Release/$platform/$pluginName"
}

$cliVesion = [version]([XML](Get-Content -Path $propsFilePath -Encoding UTF8)).CreateNavigator().Select('/Project/PropertyGroup/Version').Value
$vesion = @(
	"{0}"     -f $cliVesion.Major
	"{0:00}"  -f $cliVesion.Minor
	"{0:000}" -f $cliVesion.Build
) -join '-'

$platforms = @('x64', 'x86')

$scripts = @{
	buildProject = Join-Path -Path $scriptDirPath -ChildPath 'build-project.ps1'
	archivePlugin = Join-Path -Path $scriptDirPath -ChildPath 'archive-plugin.ps1'
	createInfo = Join-Path -Path $scriptDirPath -ChildPath 'create-info.ps1'
}

Write-Host 'プロジェクトビルド'
& $scripts.buildProject -ProjectName $pluginName -Platforms $platforms

Write-Host 'アーカイブ'
foreach($platform in $platforms) {
	$outputBaseName = "${pluginName}_${vesion}_${platform}"
	& $scripts.archivePlugin -InputDirectory (Get-OutputDirectoryPath $platform) -DestinationDirectory $outputRootDirPath -OutputBaseName $outputBaseName -Archive $archive -Filter '*.pdb'
}

Write-Host 'リリース情報生成'
$archiveBaseName = "${pluginName}_${vesion}"
& $scripts.createInfo -ProjectName $pluginName -Version $cliVesion -ReleaseNoteUrl $releaseNoteUrl -ArchiveBaseUrl $archiveBaseUrl -ArchiveBaseName $archiveBaseName -Archive $archive -InputDirectory $outputRootDirPath -Destination (Join-Path -Path $outputRootDirPath -ChildPath "update-$pluginName.json") -MinimumVersion $minimumVersion -Platforms $platforms
