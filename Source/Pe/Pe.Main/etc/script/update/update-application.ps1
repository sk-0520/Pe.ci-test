[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingInvokeExpression')]
# アップデート時に実施される処理
Param(
	[Parameter(mandatory = $true)][string] $LogPath,
	[Parameter(mandatory = $true)][int] $ProcessId,
	[Parameter(mandatory = $true)][int] $WaitSeconds,
	[Parameter(mandatory = $true)][System.IO.DirectoryInfo] $SourceDirectory,
	[Parameter(mandatory = $true)][System.IO.DirectoryInfo] $DestinationDirectory,
	[Parameter(mandatory = $true)][version] $CurrentVersion,
	[Parameter(mandatory = $true)][ValidateSet('x86', 'x64')][string] $Platform,
	[Parameter(mandatory = $true)][string] $UpdateBeforeScript,
	[Parameter(mandatory = $true)][string] $UpdateAfterScript,
	[Parameter(mandatory = $true)][string] $ExecuteCommand,
	[Parameter(mandatory = $false, ValueFromRemainingArguments = $true)][string] $ExecuteArgument
)
$ErrorActionPreference = 'Stop'
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
		0 {
			Write-Information '   ' -NoNewline -ForegroundColor White -BackgroundColor White
		}
		1 {
			Write-Information '|||' -NoNewline -ForegroundColor Black -BackgroundColor Black
		}
		2 {
			Write-Information '===' -NoNewline -ForegroundColor Red -BackgroundColor Red
		}
		3 {
			Write-Information '---' -NoNewline -ForegroundColor DarkRed -BackgroundColor DarkRed
		}
		9 {
			Write-Information '   ' -NoNewline
		}
		-1 {
			Write-Information ''
		}
	}
}
Start-Sleep -Seconds 3

Start-Transcript -Path "$LogPath" -Force
try {
	try {
		Write-Information "ProcessId: $ProcessId"
		Write-Information "WaitSeconds: $WaitSeconds"
		Write-Information "SourceDirectory: $SourceDirectory"
		Write-Information "DestinationDirectory: $DestinationDirectory"
		Write-Information "CurrentVersion: $CurrentVersion"
		Write-Information "Platform: $Platform"
		Write-Information "ExecuteCommand: $ExecuteCommand"
		Write-Information "ExecuteArgument: $ExecuteArgument"

		$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path

		if ($ProcessId -ne 0 ) {
			Write-Output "プロセス終了待機: $ProcessId ..."
			try {
				Wait-Process -Id $ProcessId -Timeout $WaitSeconds
				Write-Information "プロセス終了: $ProcessId"
			} catch {
				Write-Information $Error -ForegroundColor Yellow -BackgroundColor Black
				Write-Information 'プロセス終了を無視'
			}
		}

		Write-Information ''
		Write-Information 'アップデート処理を実施します'
		Write-Information ''

		Write-Information ''
		Write-Information '最新アップデート前スクリプト'
		if ( Test-Path -Path $UpdateBeforeScript ) {
			Write-Information "実施: $UpdateBeforeScript" -BackgroundColor Gray
			$escapeUpdateBeforeScript = $UpdateBeforeScript -Replace ' ', '` '
			Invoke-Expression "$escapeUpdateBeforeScript -DestinationDirectory ""$DestinationDirectory"" -CurrentVersion $CurrentVersion -Platform $Platform"
			Write-Information '---------------------------' -BackgroundColor Gray
		} else {
			Write-Information 'スクリプトなし' -BackgroundColor Gray
		}
		Write-Information ''

		Write-Information '本体アップデート処理実施'
		Write-Information "$SourceDirectory -> $DestinationDirectory"
		$escapeCustomCopyItem = (Join-Path -Path $currentDirPath -ChildPath 'custom-copy-item.ps1') -Replace ' ', '` '
		#Copy-Item -Path ($SourceDirectory.FullName + "/*") -Destination $DestinationDirectory.FullName -Recurse -Force
		Invoke-Expression "$escapeCustomCopyItem -SourceDirectoryPath ""$SourceDirectory"" -DestinationDirectoryPath ""$DestinationDirectory"" -ProgressType 'output'"

		Write-Information ''
		Write-Information '最新アップデート後スクリプト'
		if ( Test-Path -Path $UpdateAfterScript ) {
			Write-Information "実施: $UpdateAfterScript" -BackgroundColor Gray
			$escapeUpdateAfterScript = $UpdateAfterScript -Replace ' ', '` '
			Invoke-Expression "$escapeUpdateAfterScript -DestinationDirectory ""$DestinationDirectory"" -CurrentVersion $CurrentVersion -Platform $Platform "
			Write-Information '---------------------------' -BackgroundColor Gray
		} else {
			Write-Information 'スクリプトなし' -BackgroundColor Gray
		}

		Write-Information ''
		Write-Information 'Pe を起動しています...'
		if ( $ExecuteArgument ) {
			$process = Start-Process -PassThru -FilePath $ExecuteCommand -ArgumentList $ExecuteArgument
		} else {
			$process = Start-Process -PassThru -FilePath $ExecuteCommand
		}
		$process.WaitForInputIdle() | Out-Null
	} finally {
		Stop-Transcript
	}

} catch {
	Write-Information $error -ForegroundColor Red -BackgroundColor Black
	Read-Host "エラーが発生しました。`r`nログファイル: $LogPath を参照してください。`r`nEnter で終了します"
}

