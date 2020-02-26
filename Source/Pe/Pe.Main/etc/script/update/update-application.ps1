# アップデート時に実施される処理
Param(
	[parameter(mandatory = $true)][string] $LogPath,
	[parameter(mandatory = $true)][int] $ProcessId,
	[parameter(mandatory = $true)][int] $WaitSeconds,
	[parameter(mandatory = $true)][System.IO.DirectoryInfo] $SourceDirectory,
	[parameter(mandatory = $true)][System.IO.DirectoryInfo] $DestinationDirectory,
	[parameter(mandatory = $true)][version] $CurrentVersion,
	[parameter(mandatory = $true)][ValidateSet("x86", "x64")][string] $Platform,
	[parameter(mandatory = $true)][string] $UpdateBeforeScript,
	[parameter(mandatory = $true)][string] $UpdateAfterScript,
	[parameter(mandatory = $true)][string] $ExecuteCommand,
	[parameter(mandatory = $false)][string] $ExecuteArgument
)
$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$appIcon = @(
	0, 0, 0, 0, 0, 0, 0, 0, 0, -1
	0, 0, 0, 0, 0, 0, 0, 0, 0, -1
	0, 1, 1, 1, 0, 0, 0, 0, 0, -1
	0, 1, 0, 1, 0, 0, 0, 0, 0, -1
	0, 1, 0, 1, 0, 1, 1, 1, 0, -1
	0, 1, 1, 1, 0, 1, 0, 1, 0, -1
	0, 1, 0, 0, 0, 1, 1, 1, 0, -1
	0, 1, 0, 0, 0, 1, 0, 0, 0, -1
	0, 1, 0, 0, 0, 1, 1, 1, 0, -1
	0, 0, 0, 0, 0, 0, 0, 0, 0, -1
	0, 0, 0, 0, 0, 0, 0, 0, 0, -1
)
foreach ($dot in $appIcon) {
	switch ($dot) {
		0 { Write-Host '   ' -ForegroundColor White -BackgroundColor White -NoNewline }
		1 { Write-Host '|||' -ForegroundColor Black -BackgroundColor Black -NoNewline }
		-1 { Write-Host '' }
	}
}

Start-TranScript -Path $LogPath -Force
try {
	try {
		Write-Host "ProcessId: $ProcessId"
		Write-Host "WaitSeconds: $WaitSeconds"
		Write-Host "SourceDirectory: $SourceDirectory"
		Write-Host "DestinationDirectory: $DestinationDirectory"
		Write-Host "CurrentVersion: $CurrentVersion"
		Write-Host "Platform: $Platform"
		Write-Host "ExecuteCommand: $ExecuteCommand"
		Write-Host "ExecuteArgument: $ExecuteArgument"

		$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path

		if ($ProcessId -ne 0 ) {
			Write-Output "プロセス終了待機: $ProcessId ..."
			try {
				Wait-Process -Id $ProcessId -Timeout $WaitSeconds
				Write-Host "プロセス終了: $ProcessId"
			}
			catch {
				Write-Host $Error -ForegroundColor Yellow -BackgroundColor Black
				Write-Host "プロセス終了を無視"
			}
		}

		Write-Host ""
		Write-Host "アップデート処理を実施します"
		Write-Host ""

		Write-Host ""
		Write-Host "最新アップデート前スクリプト"
		if ( Test-Path -Path $UpdateBeforeScript ) {
			Write-Host "実施: $UpdateBeforeScript" -BackgroundColor Gray
			Invoke-Expression "$UpdateBeforeScript -DestinationDirectory ""$DestinationDirectory"" -CurrentVersion $CurrentVersion -Platform $Platform"
			Write-Host "---------------------------" -BackgroundColor Gray
		}
		else {
			Write-Host "スクリプトなし" -BackgroundColor Gray
		}
		Write-Host ""

		Write-Host "本体アップデート処理実施"
		Write-Host "$SourceDirectory -> $DestinationDirectory"
		$customCopyItem = Join-Path $currentDirPath 'custom-copy-item.ps1'
		#Copy-Item -Path ($SourceDirectory.FullName + "/*") -Destination $DestinationDirectory.FullName -Recurse -Force
		Invoke-Expression "$customCopyItem -SourceDirectoryPath ""$SourceDirectory"" -DestinationDirectoryPath ""$DestinationDirectory"" -ProgressType 'output'"

		Write-Host ""
		Write-Host "最新アップデート後スクリプト"
		if ( Test-Path -Path $UpdateAfterScript ) {
			Write-Host "実施: $UpdateAfterScript" -BackgroundColor Gray
			Invoke-Expression "$UpdateAfterScript -DestinationDirectory ""$DestinationDirectory"" -CurrentVersion $CurrentVersion -Platform $Platform "
			Write-Host "---------------------------" -BackgroundColor Gray
		}
		else {
			Write-Host "スクリプトなし" -BackgroundColor Gray
		}

		Write-Host ""
		Write-Host "Pe を起動しています..."
		$process = Start-Process -PassThru -FilePath $ExecuteCommand -ArgumentList $ExecuteArgument
		$process.WaitForInputIdle() | Out-Null
	}
	finally {
		Stop-TranScript
	}

}
catch {
	Write-Host $error -ForegroundColor Red -BackgroundColor Black
	Read-Host "エラーが発生しました。`r`nログファイル: $LogPath を参照してください。`r`nEnter で終了します"
}

