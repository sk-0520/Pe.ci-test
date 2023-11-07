Param(
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'version.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path -Path $currentDirPath -ChildPath $scriptFileName
	. $scriptFilePath
}

$verionEndPoint = [uri]"https://peserver.gq/api/application/version/update"
$extension = '7z'
$response = Invoke-WebRequest -Uri $verionEndPoint -Method Get
$latestResult = ConvertFrom-Json ([System.Text.Encoding]::UTF8.GetString($response.Content))
$latestItem = $latestResult.items[0]
$latestVersion = $latestItem.version

$workDirectoryPath = ".\work"
$outputDirectoryPath = ".\package"
$gitHubReleaseUrl = "https://github.com/sk-0520/Pe/releases/download"
$7zipPath = "%PROGRAMFILES%\7-Zip\7z.exe"

$_workDirectoryPath = [System.IO.Path]::GetFullPath([System.Environment]::ExpandEnvironmentVariables($workDirectoryPath))
$_outputDirectoryPath = [System.IO.Path]::GetFullPath([System.Environment]::ExpandEnvironmentVariables($outputDirectoryPath))
$_7zipPath = [System.Environment]::ExpandEnvironmentVariables($7zipPath)

Write-Verbose "[`$WorkDirectoryPath] $_workDirectoryPath"
Write-Verbose "[`$OutputDirectoryPath] $_outputDirectoryPath"
Write-Verbose "[`$7zipPath] $_7zipPath"

Write-Host "Version = $latestVersion"

$versions = @{
	"path" = ConvertVersion $latestVersion "."
	"file" = "Pe_" + (ConvertVersion $latestVersion "-")
}

$platforms = @(
	"x86",
	"x64"
)

$archiveFiles = @()

if (!(Test-Path -Path $_workDirectoryPath)) {
	New-Item -Path $_workDirectoryPath -ItemType Directory
}

foreach ($platform in $platforms) {
	$url = $gitHubReleaseUrl.Trim("/") + "/" + $versions["path"] + "/" + $versions["file"] + "_${platform}.${extension}";
	$fileName = $versions["file"] + "_${platform}.${extension}"

	Write-Verbose "[$platform] $url $fileName"

	$filePath = Join-Path -Path $_workDirectoryPath -ChildPath $fileName
	Invoke-WebRequest -Uri $url -Method Get -OutFile $filePath
	$archiveFiles += $filePath
}

if(!(Test-Path -Path $_outputDirectoryPath)) {
	New-Item -Path $_outputDirectoryPath -ItemType Directory
}
Remove-Item -Path (Join-Path -Path $_outputDirectoryPath -ChildPath "*") -Force -Recurse

foreach($filePath in $archiveFiles) {
	Write-Verbose "[TARGET] $filePath"

	$parentDirPath = [System.IO.Path]::GetDirectoryName($filePath)
	$baseFileName = [System.IO.Path]::GetFileNameWithoutExtension($filePath)
	$sfxName = $baseFileName + '.exe'
	$outputPath = [System.IO.Path]::Combine($_outputDirectoryPath, $sfxName)

	try {
		Push-Location $parentDirPath
		& "${_7zipPath}" x -y "-o$baseFileName" "$filePath"
		& "${_7zipPath}" a $outputPath -sfx -m0=lzma2 -mx=9 -mfb=64 -md=64m -ms=on $baseFileName\* -r
	} finally {
		Pop-Location
	}

}
