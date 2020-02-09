Param(
    [parameter(mandatory=$true)][int] $ProcessId,
    [parameter(mandatory=$true)][int] $WaitSeconds,
    [parameter(mandatory=$true)][System.IO.DirectoryInfo] $SourceDirectory,
    [parameter(mandatory=$true)][System.IO.DirectoryInfo] $DestinationDirectory,
    [parameter(mandatory=$true)][ValidateSet("32", "64")][string] $ProcessSize,
    [parameter(mandatory=$true)][string] $UpdateScript,
    [parameter(mandatory=$true)][string] $ExecuteCommand,
    [parameter(mandatory=$true)][string[]] $ExecuteArguments

)
$OutputEncoding='utf-8'
$ErrorActionPreference = "Stop"

Write-Output "ProcessId: $ProcessId"
Write-Output "WaitSeconds: $WaitSeconds"
Write-Output "SourceDirectory: $SourceDirectory"
Write-Output "DestinationDirectory: $DestinationDirectory"
Write-Output "ProcessSize: $ProcessSize"
Write-Output "UpdateScript: $UpdateScript"
Write-Output "ExecuteCommand: $ExecuteCommand"
Write-Output "ExecuteArguments: $ExecuteArguments"

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

Copy-Item -Path ($SourceDirectory.FullName + "/*") -Destination $DestinationDirectory.FullName -Recurse -Force

Invoke-Expression "$UpdateScript"

Start-Process -FilePath $ExecuteCommand -ArgumentList $ExecuteArguments

Read-Host "Enter ..."
