Param(
	[Parameter(mandatory = $true)][ValidateSet('boot', 'main', 'plugins')][string] $Module,
	[ValidateSet('github')][string] $Service
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Project"


#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------

if ($Service -eq 'github') {
	if ($Module -eq 'boot') {
		# 何もしない
	} elseif ($Module -eq 'main' -or $Module -eq 'plugins') {
		$testProjectDirs = Get-TestProjectDirectory -Kind $Module
		foreach ($testProjectDir in $testProjectDirs) {
			Push-Location $testProjectDir
			try {
				nuget install GitHubActionsTestLogger
				if (-not $?) {
					throw "error: $Module - $testProjectDir"
				}
			} finally {
				Pop-Location
			}
		}
	} else {
		throw "error module: $Module"
	}
} else {
	throw "error service: $Service"
}
