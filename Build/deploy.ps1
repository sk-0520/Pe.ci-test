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
echo 1
if(TestAliasExists "curl") {
    echo 2
    Remove-Item curl
}
echo 3
SetCommand 'curl' 'BUILD_CURL_PATH' "%WINDIR%\System32"
echo 4

$archiveFiles = Get-ChildItem -Path $DeployRootDirectory -Filter "*.zip" | Select-Object -Expand FullName
$updateFile = Join-Path $DeployRootDirectory 'update.json'

switch ($TargetRepository) {
    'bitbucket' {
        foreach($archiveFile in $archiveFiles) {
            curl --user ${DeployAccount}:${DeployPassword} -X POST $DeployApiDownloadUrl -F files=@$archiveFile
        }
        curl --user ${DeployAccount}:${DeployPassword} -X POST $DeployApiDownloadUrl -F files=@$updateFile

        $bitbucketTagApiFile = Join-Path $DeployRootDirectory 'bitbucket-tag.json'
        echo (Get-Content $bitbucketTagApiFile)
        echo $DeployApiTagUrl
        curl --user ${DeployAccount}:${DeployPassword} -H 'Content-Type: application/json' -X POST $DeployApiTagUrl -d @$bitbucketTagApiFile
    }
}
