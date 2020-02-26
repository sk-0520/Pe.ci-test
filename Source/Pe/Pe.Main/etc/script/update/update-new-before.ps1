# アップデート後に最新版として実施される前処理（ローカルじゃなくて更新モジュールが対象）
Param(
	[parameter(mandatory = $true)][System.IO.DirectoryInfo] $DestinationDirectory,
	[parameter(mandatory = $true)][version] $CurrentVersion,
	[parameter(mandatory = $true)][ValidateSet("x86", "x64")][string] $Platform
)
$ErrorActionPreference = "Stop"

Write-Host "最新アップデート前スクリプト実施!!"

Write-Host "DestinationDirectory: $DestinationDirectory"
Write-Host "CurrentVersion: $CurrentVersion"
Write-Host "Platform: $Platform"

