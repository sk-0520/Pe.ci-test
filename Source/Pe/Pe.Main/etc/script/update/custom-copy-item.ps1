
Param(
	[Parameter(mandatory = $true)][string] $SourceDirectoryPath, # 絶対パス
	[Parameter(mandatory = $true)][string] $DestinationDirectoryPath, # 絶対パス
	[ValidateSet('animation', 'output', 'none')][string] $ProgressType = 'none', # 進捗状況
	[switch] $CreateDirectory # 新規ディレクトリを作成するか
)
$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$srcChildItems = Get-ChildItem -LiteralPath $SourceDirectoryPath -Recurse -Force
if($srcChildItems.Count -eq 0){
	return
}

$destRootDirPath = if ( $CreateDirectory ) {
	Join-Path -Path $DestinationDirectoryPath -ChildPath ([System.IO.Path]::GetFileName($SourceDirectoryPath))
} else {
	Join-Path -Path $DestinationDirectoryPath -ChildPath ''
}

$srcDirs = $srcChildItems | Where-Object { $_.PSIsContainer }
$srcFiles = $srcChildItems | Where-Object { !$_.PSIsContainer }

$processCount = 0
$totalProcessCount = $srcChildItems.Count

$progressAnimation = 1
$progressOutput = 2
$progressNone = 0
$progress = switch($ProgressType) {
	'animation' { $progressAnimation }
	'output' { $progressOutput }
	default { $progressNone }
}


foreach($srcDir in $srcDirs) {
	$processCount += 1
	$percent = ($processCount / $totalProcessCount) * 100
	$destPath = $srcDir.FullName.Replace($SourceDirectoryPath, $destRootDirPath)
	switch ($progress) {
		$progressAnimation {
			Write-Progress -Activity "COPY" -CurrentOperation "mkdir $destPath" -PercentComplete $percent
		}
		$progressOutput {
			#Write-Host "[$percent%] mkdir $destPath"
			Write-Host "[$('{0,6:##0.00}' -f $percent)%] mkdir $destPath"
		}
	}

	New-Item -Path "$destPath" -ItemType Directory -Force | Out-Null
}
foreach($srcFile in $srcFiles) {
	$processCount += 1
	$percent = ($processCount / $totalProcessCount) * 100
	$destPath = $srcFile.FullName.Replace($SourceDirectoryPath, $destRootDirPath)
	$srcPath = $srcFile.FullName
	switch ($progress) {
		$progressAnimation {
			Write-Progress -Activity "COPY" -CurrentOperation "$srcFile -> $destPath" -PercentComplete $percent
		}
		$progressOutput {
			Write-Host "[$('{0,6:##0.00}' -f $percent)%] $srcPath -> $destPath"
		}
	}

	Copy-Item -Path "$srcPath" -Destination "$destPath" -Force | Out-Null
}

if($progress -eq $progressAnimation) {
	Write-Progress -Activity "COPY" -Completed
}
