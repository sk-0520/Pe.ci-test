
Param(
	[Parameter(mandatory = $true)][string] $TargetDirectory,
	[Parameter(mandatory = $true)][string] $OutputFileName
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootDirectory = Split-Path -Path $currentDirPath -Parent

#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------

Write-Output $TargetDirectory
Write-Output $OutputFileName
Write-Output $rootDirectory

$archive = [System.IO.Path]::GetExtension($OutputFileName).ToLower().Substring(1)

try {
	Push-Location $TargetDirectory

	switch ($archive) {
		'7z' {
			7z a -t7z -m0=lzma2 -mx=9 -mfb=64 -md=64m -ms=on -mmt=on "$OutputFileName" * -r -bsp1
		}
		'zip' {
			7z a -tzip "$OutputFileName" * -r -bsp1
		}
		'tar' {
			7z a -ttar "$OutputFileName" * -r -bsp1
		}
		Default { throw "error: $archive" }
	}
	if (-not $?) {
		throw "7z: $OutputFileName"
	}

	Move-Item -Path $OutputFileName -Destination $rootDirectory
}
finally {
	Pop-Location
}
