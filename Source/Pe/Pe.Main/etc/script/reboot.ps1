Param(
	[Parameter(mandatory = $true)][string] $LogPath,
	[Parameter(mandatory = $true)][int] $ProcessId,
	[Parameter(mandatory = $true)][int] $WaitSeconds,
	[Parameter(mandatory = $true)][string] $ExecuteCommand,
	[Parameter(mandatory = $false,ValueFromRemainingArguments=$true)][string] $ExecuteArgument
)
$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

Start-Sleep -Seconds 3

Start-TranScript -Path $LogPath -Force

try {
	try {
		Write-Host "ProcessId: $ProcessId"
		Write-Host "WaitSeconds: $WaitSeconds"
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
		Write-Host "再起動処理を実施します"
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

