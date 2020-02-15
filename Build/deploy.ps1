Param(
    [parameter(mandatory = $true)][ValidateSet("bitbucket")][string] $TargetRepository,
    [parameter(mandatory = $true)][string] $DeployRootDirectory,
    [parameter(mandatory = $true)][string] $DeployApiDownloadUrl,
    [parameter(mandatory = $true)][string] $DeployApiTagUrl,
    [parameter(mandatory = $true)][string] $DeployAccount,
    [parameter(mandatory = $true)][string] $DeployPassword
)
$ErrorActionPreference = 'Stop'

$password = ConvertTo-SecureString $DeployPassword -AsPlainText -Force
$credential = New-Object System.Management.Automation.PSCredential($DeployAccount, $password)

$archiveItems = Get-ChildItem -Path $DeployRootDirectory -Filter "*.zip" | Select-Object -Expand FullName
foreach($archiveItem in $archiveItems) {
    Invoke-RestMethod -Method Post -InFile $archiveItem -Uri $DeployApiDownloadUrl  -Credential $credential
}
