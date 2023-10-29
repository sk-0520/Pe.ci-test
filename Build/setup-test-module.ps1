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
		# 何もしない
	}
	elseif ($Module -eq 'main' -or $Module -eq 'plugins') {
		$testProjectDirs = GetTestProjectDirectories $Module
		foreach ($testProjectDir in $testProjectDirs) {
			Push-Location $testProjectDir
			try {
				nuget install GitHubActionsTestLogger
				if (-not $?) {
					throw "error: $Module - $testProjectDir"
				}
			}
			finally {
				Pop-Location
			}
		}
	}
	else {
		throw "error module: $Module"
	}
}
else {
	throw "error service: $Service"
}
