Param(
    [parameter(mandatory = $true)][ValidateSet("bitbucket")][string] $TargetRepository,
    [parameter(mandatory = $true)][string] $DeployRootDirectory,
    [parameter(mandatory = $true)][string] $DeployApiDownloadUrl,
    [parameter(mandatory = $true)][string] $DeployApiTagUrl,
    [parameter(mandatory = $true)][string] $DeployAccount,
    [parameter(mandatory = $true)][string] $DeployPassword
)
$ErrorActionPreference = 'Stop'
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
    'command.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
    $scriptFilePath = Join-Path $currentDirPath $scriptFileName
    . $scriptFilePath
}


# Invoke-RestMethod しんどい。。。
if(TestAliasExists curl) {
    Remove-Item curl
}

$archiveItems = Get-ChildItem -Path $DeployRootDirectory -Filter "*.zip" | Select-Object -Expand FullName
foreach($archiveItem in $archiveItems) {
    curl --user ${DeployAccount}:${DeployPassword} -X POST $DeployApiDownloadUrl -F files=@$archiveItem
}
