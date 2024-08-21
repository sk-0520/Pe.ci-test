Param(
	[switch] $Fix,
	[switch] $RunCi,
	[string] $File = ''
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$imageName = 'sqlfluff/sqlfluff'
$versionTag = '3.1.1'


Import-Module "${PSScriptRoot}/Modules/Project"

$sqlDir = Join-Path -Path (Get-RootDirectory) -ChildPath 'Source/Pe/Pe.Main/etc/sql'

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
$params += "${imageName}:${versionTag}"
$params += ${mode}
if($File) {
	$sqlPath = $File.Replace('\', '/').TrimStart('/')
	$prefix = 'sql/'
	if($sqlPath.StartsWith($prefix)) {
		$sqlPath = $sqlPath.Substring($prefix.Length)
	}
	$params += "/sql/${sqlPath}"
} else {
	$params += '/sql'
}

docker run ($params)
