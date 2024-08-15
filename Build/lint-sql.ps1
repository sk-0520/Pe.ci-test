Param(
	[switch] $Fix,
	[switch] $RunCi,
	[string] $File = ''
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Project"

$sqlDir = Join-Path -Path (Get-RootDirectory) -ChildPath 'Source/Pe/Pe.Main/etc/sql'

$versionTag = '3.1.0'

$mode = 'lint'
if ($Fix) {
	$mode = 'fix'
}

$params = @()

if (! $RunCi) {
	$params += '--tty'
	$params += '--interactive'
}

$params += '--rm'
$params += '--volume'
$params += "${sqlDir}:/sql"
$params += "sqlfluff/sqlfluff:${versionTag}"
$params += ${mode}
if($File) {
	$sqlPath = $File.Replace('\', '/').TrimStart('/')
	$params += "/sql/${sqlPath}"
} else {
	$params += '/sql'
}

docker run ($params)
