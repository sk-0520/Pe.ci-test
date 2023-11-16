Param(
	[Parameter(mandatory = $true)][string] $BuildToolsSqlPack,
	[Parameter(mandatory = $true)][string] $OutputFile
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Project"


#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------

$sourceDirectory = Get-SourceDirectory -Kind 'main'
$sqlDirectory = Join-Path -Path $sourceDirectory -ChildPath 'Pe.Main' | Join-Path -ChildPath 'etc' | Join-Path -ChildPath  'sql'

Write-Information 'Package SQL'
if (Test-Path $OutputFile) {
	Remove-Item -Path $OutputFile
}
& "$BuildToolsSqlPack" --sql-root-dir $sqlDirectory --output $OutputFile
