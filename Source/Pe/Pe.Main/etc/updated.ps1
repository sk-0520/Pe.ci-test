# アップデート後に最新版として実施される処理（ローカルじゃなくて更新モジュールが対象）
Param(
	[parameter(mandatory = $true)][System.IO.DirectoryInfo] $DestinationDirectory,
	[parameter(mandatory = $true)][ValidateSet("x32", "x64")][string] $Platform
)
$ErrorActionPreference = "Stop"

Write-Host "DestinationDirectory: $DestinationDirectory"
Write-Host "Platform: $Platform"

Write-Host "最新アップデート後スクリプト実施!!"
