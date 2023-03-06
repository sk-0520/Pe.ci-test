Param(
	[Parameter(mandatory = $true)][string[]] $MainConfigurations,
	[Parameter(mandatory = $true)][string[]] $BootConfigurations,
	[Parameter(mandatory = $false)][string] $Logger,
	[Parameter(mandatory = $true)][string[]] $Platforms
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'command.ps1',
	'version.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path $currentDirPath $scriptFileName
	. $scriptFilePath
}
$rootDirectory = Split-Path -Path $currentDirPath -Parent

$sourceMainDirectoryPath = Join-Path "$rootDirectory" "Source/Pe"
$sourceBootDirectoryPath = Join-Path "$rootDirectory" "Source/Pe.Boot"

$outputDirectoryPath = Join-Path "$rootDirectory" "Output/Release"

$mainSolutionPath = Join-Path $sourceMainDirectoryPath "Pe.sln"

foreach ($platform in $Platforms) {
	$mainLoggerArg = ''
	if (![string]::IsNullOrEmpty($Logger)) {
		$mainLoggerArg = "--logger:$Logger"
	}
	$mainProjectDirItems = Get-ChildItem -Path $sourceMainDirectoryPath -Filter "*.Test" -Directory
	foreach ($mainConfiguration in $MainConfigurations) {
		foreach ($projectDirItem in $mainProjectDirItems) {
			$testDirPath = Join-Path $projectDirItem.FullName "bin" | Join-Path -ChildPath $platform | Join-Path -ChildPath $mainConfiguration
			$testFileName = $projectDirItem.BaseName + '.dll'
			$testFilePath = Join-Path $testDirPath (Get-ChildItem -LiteralPath $testDirPath -Recurse -Name -File -Include $testFileName)

			dotnet test $testFilePath $mainLoggerArg
			if (-not $?) {
				exit 1
			}
		}
	}

	$bootLoggerArg = ''
	if (![string]::IsNullOrEmpty($Logger)) {
		$bootLoggerArg = "/Logger:$Logger"
	}
	$bootProjectDirItems = Get-ChildItem -Path $sourceBootDirectoryPath -Filter "*.Test" -Directory
	foreach ($bootConfiguration in $BootConfigurations) {
		foreach ($projectDirItem in $bootProjectDirItems) {
			$testDirPath = Join-Path $projectDirItem.FullName "bin" | Join-Path -ChildPath $bootConfiguration | Join-Path -ChildPath $platform
			$testFileName = $projectDirItem.BaseName + '.dll'
			$testFilePath = Join-Path $testDirPath $testFileName

			VSTest.Console $testFilePath /InIsolation /Platform:$platform $bootLoggerArg
			if (-not $?) {
				exit 1
			}
		}
	}

	$releaseDirPath = Join-Path $outputDirectoryPath $platform | Join-Path -ChildPath "Pe"
	$releaseAppPath = Join-Path $releaseDirPath "Pe.exe"

	& $releaseAppPath --_mode dry-run --mou honma --kanben shitekure
	if (-not $?) {
		exit 1
	}

}
