Param(
	[Parameter(mandatory = $true)][string] $LogPath,
	[Parameter(mandatory = $true)][int] $ProcessId,
	[Parameter(mandatory = $true)][int] $WaitSeconds,
	[Parameter(mandatory = $true)][string] $ExecuteCommand,
	[Parameter(mandatory = $false, ValueFromRemainingArguments = $true)][string] $ExecuteArgument
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Start-Sleep -Seconds 3

Start-Transcript -Path $LogPath -Force

try {
	try {
		Write-Information "ProcessId: $ProcessId"
		Write-Information "WaitSeconds: $WaitSeconds"
		Write-Information "ExecuteCommand: $ExecuteCommand"
		Write-Information "ExecuteArgument: $ExecuteArgument"

		if ($ProcessId -ne 0 ) {
			Write-Information "プロセス終了待機: $ProcessId ..."
			try {
				Wait-Process -Id $ProcessId -Timeout $WaitSeconds
				Write-Information "プロセス終了: $ProcessId"
			} catch {
				Write-Warning $Error -ForegroundColor Yellow -BackgroundColor Black
				Write-Warning 'プロセス終了を無視'
			}
		}

		Write-Information ''
		Write-Information '再起動処理を実施します'
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

