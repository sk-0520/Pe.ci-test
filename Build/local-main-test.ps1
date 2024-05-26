Param(
	[ValidateSet('x86', 'x64')][string] $Platform = 'x64',
	[ValidateSet('Debug', 'Release')][string] $Configuration = 'Debug',
	[string[]] $Project,
	[switch] $SuppressOpen
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Project"

$maxCount = 30

$testDirs = @()
$testDirs += Get-TestProjectDirectory -Kind main
$testDirs += Get-TestProjectDirectory -Kind plugins

if ($null -eq $Project -or $Project.Count -eq 0) {
	$testDirs
	exit
}
if ($Project.Count -eq 1 -and $Project[0] -eq '*') {
	$Project = $testDirs | Select-Object -ExpandProperty Name
}

$targetProjectDirs = @()

foreach ($pj in $Project) {
	$exists = $false
	foreach ($testDir in $testDirs) {
		if ($testDir.Name -eq $pj) {
			$exists = $true
			$targetProjectDirs += $testDir
		}
	}
	if (!$exists) {
		throw "プロジェクト指定不備: $pj"
	}
}


$rootDir = Get-RootDirectory
$baseDir = Join-Path -Path $rootDir -ChildPath '_coverage' | Join-Path -ChildPath 'main'
$workBaseDir = Join-Path -Path $baseDir -ChildPath (Get-Date -Format 'yyyy-MM-dd_HHmmss')
if (!(Test-Path $workBaseDir)) {
	New-Item $workBaseDir -ItemType Directory
}
$resultBaseDir = Join-Path -Path $workBaseDir -ChildPath 'result'
New-Item $resultBaseDir -ItemType Directory
$codeCoverageDir = Join-Path -Path $workBaseDir -ChildPath 'codecoverage'

$testResultFiles = @()
# テスト実行
foreach ($dir in $targetProjectDirs) {
	$testResultFileName = 'coverage.cobertura.xml'
	Push-Location -LiteralPath $dir.FullName
	try {
		dotnet test /p:Platform=$Platform --runtime win-$Platform --configuration Debug --collect:"XPlat Code Coverage"
		if (-not $?) {
			throw "build error: $dir"
		}

		# 恐らく最新の結果ファイルを取得
		$testResultFile = Get-ChildItem -LiteralPath 'TestResults' -Filter $testResultFileName -Recurse -File |
			Sort-Object LastWriteTime -Descending |
			Select-Object -First 1

		# 作業ディレクトリにお引越し
		$testDestPath = Join-Path -Path $resultBaseDir -ChildPath "$($dir.Name)_$($testResultFile.Name)"
		Move-Item -LiteralPath $testResultFile.FullName -Destination $testDestPath
		$testResultFiles += $testDestPath
	} finally {
		Pop-Location
	}
}

# カバレッジ用HTMLの生成

$sources = @(
	Get-SourceDirectory -Kind main
	Join-Path -Path (Get-SourceDirectory -Kind main) -ChildPath Pe.Bridge
)

$assemblyfilters = @()
foreach($dir in $targetProjectDirs) {
	$assemblyfilters += "+$($dir.Name.SubString(0, $dir.Name.Length - '.Test'.Length))"
}

$reportgenerator = Join-Path -Path $rootDir -ChildPath '_tools' | Join-Path -ChildPath 'reportgenerator.exe'
& $reportgenerator `
	-reports:$($testResultFiles -join ';') `
	-sourcedirs:$($sources -join ';') `
	-targetdir:"$codeCoverageDir" `
	-assemblyfilters:"$($assemblyfilters -join ';')" `
	-classfilters:"-*.Properties.Resources" `
	-reporttypes:Html

if (! $SuppressOpen) {
	$html = Join-Path -Path $codeCoverageDir -ChildPath 'index.html'
	Start-Process $html
}


$dirs = @(Get-ChildItem -LiteralPath $baseDir -Directory | Sort-Object BaseName -Descending)
for($i = $maxCount; $i -lt $dirs.Count; $i++) {
	$dir = $dirs[$i]
	Remove-Item -LiteralPath $dir.FullName -Force -Recurse
}
