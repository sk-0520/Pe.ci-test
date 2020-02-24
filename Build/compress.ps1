Param(
	[switch] $Diet,
	[parameter(mandatory = $true)][ValidateSet("zip", "7z")][string] $Archive,
	[parameter(mandatory = $true)][string[]] $Platforms
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

foreach($platform in $Platforms) {
	$archiveFileName = (ConvertAppArchiveFileName $version $platform $Archive)
	$binDirPath = "Output\Release\$platform\Pe"

	switch ($Archive) {
		'zip' {
			$destinationPath = Join-Path 'Output' $archiveFileName
			Compress-Archive -Force -Path (Join-Path $binDirPath "*") -DestinationPath $destinationPath
		}
		'7z' {
			try {
				Push-Location $binDirPath
				7z a -t7z -m0=lzma2 -mx=9 -mfb=64 -md=64m -ms=on "$archiveFileName" * -r
			} finally {
				Pop-Location
			}
			Move-Item -Path (Join-Path $binDirPath $archiveFileName) -Destination 'Output'
		}
	}
}
