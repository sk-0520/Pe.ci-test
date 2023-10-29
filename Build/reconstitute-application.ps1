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

$inputItems = @{
	buildTools = Join-Path $InputDirectory -ChildPath 'buildtools'
	sql        = Join-Path $InputDirectory -ChildPath 'sql.sqlite3' | Join-Path -ChildPath 'sql.sqlite3'
	help       = Join-Path $InputDirectory -ChildPath 'help'
	boot       = Join-Path $InputDirectory -ChildPath 'boot'
	main       = Join-Path $InputDirectory -ChildPath 'main-bin'
}

# 出力ディレクトリになんかあっても面倒なので更地にしてからあれこれする
if ( Test-Path -Path $OutputDirectory ) {
	Remove-Item -Path $OutputDirectory -Force -Recurse
}
New-Item -Path $OutputDirectory -ItemType Directory


# boot の移行
Copy-Item -Path (Join-Path -Path $inputItems.boot -ChildPath '*') -Destination $OutputDirectory


# main の移行
$outputMainDir = Join-Path -Path $OutputDirectory -ChildPath 'bin'
New-Item -Path $outputMainDir -ItemType Directory
Copy-Item -Path (Join-Path -Path $inputItems.main -ChildPath '*') -Destination $outputMainDir


# main 内の各ディレクトリを上に移す
$mainSubDirs = @('etc', 'doc', 'bat')
foreach ($mainSubDir in $mainSubDirs) {
	$srcDir = Join-Path -Path $inputItems.main -ChildPath $mainSubDir
	Move-Item -Path $srcDir -Destination $OutputDirectory
}
# etc/sql の各 SQL をまとめたものに置き換え
$sqlRootDir = Join-Path -Path $OutputDirectory -ChildPath 'etc' | Join-Path -ChildPath 'sql'
Get-ChildItem -Path $sqlRootDir -Directory `
| Remove-Item -Force -Recurse
Move-Item $inputItems.sql -Destination $sqlRootDir
# doc/help を生成済みヘルプに置き換え
$helpRootDir = Join-Path -Path $OutputDirectory -ChildPath 'doc'
Move-Item $inputItems.help -Destination $helpRootDir

