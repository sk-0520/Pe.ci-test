Param()

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path

$pluginName = 'TEMPLATE_PluginName'

$scriptDirPath = Join-Path -Path $currentDirPath -ChildPath 'Build'

$platforms = @('x64', 'x86')

$scripts = @{
	test = Join-Path -Path $scriptDirPath -ChildPath 'test-project.ps1'
}

Write-Information 'プロジェクトビルド'
& $scripts.test -ProjectName "$pluginName.Test" -Platforms $platforms
