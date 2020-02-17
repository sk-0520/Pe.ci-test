Param(
	[parameter(mandatory = $true)][string] $SourceDirectory,
	[parameter(mandatory = $true)][string] $DestinationDirectory,
	[parameter(mandatory = $true)][string] $Platform,
	[parameter(mandatory = $true)][ValidateSet("zip", "7z")][string] $Archive,
	[switch] $Diet
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


$version = GetAppVersion

$archiveFileName = (ConvertAppArchiveFileName $version $Platform $Archive)

switch ($Archive) {
	'zip' {
		$destinationPath = Join-Path $DestinationDirectory $archiveFileName
		Compress-Archive -Force -Path (Join-Path $SourceDirectory "*") -DestinationPath $destinationPath
	}
	'7z' {
		try {
			Push-Location $SourceDirectory
			7z a -t7z -m0=lzma2 -mx=9 -mfb=64 -md=64m -ms=on "$archiveFileName" * -r
		} finally {
			Pop-Location
		}
		Move-Item -Path (Join-Path $SourceDirectory $archiveFileName) -Destination $DestinationDirectory
	}
}


