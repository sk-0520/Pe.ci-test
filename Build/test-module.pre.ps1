Param(
	[Parameter(mandatory = $true)][ValidateSet('boot', 'main', 'plugins')][string] $Module,
	[ValidateSet('github')][string] $Service
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

if ($Service -eq 'github') {
	if ($Module -eq 'boot') {
	}
	elseif ($Module -eq 'main') {
	}
	elseif ($Module -eq 'plugins') {
	}
	else {
		throw "error module: $Module"
	}
}
else {
	throw "error service: $Service"
}
