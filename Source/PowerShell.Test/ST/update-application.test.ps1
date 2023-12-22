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
# $source = Get-Childitem -Path $Source -Recurse
# $destination = Get-Childitem -Path $Destination -Recurse

# Compare-Object $source $destination -Property Name, Length
