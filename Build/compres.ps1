Param(
    [parameter(mandatory=$true)][string] $SourceDirectory,
    [parameter(mandatory=$true)][string] $DestinationDirectory,
    [parameter(mandatory=$true)][string] $Platform
)
$ErrorActionPreference = 'Stop'

$projectFile = Join-Path (Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)) "Source/Pe/Pe.Main/Pe.Main.csproj"

$projectXml = [XML](Get-Content -Path $projectFile -Encoding UTF8)
$projectNav = $projectXml.CreateNavigator()
$vesion = $projectNav.Select('/Project/PropertyGroup/Version').Value

$destinationPath = Join-Path $DestinationDirectory ("Pe_" + $vesion + "_" + $Platform + ".zip")

Compress-Archive -Force -Path (Join-Path $SourceDirectory "*") -DestinationPath $destinationPath

