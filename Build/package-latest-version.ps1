Param(
	[Parameter(mandatory = $true)][ValidateSet('x86', 'x64')][string] $Platform
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Version"


$verionEndPoint = [uri]'https://peserver.site/api/application/version/update'
$extension = '7z'
$response = Invoke-WebRequest -Uri $verionEndPoint -Method Get
$latestResult = ConvertFrom-Json ([System.Text.Encoding]::UTF8.GetString($response.Content))
$latestItem = $latestResult.items[0]
$latestVersion = $latestItem.version

$workDirectoryPath = '.\work'
$outputDirectoryPath = '.\package'
$gitHubReleaseUrl = 'https://github.com/sk-0520/Pe/releases/download'
$7zipPath = '%PROGRAMFILES%\7-Zip\7z.exe'

$_workDirectoryPath = [System.IO.Path]::GetFullPath([System.Environment]::ExpandEnvironmentVariables($workDirectoryPath))
$_outputDirectoryPath = [System.IO.Path]::GetFullPath([System.Environment]::ExpandEnvironmentVariables($outputDirectoryPath))
$_7zipPath = [System.Environment]::ExpandEnvironmentVariables($7zipPath)

Write-Verbose "WorkDirectoryPath: $_workDirectoryPath"
Write-Verbose "OutputDirectoryPath: $_outputDirectoryPath"
Write-Verbose "7zipPath: $_7zipPath"

Write-Verbose "Version: $latestVersion"

if (!(Test-Path -Path $_workDirectoryPath)) {
	New-Item -Path $_workDirectoryPath -ItemType Directory
}


$versions = @{
	'path' = Convert-Version -Version $latestVersion -Separator '.'
	'file' = 'Pe_' + (Convert-Version -Version $latestVersion -Separator '-')
}

$archiveFilePath = ''

$url = $gitHubReleaseUrl.Trim('/') + '/' + $versions['path'] + '/' + "Pe_${platform}.${extension}"
$fileName = $versions['file'] + ".${extension}"

Write-Information "[$Platform] $url $fileName"

$filePath = Join-Path -Path $_workDirectoryPath -ChildPath $fileName
Invoke-WebRequest -Uri $url -Method Get -OutFile $filePath
$archiveFilePath = $filePath

if (!(Test-Path -Path $_outputDirectoryPath)) {
	New-Item -Path $_outputDirectoryPath -ItemType Directory
}
Remove-Item -Path (Join-Path -Path $_outputDirectoryPath -ChildPath '*') -Force -Recurse


Write-Information "[TARGET] $archiveFilePath"

$parentDirPath = [System.IO.Path]::GetDirectoryName($archiveFilePath)
$baseFileName = [System.IO.Path]::GetFileNameWithoutExtension($archiveFilePath)
$sfxName = $baseFileName + '_' + $Platform + '.exe'
$outputPath = [System.IO.Path]::Combine($_outputDirectoryPath, $sfxName)

try {
	Push-Location $parentDirPath
	& "${_7zipPath}" x -y "-o$baseFileName" "$archiveFilePath"
	& "${_7zipPath}" a $outputPath -sfx -m0=lzma2 -mx=9 -mfb=64 -md=64m -ms=on $baseFileName\* -r
} finally {
	Pop-Location
}
