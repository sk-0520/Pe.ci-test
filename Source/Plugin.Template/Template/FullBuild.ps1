Param()

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path

$pluginName = 'TEMPLATE_PluginName'
$pluginShortName = 'TEMPLATE_PluginShortName'
$pluginId = 'TEMPLATE_PluginId'

$scriptDirPath = Join-Path -Path $currentDirPath -ChildPath 'Build'
$sourceDirPath = Join-Path -Path $currentDirPath -ChildPath 'Source'
$projectDirPath =  Join-Path -Path $sourceDirPath -ChildPath $pluginName | Join-Path -ChildPath "$pluginName.csproj"
$propsFilePath = Join-Path -Path $sourceDirPath -ChildPath 'Directory.Build.props'

echo $projectDirPath
echo $propsFilePath

$rawVesion = [version]([XML](Get-Content -Path $propsFilePath -Encoding UTF8)).CreateNavigator().Select('/Project/PropertyGroup/Version').Value
$vesion = @(
	"{0}"     -f $rawVesion.Major
	"{0:00}"  -f $rawVesion.Minor
	"{0:000}" -f $rawVesion.Build
) -join '-'
$vesion

$platforms = @('x64', 'x86')

$scripts = @{
	buildProject = Join-Path -Path $scriptDirPath -ChildPath 'build-project.ps1'
	archivePlugin = Join-Path -Path $scriptDirPath -ChildPath 'archive-plugin.ps1'
	createReleaseNote = Join-Path -Path $scriptDirPath -ChildPath 'create-release-note.ps1'
}

. $scripts.buildProject -ProjectName $pluginName -Platforms $platforms


