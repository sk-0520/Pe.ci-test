$ErrorActionPreference = 'Stop'

$projectFile = Join-Path (Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)) "Source/Pe/Pe.Main/Pe.Main.csproj"

function Get-AppVersion {
    $projectXml = [XML](Get-Content -Path $projectFile -Encoding UTF8)
    $projectNav = $projectXml.CreateNavigator()
    $vesion = $projectNav.Select('/Project/PropertyGroup/Version').Value

    return $vesion
}
