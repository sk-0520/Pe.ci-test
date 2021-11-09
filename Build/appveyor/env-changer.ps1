# コミットメッセージの2行目以降のコメント「//KEY:VALUE」を割り当て可能な環境変数に上書きする
# 割り当て環境変数は $ciEnvs 定義する

$ciEnvs = @(
	'BUILD_TYPE',
	'MAIN_ARCHIVE',
	'DEFAULT_ARCHIVE',
	'MINIMUM_VERSION',
	'NUGET_PACKAGES'
)

if ($env:APPVEYOR_REPO_COMMIT_MESSAGE_EXTENDED) {
	foreach ($line in $env:APPVEYOR_REPO_COMMIT_MESSAGE_EXTENDED.Split("\n", [StringSplitOptions]::RemoveEmptyEntries)) {
		if ($line -match '^//\s*(?<KEY>\w+)\s*:\s*(?<VALUE>\w+)') {
			$key = $Matches['KEY']
			$value = $Matches['VALUE']
			if ($ciEnvs.Contains($key)) {
				$oldValue = [Environment]::GetEnvironmentVariable($key)
				[Environment]::SetEnvironmentVariable($key, $value)
				Write-Host ("CHANGE: [$key] $oldValue -> $value")
			}
		}
	}
}
