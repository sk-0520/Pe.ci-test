Param(
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'command.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path $currentDirPath $scriptFileName
	. $scriptFilePath
}
$rootDirectory = Split-Path -Path $currentDirPath -Parent
$builToolDirPath = Join-Path $rootDirectory "Output\tools"

try {
	Push-Location $rootDirectory
	dotnet build  Source/BuildTools/BuildTools.sln /m --verbosity minimal --configuration Debug /p:Platform=x86 --runtime win-x86 --output $builToolDirPath
} finally {
	Pop-Location
}

