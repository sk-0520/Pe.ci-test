$ErrorActionPreference = 'Stop'
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
    'command.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
    $scriptFilePath = Join-Path $currentDirPath $scriptFileName
    . $scriptFilePath
}
Get-ChildItem env:
echo 1
if(TestAliasExists curl) {
    echo 2
    Remove-Item  alias:curl
}
SetCommand 'curl' 'BUILD_CURL_PATH' "%WINDIR%\System32"

echo 3
curl --version
echo 4
