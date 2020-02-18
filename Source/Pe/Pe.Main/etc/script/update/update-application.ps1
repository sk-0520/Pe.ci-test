# アップデート時に実施される処理
Param(
	[parameter(mandatory = $true)][int] $ProcessId,
	[parameter(mandatory = $true)][int] $WaitSeconds,
	[parameter(mandatory = $true)][System.IO.DirectoryInfo] $SourceDirectory,
	[parameter(mandatory = $true)][System.IO.DirectoryInfo] $DestinationDirectory,
	[parameter(mandatory = $true)][version] $CurrentVersion,
	[parameter(mandatory = $true)][ValidateSet("x32", "x64")][string] $Platform,
	[parameter(mandatory = $true)][string] $UpdateBeforeScript,
	[parameter(mandatory = $true)][string] $UpdateAfterScript,
	[parameter(mandatory = $true)][string] $ExecuteCommand,
	[parameter(mandatory = $false)][string] $ExecuteArgument
)
$ErrorActionPreference = "Stop"

Write-Host "ProcessId: $ProcessId"
Write-Host "WaitSeconds: $WaitSeconds"
Write-Host "SourceDirectory: $SourceDirectory"
Write-Host "DestinationDirectory: $DestinationDirectory"
Write-Host "Platform: $Platform"
Write-Host "UpdateScript: $UpdateScript"
Write-Host "ExecuteCommand: $ExecuteCommand"
Write-Host "ExecuteArgument: $ExecuteArgument"

if ($ProcessId -ne 0 ) {
	Write-Output "プロセス終了待機: $ProcessId ..."
	try {
		Wait-Process -Id $ProcessId -Timeout $WaitSeconds
		Write-Host "プロセス終了: $ProcessId"
	}
	catch {
		Write-Host $Error
		Write-Host "プロセス($ProcessId)が存在しなかったためプロセス終了を無視"
	}
}

if ( Test-Path -Path $UpdateBeforeScript ) {
	Write-Host "最新アップデート前スクリプトの実施: $UpdateBeforeScript"
	Invoke-Expression "$UpdateBeforeScript -DestinationDirectory ""$DestinationDirectory"" -CurrentVersion $CurrentVersion -Platform $Platform "
}

Write-Host "アップデート処理実施"
Write-Host "$SourceDirectory -> $DestinationDirectory"
Copy-Item -Path ($SourceDirectory.FullName + "/*") -Destination $DestinationDirectory.FullName -Recurse -Force

if ( Test-Path -Path $UpdateAfterScript ) {
	Write-Host "最新アップデート後スクリプトの実施: $UpdateAfterScript"
	Invoke-Expression "$UpdateAfterScript -DestinationDirectory ""$DestinationDirectory"" -CurrentVersion $CurrentVersion -Platform $Platform "
}

Start-Process -FilePath $ExecuteCommand -ArgumentList $ExecuteArgument

Read-Host "Enter キーを押すとコンソールが閉じます ..."
