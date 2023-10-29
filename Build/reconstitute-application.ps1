Param(
	[Parameter(mandatory = $true)][ValidateSet('x86', 'x64')][string] $Platform,
	[Parameter(mandatory = $true)][ValidateSet('zip', '7z', 'tar')][string] $Archive,
	[switch] $ProductMode,
	[Parameter(mandatory = $true)][string] $InputDirectory,
	[Parameter(mandatory = $true)][string] $OutputDirectory
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'project.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path $currentDirPath $scriptFileName
	. $scriptFilePath
}

#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------

$inputDirectories = @{
	buildTools = Join-Path $InputDirectory 'buildtools'
	help       = Join-Path $InputDirectory 'help'
	boot       = Join-Path $InputDirectory 'boot'
	main       = Join-Path $InputDirectory 'main-bin'
}

# 出力ディレクトリになんかあっても面倒なので更地にしてからあれこれする
if ( Test-Path -Path $OutputDirectory ) {
	Remove-Item -Path $OutputDirectory -Force -Recurse
}
New-Item -Path $OutputDirectory -ItemType Directory


# boot の移行
Copy-Item -Path (Join-Path -Path $inputDirectories.boot -ChildPath '*') -Destination $OutputDirectory


# main の移行
$outputMainDir = Join-Path -Path $OutputDirectory -ChildPath 'bin'
New-Item -Path $outputMainDir -ItemType Directory
Copy-Item -Path (Join-Path -Path $inputDirectories.main -ChildPath '*') -Destination $outputMainDir


# main 内の各ディレクトリを上に移す
$mainSubDirs = @('etc', 'doc', 'bat')
foreach ($mainSubDir in $mainSubDirs) {
	$srcDir = Join-Path -Path $inputDirectories.main -ChildPath $mainSubDir
	Move-Item -Path $srcDir -Destination $outputMainDir
}
