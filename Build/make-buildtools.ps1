Param(
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path

$rootDirectory = Split-Path -Path $currentDirPath -Parent

$builToolDirPath = Join-Path $rootDirectory "Output\tools"

dotnet build Source/BuildTools/SqlPack/SqlPack.csproj --verbosity normal --configuration Debug /p:Platform=x86 --runtime win-x86 --output $builToolDirPath --no-self-contained
if (-not $?) {
	throw "build error: Build Tool"
}

