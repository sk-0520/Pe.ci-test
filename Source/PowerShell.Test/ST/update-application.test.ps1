Param(
	[Parameter(mandatory = $true)][string] $Source,
	[Parameter(mandatory = $true)][string] $Destination,
	[Parameter(mandatory = $true)][string] $Command
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$rootDirectory = Split-Path -Parent $PSScriptRoot | Split-Path -Parent | Split-Path -Parent

$updateScript = @{
	main = "${rootDirectory}\Source\Pe\Pe.Main\etc\script\update\update-application.ps1"
	before = "${rootDirectory}\Source\Pe\Pe.Main\etc\script\update\update-new-before.ps1"
	after = "${rootDirectory}\Source\Pe\Pe.Main\etc\script\update\update-new-after.ps1"
}

New-Item -Path $Destination -ItemType Directory
& $updateScript.main -LogPath "${Destination}.log" -ProcessId 0 -WaitSeconds 0 -SourceDirectory $Source -DestinationDirectory $Destination -CurrentVersion 0.0.0 -Platform x64 -UpdateBeforeScript $updateScript.before -UpdateAfterScript $updateScript.after -ExecuteCommand $Command
Write-Output "? = $?"
if(-not $?) {
	throw "? = $?"
}
$sourceFiles = @(Get-Childitem -LiteralPath $Source -Recurse -Force)
$destinationFiles = @(Get-Childitem -LiteralPath $Destination -Recurse -Force)

$errors = @()
if($sourceFiles.Length -ne $destinationFiles.Length) {
	$errors += "source($($sourceFiles.Length)) -ne destination($($destinationFiles.Length))"
}

for ($i = 0; $i -lt $sourceFiles.Length; $i++) {
	$sourceFile = $sourceFiles[$i]
	$destPath = $sourceFile.FullName.Replace($Source, $Destination)
	if(Test-Path -LiteralPath $destPath) {
		if(Test-Path -LiteralPath $sourceFile.FullName -PathType Container) {
			if(!(Test-Path -LiteralPath $destPath -PathType Container)) {
				$errors += "sourceFile($($sourceFile.FullName))[dir] : destPath($destPath)[file]"
			}
		} else {
			$srcHash = Get-FileHash -LiteralPath $sourceFile.FullName -Algorithm SHA256
			$dstHash = Get-FileHash -LiteralPath $destPath -Algorithm SHA256

			if($srcHash.Hash -ne $dstHash.Hash) {
				$errors += "srcHash($($srcHash.Hash)) -ne dstHash($($dstHash.Hash))"
			}
		}
	} else {
		$errors += "not found $destPath"
	}
}

if(0 -lt $errors.Length) {
	throw ($errors -join [System.Environment]::NewLine)
}
