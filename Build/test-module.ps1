Param(
	[Parameter(mandatory = $true)][ValidateSet('boot', 'main', 'plugins')][string] $Module,
	[Parameter(mandatory = $true)][ValidateSet('x86', 'x64')][string] $Platform,
	[Parameter(mandatory = $true)][string] $Configuration,
	[string] $Logger
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'project.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path -Path $currentDirPath -ChildPath $scriptFileName
	. $scriptFilePath
}

#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------

if ($Module -eq 'boot') {
	$projectDirItems = GetTestProjectDirectories $Module

	foreach ($projectDirItem in $projectDirItems) {
		$testDirPath = Join-Path -Path $projectDirItem.FullName -ChildPath 'bin' | Join-Path -ChildPath $Configuration | Join-Path -ChildPath $Platform
		$testFileName = $projectDirItem.BaseName + '.dll'
		$testFilePath = Join-Path -Path $testDirPath -ChildPath $testFileName

		Write-Verbose "VSTest.Console $testFilePath /InIsolation /Platform:$Platform"
		VSTest.Console $testFilePath /InIsolation /Platform:$Platform
		if (-not $?) {
			throw "test error: $Module"
		}
	}
} elseif ($Module -eq 'main' -or $Module -eq 'plugins') {
	$loggerArg = ''
	if (![string]::IsNullOrEmpty($Logger)) {
		$loggerArg = "--logger:$Logger"
	}

	$projectDirItems = GetTestProjectDirectories $Module

	foreach ($projectDirItem in $projectDirItems) {
		$testDirPath = Join-Path -Path $projectDirItem.FullName -ChildPath 'bin' | Join-Path -ChildPath $Platform | Join-Path -ChildPath $Configuration
		$testFileName = $projectDirItem.BaseName + '.dll'
		$testFilePath = Join-Path -Path $testDirPath -ChildPath (Get-ChildItem -LiteralPath $testDirPath -Recurse -Name -File -Include $testFileName)

		Write-Verbose "dotnet test $testFilePath --test-adapter-path:. $loggerArg"
		dotnet test $testFilePath --test-adapter-path:. $loggerArg
		if (-not $?) {
			throw "test error: $Module - $testFileName"
		}
	}
} else {
	throw "module error: $Module"
}
