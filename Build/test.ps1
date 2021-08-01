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
	foreach ($mainConfiguration in $MainConfigurations) {
		dotnet test $mainSolutionPath --verbosity normal --no-build --no-restore --configuration $mainConfiguration --runtime win-$platform /p:Platform=$platform /p:DefineConstants="" --test-adapter-path:. $mainLoggerArg
	}

	$bootLoggerArg = ''
	if (![string]::IsNullOrEmpty($Logger)) {
		$bootLoggerArg = "/Logger:$Logger"
	}
	$projectDirItems = Get-ChildItem -Path $sourceBootDirectoryPath -Filter "*.Test" -Directory
	foreach ($bootConfiguration in $BootConfigurations) {
		foreach($projectDirItem in $projectDirItems) {
			$testDirPath = Join-Path $projectDirItem.FullName "bin" | Join-Path -ChildPath $bootConfiguration | Join-Path -ChildPath $platform
			$testFileName = $projectDirItem.BaseName + '.dll'
			$testFilePath = Join-Path $testDirPath $testFileName

			VSTest.Console $testFilePath /InIsolation /Platform:$platform $bootLoggerArg
		}
	}

	$releaseDirPath = Join-Path $outputDirectoryPath $platform | Join-Path -ChildPath "Pe"
	$releaseAppPath = Join-Path $releaseDirPath "Pe.exe"
	#echo $releaseAppPath
}
