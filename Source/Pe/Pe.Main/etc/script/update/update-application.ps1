# アップデート時に実施される処理
Param(
	[Parameter(mandatory = $true)][string] $LogPath,
	[Parameter(mandatory = $true)][int] $ProcessId,
	[Parameter(mandatory = $true)][int] $WaitSeconds,
	[Parameter(mandatory = $true)][System.IO.DirectoryInfo] $SourceDirectory,
	[Parameter(mandatory = $true)][System.IO.DirectoryInfo] $DestinationDirectory,
	[Parameter(mandatory = $true)][version] $CurrentVersion,
	[Parameter(mandatory = $true)][ValidateSet("x86", "x64")][string] $Platform,
	[Parameter(mandatory = $true)][string] $UpdateBeforeScript,
	[Parameter(mandatory = $true)][string] $UpdateAfterScript,
	[Parameter(mandatory = $true)][string] $ExecuteCommand,
	[Parameter(mandatory = $false,ValueFromRemainingArguments=$true)][string] $ExecuteArgument
)
$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$appIcon = @(
	9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, -1
	9, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, -1
	9, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, -1
	9, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, -1
	9, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, -1
	9, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 9, 2, 9, 2, 9, 2, 2, 2, 9, 2, 2, 9, 9, 9, 2, 9, 9, 2, 2, 2, 9, 2, 2, 2, -1
	9, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 9, 2, 9, 2, 9, 2, 3, 2, 9, 2, 3, 2, 9, 2, 3, 2, 9, 3, 2, 3, 9, 2, 3, 3, -1
	9, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 1, 9, 2, 9, 2, 9, 2, 9, 2, 9, 2, 9, 2, 9, 2, 9, 2, 9, 9, 2, 9, 9, 2, 9, 9, -1
	9, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 9, 2, 9, 2, 9, 2, 2, 2, 9, 2, 9, 2, 9, 2, 9, 2, 9, 9, 2, 9, 9, 2, 2, 2, -1
	9, 1, 0, 1, 0, 0, 0, 1, 1, 1, 0, 1, 9, 2, 9, 2, 9, 2, 3, 3, 9, 2, 9, 2, 9, 2, 2, 2, 9, 9, 2, 9, 9, 2, 3, 3, -1
	9, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 9, 2, 9, 2, 9, 2, 9, 9, 9, 2, 9, 2, 9, 2, 3, 2, 9, 9, 2, 9, 9, 2, 9, 9, -1
	9, 1, 0, 1, 0, 0, 0, 1, 1, 1, 0, 1, 9, 2, 2, 2, 9, 2, 9, 9, 9, 2, 2, 3, 9, 2, 9, 2, 9, 9, 2, 9, 9, 2, 2, 2, -1
	9, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 9, 3, 3, 3, 9, 3, 9, 9, 9, 3, 3, 9, 9, 3, 9, 3, 9, 9, 3, 9, 9, 3, 3, 3, -1
	9, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, -1
	9, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, -1
	9, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, -1
	9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, -1
)
foreach ($dot in $appIcon) {
	switch ($dot) {
		0 { Write-Host '   ' -NoNewline -ForegroundColor White -BackgroundColor White }
		1 { Write-Host '|||' -NoNewline -ForegroundColor Black -BackgroundColor Black }
		2 { Write-Host '===' -NoNewline -ForegroundColor Red -BackgroundColor Red }
		3 { Write-Host '---' -NoNewline -ForegroundColor DarkRed -BackgroundColor DarkRed }
		9 { Write-Host '   ' -NoNewline }
		-1 { Write-Host '' }
	}
}
Start-Sleep -Seconds 3

Start-TranScript -Path "$LogPath" -Force
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
			$escapeUpdateBeforeScript = $UpdateBeforeScript -Replace ' ', '` '
			Invoke-Expression "$escapeUpdateBeforeScript -DestinationDirectory ""$DestinationDirectory"" -CurrentVersion $CurrentVersion -Platform $Platform"
			Write-Host "---------------------------" -BackgroundColor Gray
		}
		else {
			Write-Host "スクリプトなし" -BackgroundColor Gray
		}
		Write-Host ""

		Write-Host "本体アップデート処理実施"
		Write-Host "$SourceDirectory -> $DestinationDirectory"
		$escapeCustomCopyItem = (Join-Path -Path $currentDirPath -ChildPath 'custom-copy-item.ps1') -Replace ' ', '` '
		#Copy-Item -Path ($SourceDirectory.FullName + "/*") -Destination $DestinationDirectory.FullName -Recurse -Force
		Invoke-Expression "$escapeCustomCopyItem -SourceDirectoryPath ""$SourceDirectory"" -DestinationDirectoryPath ""$DestinationDirectory"" -ProgressType 'output'"

		Write-Host ""
		Write-Host "最新アップデート後スクリプト"
		if ( Test-Path -Path $UpdateAfterScript ) {
			Write-Host "実施: $UpdateAfterScript" -BackgroundColor Gray
			$escapeUpdateAfterScript = $UpdateAfterScript -Replace ' ', '` '
			Invoke-Expression "$escapeUpdateAfterScript -DestinationDirectory ""$DestinationDirectory"" -CurrentVersion $CurrentVersion -Platform $Platform "
			Write-Host "---------------------------" -BackgroundColor Gray
		}
		else {
			Write-Host "スクリプトなし" -BackgroundColor Gray
		}

		Write-Host ""
		Write-Host "Pe を起動しています..."
		if( $ExecuteArgument ) {
			$process = Start-Process -PassThru -FilePath $ExecuteCommand -ArgumentList $ExecuteArgument
		} else {
			$process = Start-Process -PassThru -FilePath $ExecuteCommand
		}
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

