$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path


#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------


$dot = GetAppVersion
$hyphen = (ConvertVersion (GetAppVersion) '-')

"dot=$dot" >> $env:GITHUB_OUTPUT
"hyphen=$hyphen" >> $env:GITHUB_OUTPUT

