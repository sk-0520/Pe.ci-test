Param(
	[string[]] $Platforms = @('x64', 'x86')
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$pluginName = 'TEMPLATE_PluginName'

$scriptDirPath = Join-Path -Path $PSScriptRoot -ChildPath 'Build'

$scripts = @{
	test = Join-Path -Path $scriptDirPath -ChildPath 'test-project.ps1'
}

Write-Information 'プロジェクトテスト'
& $scripts.test -ProjectName "$pluginName.Test" -Platforms $Platforms
