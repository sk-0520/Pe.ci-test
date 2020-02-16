Param(
    [parameter(mandatory = $true)][ValidateSet("bitbucket")][string] $TargetRepository,
    [parameter(mandatory = $true)][string] $DeployRootDirectory,
    [parameter(mandatory = $true)][string] $DeployApiDownloadUrl,
    [parameter(mandatory = $true)][string] $DeployApiTagUrl,
    [parameter(mandatory = $true)][string] $DeployAccount,
    [parameter(mandatory = $true)][string] $DeployPassword
)
$ErrorActionPreference = 'Stop'

$securePassword = ConvertTo-SecureString $DeployPassword -AsPlainText -Force
$credential = New-Object System.Management.Automation.PSCredential($DeployAccount, $securePassword)

$archiveFiles = Get-ChildItem -Path $DeployRootDirectory -Filter "*.zip" | Select-Object -Expand FullName
$updateFile = Join-Path (Get-Location) (Join-Path $DeployRootDirectory 'update.json')

# https://stackoverflow.com/a/50255917
function UploadFile([string] $filePath) {
    $fileName = [System.IO.Path]::GetFileName($filePath)
    $fileBytes = [System.IO.File]::ReadAllBytes($filePath);

    $boundary = [System.Guid]::NewGuid().ToString();
    $CRLF = [byte[]]@(0x0d, 0x0a);

    $httpPath = $filePath + '.http'
    $fileStream = New-Object System.IO.FileStream($httpPath), Create
    $binaryWriter = New-Object System.IO.BinaryWriter($fileStream)

    function WriteHttp([string] $value) {
        $binaryWriter.Write([Text.Encoding]::UTF8.GetBytes($value))
        $binaryWriter.Write($CRLF)
    }

    WriteHttp("--$boundary");
    WriteHttp("Content-Disposition: form-data; name=`"files`"; filename=`"$fileName`"")

    WriteHttp("Content-Type: application/octet-stream")
    $binaryWriter.Write($CRLF)

    $binaryWriter.Write($fileBytes)
    $binaryWriter.Write($CRLF)

    WriteHttp("--$boundary");
    $binaryWriter.Write($CRLF)

    $binaryWriter.Dispose()
    $fileStream.Dispose()

    Write-Output "Post: $DeployApiDownloadUrl"
    Write-Output "File: $filePath"
    Invoke-RestMethod `
        -Credential $credential `
        -Method Post `
        -Uri $DeployApiDownloadUrl `
        -ContentType "multipart/form-data; boundary=`"$boundary`"" `
        -InFile $httpPath
}

switch ($TargetRepository) {
    'bitbucket' {
        foreach($archiveFile in $archiveFiles) {
            UploadFile $archiveFile
        }
        UploadFile $updateFile

        $bitbucketTagApiFile = Join-Path $DeployRootDirectory 'bitbucket-tag.json'
        Write-Output "Post: $DeployApiTagUrl"
        Write-Output "File: $bitbucketTagApiFile"

        # 素直実装だと全然動かない悲しみ
        # Invoke-RestMethod `
        #     -Credential $credential `
        #     -Method Post `
        #     -Uri $DeployApiTagUrl `
        #     -ContentType "Content-Type: application/json" `
        #     -InFile $bitbucketTagApiFile
        #$base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f $DeployAccount, $DeployPassword)))
        # Invoke-RestMethod `
        #     -Headers @{
        #         Authorization=("Basic {0}" -f $base64AuthInfo)
        #         "Content-type"="application/json"
        #     }  `
        #     -Method Post `
        #     -Uri $DeployApiTagUrl `
        #     -InFile $bitbucketTagApiFile
    }
}
