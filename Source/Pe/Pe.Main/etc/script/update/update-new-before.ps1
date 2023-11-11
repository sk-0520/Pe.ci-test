# アップデート後に最新版として実施される前処理（ローカルじゃなくて更新モジュールが対象）
Param(
	[Parameter(mandatory = $true)][System.IO.DirectoryInfo] $DestinationDirectory,
	[Parameter(mandatory = $true)][version] $CurrentVersion,
	[Parameter(mandatory = $true)][ValidateSet("x86", "x64")][string] $Platform
)
$ErrorActionPreference = "Stop"

Write-Information "最新アップデート前スクリプト実施!!"

Write-Information "DestinationDirectory: $DestinationDirectory"
Write-Information "CurrentVersion: $CurrentVersion"
Write-Information "Platform: $Platform"

