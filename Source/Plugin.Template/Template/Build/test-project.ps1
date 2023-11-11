Param(
	[Parameter(mandatory = $true)][string] $ProjectName,
	[Parameter(mandatory = $true)][string[]] $Platforms
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'command.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path -Path $currentDirPath -ChildPath $scriptFileName
	. $scriptFilePath
}


foreach ($platform in $Platforms) {
	dotnet test Source/$ProjectName/$ProjectName.csproj /m --verbosity normal --configuration Release /p:Platform=$platform --runtime win-$platform
	if (-not $?) {
		throw "test error: $?"
	}
}
