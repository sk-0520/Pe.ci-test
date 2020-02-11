Param(
    [parameter(mandatory=$true)][int] $ProcessId,
    [parameter(mandatory=$true)][int] $WaitSeconds,
    [parameter(mandatory=$true)][System.IO.DirectoryInfo] $SourceDirectory,
    [parameter(mandatory=$true)][System.IO.DirectoryInfo] $DestinationDirectory,
    [parameter(mandatory=$true)][ValidateSet("x32", "x64")][string] $Platform,
    [parameter(mandatory=$true)][string] $UpdateScript,
    [parameter(mandatory=$true)][string] $ExecuteCommand,
    [parameter(mandatory=$false)][string] $ExecuteArgument

)
#$OutputEncoding='utf-8'
$ErrorActionPreference = "Stop"

Write-Output "ProcessId: $ProcessId"
Write-Output "WaitSeconds: $WaitSeconds"
Write-Output "SourceDirectory: $SourceDirectory"
Write-Output "DestinationDirectory: $DestinationDirectory"
Write-Output "Platform: $Platform"
Write-Output "UpdateScript: $UpdateScript"
Write-Output "ExecuteCommand: $ExecuteCommand"
Write-Output "ExecuteArgument: $ExecuteArgument"

if ($ProcessId -ne 0 ) {
    Write-Output "wait process: $ProcessId ..."
    try {
        Wait-Process -Id $ProcessId -Timeout $WaitSeconds
        Write-Output "exited process: $ProcessId !"
    } catch {
        Write-Output $Error
        Write-Output "ignore process"
    }
}

Write-Output "$SourceDirectory -> $DestinationDirectory"
Copy-Item -Path ($SourceDirectory.FullName + "/*") -Destination $DestinationDirectory.FullName -Recurse -Force

if( Test-Path -Path $UpdateScript ) {
    Write-Output "execute script: $UpdateScript"
    Invoke-Expression "$UpdateScript -DestinationDirectory ""$DestinationDirectory"" -Platform $Platform "
}

Start-Process -FilePath $ExecuteCommand -ArgumentList $ExecuteArgument

Read-Host "Enter ..."
