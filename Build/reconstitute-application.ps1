Param(
	[Parameter(mandatory = $true)][ValidateSet('x86', 'x64')][string] $Platform,
	[Parameter(mandatory = $true)][ValidateSet('zip', '7z', 'tar')][string] $Archive,
	[switch] $ProductMode,
	[string] $BuildType,
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
	$scriptFilePath = Join-Path -Path $currentDirPath -ChildPath $scriptFileName
	. $scriptFilePath
}
$rootDirectory = Split-Path -Path $currentDirPath -Parent

#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------

$inputItems = @{
	buildTools = Join-Path -Path $InputDirectory -ChildPath 'buildtools'
	sql        = Join-Path -Path $InputDirectory -ChildPath 'sql' | Join-Path -ChildPath 'sql.sqlite3'
	help       = Join-Path -Path $InputDirectory -ChildPath 'help'
	boot       = Join-Path -Path $InputDirectory -ChildPath 'boot'
	main       = Join-Path -Path $InputDirectory -ChildPath 'main-bin'
}

# 出力ディレクトリになんかあっても面倒なので更地にしてからあれこれする
if ( Test-Path -Path $OutputDirectory ) {
	Remove-Item -Path $OutputDirectory -Force -Recurse
}
New-Item -Path $OutputDirectory -ItemType Directory


# boot の移行
Copy-Item -Path (Join-Path -Path $inputItems.boot -ChildPath '*') -Destination $OutputDirectory -Recurse


# main の移行
$outputMainDir = Join-Path -Path $OutputDirectory -ChildPath 'bin'
New-Item -Path $outputMainDir -ItemType Directory
Copy-Item -Path (Join-Path -Path $inputItems.main -ChildPath '*') -Destination $outputMainDir -Recurse

# main 内の各ディレクトリを上に移す
$mainSubDirNames = @('etc', 'doc', 'bat')
foreach ($mainSubDirName in $mainSubDirNames) {
	$mainSubDir = Join-Path -Path $outputMainDir -ChildPath $mainSubDirName
	Move-Item -Path $mainSubDir -Destination $OutputDirectory
}

# bin/lib にVC++ 再頒布可能パッケージ埋め込み
$srcLibDir = Join-Path -Path $rootDirectory -ChildPath 'Resource' | Join-Path -ChildPath 'Library'
$dstLibDir = Join-Path -Path $outputMainDir -ChildPath 'lib'
Move-Item -Path $srcLibDir -Destination $dstLibDir

# etc/appsettings.*.json の整理
$outputEtcDir = Join-Path -Path $OutputDirectory -ChildPath 'etc'
if ($BuildType -ne "BETA") {
	Remove-Item -Path (Join-Path -Path $outputEtcDir -ChildPath "appsettings.beta.json")
}
Remove-Item -Path (Join-Path -Path $outputEtcDir -ChildPath "@appsettings.debug.json")

# etc/sql の各 SQL をまとめたものに置き換え
$outputSqlDir = Join-Path -Path $outputEtcDir -ChildPath 'sql'
Get-ChildItem -Path $outputSqlDir -Directory `
| Remove-Item -Force -Recurse
Move-Item -Path $inputItems.sql -Destination $outputSqlDir

# doc/help を生成済みヘルプに置き換え(ダミーのindex.htmlがあるので上書きOK)
$helpRootDir = Join-Path -Path $OutputDirectory -ChildPath 'doc'
Move-Item $inputItems.help -Destination $helpRootDir -Force


# プラットフォームに合わないディレクトリを破棄(機械的にやってもいいけどちょっと自信ないのです)
$unsupportPlatform = switch ($Platform) {
	'x64' { 'x86' }
	'x86' { 'x64' }
	Default { throw "error: $Platform" }
}
$unsupportTargets = @(
	Join-Path -Path $outputMainDir -ChildPath $unsupportPlatform
	Join-Path -Path $dstLibDir -ChildPath 'Redist.MSVC.CRT' | Join-Path -ChildPath $unsupportPlatform
)
foreach ($unsupportTarget in $unsupportTargets) {
	Write-Output "Remove: $unsupportTarget"
	Remove-Item $unsupportTarget -Recurse -Force
}

