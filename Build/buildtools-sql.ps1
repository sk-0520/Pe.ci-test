Param(
	[Parameter(mandatory = $true)][string] $BuildToolsSqlPack,
	[Parameter(mandatory = $true)][string] $OutputFile
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'project.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path $currentDirPath $scriptFileName
	. $scriptFilePath
}

#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------

$sourceDirectory = GetSourceDirectory 'main'
$sqlDirectory = Join-Path -Path $sourceDirectory -ChildPath 'Pe.Main' | Join-Path -ChildPath 'etc' | Join-Path -ChildPath  'sql'

Write-Output "pack SQL"
if (Test-Path $OutputFile) {
	Remove-Item -Path $OutputFile
}
& "$BuildToolsSqlPack" --sql-root-dir $sqlDirectory --output $OutputFile
