
Param(
	[Parameter(mandatory = $true)][string] $TargetDirectory,
	[Parameter(mandatory = $true)][string] $OutputFileBaseName,
	[Parameter(mandatory = $true)][ValidateSet('zip', '7z', 'tar')][string] $Archive
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootDirectory = Split-Path -Path $currentDirPath -Parent

#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------

Write-Output $TargetDirectory
Write-Output $OutputFileBaseName
Write-Output $rootDirectory

$outputFileName = $OutputFileBaseName + '.' + $Archive

try {
	Push-Location $TargetDirectory

	switch ($Archive) {
		'7z' {
			7z a -t7z -m0=lzma2 -mx=9 -mfb=64 -md=64m -ms=on -mmt=on "$outputFileName" * -r -bsp1
		}
		'zip' {
			7z a -tzip "$outputFileName" * -r -bsp1
		}
		'tar' {
			7z a -ttar "$outputFileName" * -r -bsp1
		}
		Default {
			throw "error: $Archive"
  }
	}
	if (-not $?) {
		throw "7z: $outputFileName"
	}

	Move-Item -Path $outputFileName -Destination $rootDirectory
} finally {
	Pop-Location
}
