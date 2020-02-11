Param(
    [parameter(mandatory=$true)][System.IO.DirectoryInfo] $DestinationDirectory,
    [parameter(mandatory=$true)][ValidateSet("x32", "x64")][string] $Platform
)
#$OutputEncoding='utf-8'
$ErrorActionPreference = "Stop"

Write-Output "DestinationDirectory: $DestinationDirectory"
Write-Output "Platform: $Platform"

Write-Output "UPDATED SCRIPT!"
