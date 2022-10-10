# 破棄予定
Param(
	[Parameter(mandatory = $true)][ValidateSet("bitbucket")][string] $TargetDownloader,
	[Parameter(mandatory = $true)][ValidateSet("bitbucket")][string] $TargetRepository,
	[Parameter(mandatory = $true)][ValidateSet('zip', '7z', 'tar')][string] $MainArchive,
	[Parameter(mandatory = $true)][ValidateSet('zip', '7z', 'tar')][string] $DefaultArchive,
	[Parameter(mandatory = $true)][string] $DeployApiDownloadUrl,
	[Parameter(mandatory = $true)][string] $DeployApiTagUrl,
	[Parameter(mandatory = $true)][string] $DeployAccount,
	[Parameter(mandatory = $true)][string] $DeployPassword
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'version.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path $currentDirPath $scriptFileName
	. $scriptFilePath
}
$rootDirPath = Split-Path -Parent $currentDirPath
$outputDirectory = Join-Path $rootDirPath 'Output'

$securePassword = ConvertTo-SecureString $DeployPassword -AsPlainText -Force
$credential = New-Object System.Management.Automation.PSCredential($DeployAccount, $securePassword)

$archiveFiles = @(
	Get-ChildItem -Path $outputDirectory -Filter "*.$MainArchive"
	Get-ChildItem -Path $outputDirectory -Filter "*.$DefaultArchive"
) | Select-Object -Expand FullName
$releaseNoteFiles = Get-ChildItem -Path $outputDirectory -Filter "Pe*.html" | Select-Object -Expand FullName
$updateFiles = Get-ChildItem -Path $outputDirectory -Filter "update*.json" | Select-Object -Expand FullName

$version = GetAppVersion
$versionFiles = $releaseNoteFiles + $updateFiles + (Get-ChildItem -Path $outputDirectory -Filter "Pe.Plugins.Reference.*.$DefaultArchive" | Select-Object -Expand FullName)
$versionArchiveFile = Join-Path $outputDirectory (ConvertFileName 'Archive' $version '' 'zip')

Compress-Archive -Force -Path $versionFiles -DestinationPath $versionArchiveFile

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

# ダウンローダーへ送信
switch ($TargetRepository) {
	'bitbucket' {
		foreach ($archiveFile in $archiveFiles) {
			UploadFile $archiveFile
		}
		foreach($releaseNoteFile in $releaseNoteFiles) {
			UploadFile $releaseNoteFile
		}
		foreach($updateFile in $updateFiles) {
			UploadFile $updateFile
		}
		UploadFile $versionArchiveFile
		#UploadFile $releaseNoteFile
		#UploadFile $updateFile

		#Copy-Item -Path $updateFile -Destination $currentUpdateFile
		#UploadFile $currentUpdateFile
	}
}

# タグ付け
switch ($TargetRepository) {
	'bitbucket' {
		$bitbucketTagApiFile = Join-Path $outputDirectory 'bitbucket-tag.json'
		Write-Output "Post: $DeployApiTagUrl"
		Write-Output "File: $bitbucketTagApiFile"

		# 素直実装だと全然動かない悲しみ
		# Invoke-RestMethod `
		#     -Credential $credential `
		#     -Method Post `
		#     -Uri $DeployApiTagUrl `
		#     -ContentType "Content-Type: application/json" `
		#     -InFile $bitbucketTagApiFile
		$base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f $DeployAccount, $DeployPassword)))
		$headers = @{
			Authorization  = ("Basic {0}" -f $base64AuthInfo)
			"Content-type" = "application/json"
		}
		Invoke-RestMethod `
			-Headers $headers `
			-Method Post `
			-Uri $DeployApiTagUrl `
			-InFile $bitbucketTagApiFile
	}
}
