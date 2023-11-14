Param()

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$pluginName = 'TEMPLATE_PluginName'

$scriptDirPath = Join-Path -Path $PSScriptRoot -ChildPath 'Build'

$platforms = @('x64', 'x86')

$scripts = @{
	test = Join-Path -Path $scriptDirPath -ChildPath 'test-project.ps1'
}

Write-Information 'プロジェクトテスト'
& $scripts.test -ProjectName "$pluginName.Test" -Platforms $platforms
