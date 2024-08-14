Param(
	[switch] $Fix
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Project"

$sqlDir = Join-Path -Path (Get-RootDirectory) -ChildPath 'Source/Pe/Pe.Main/etc/sql'

$versionTag = '3.1.0'

$mode = 'lint'
if($Fix) {
	$mode = 'fix'
}

docker run `
	--interactive `
	--tty `
	--rm `
	--volume ${sqlDir}:/sql `
	sqlfluff/sqlfluff:${versionTag} `
	${mode} `
	/sql
