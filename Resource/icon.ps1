Param(
	[string] $FirstInput = '',
	[switch] $BatchMode
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootDirPath = Split-Path -Parent $currentDirPath
$iconDirPath = Join-Path -Path $currentDirPath -ChildPath 'Icon'
$workDirPath = Join-Path -Path $iconDirPath -ChildPath '@work'

$exeIncspace = if ($env:INKSCAPE) { $env:INKSCAPE } else { [Environment]::ExpandEnvironmentVariables('C:\Program Files\Inkscape\bin\inkscape.exe') }
$exeImageMagic = if ($env:IMAGEMAGIC) { $env:IMAGEMAGIC } else { [Environment]::ExpandEnvironmentVariables('C:\Applications\ImageMagick\convert.exe') }

$appIcons = @(
	@{
		name  = 'App-release'
		color = '#ffffff'
	}
	@{
		name  = 'App-debug'
		color = '#ff8888'
	}
	@{
		name  = 'App-beta'
		color = '#8888ff'
	}
	@{
		name  = 'App-plugin'
		color = '#88ff88'
	}
)
$iconSize = @(
	16, 18, 20, 24, 28
	32, 40
	48, 56, 60, 64
	72, 80, 84, 96, 112, 128, 160
	192, 224
	256, 320, 384, 448, 512
)




function ConvertAppSvgToPng() {
	# App.svg から release(そのまま), debug, beta を生成
	$appPath = Join-Path -Path $iconDirPath -ChildPath 'App.svg'
	$appXml = [xml](Get-Content $appPath -Raw -Encoding UTF8)

	$nodes = @(
		$appXml.SelectSingleNode('//*[@id="backgroundLinearGradientStart"]')
		$appXml.SelectSingleNode('//*[@id="backgroundLinearGradientEnd"]')
	)

	New-Item -Path $workDirPath -ItemType Directory -Force | Out-Null

	$savedPaths = @()
	foreach ($appIcon in $appIcons) {
		foreach ($node in $nodes) {
			$style = $node.Attributes['style'].Value
			$node.Attributes['style'].Value = $style -replace "stop-color:(#[0-9a-f]{6})", "stop-color:$($appIcon.color)"
		}
		$savePath = Join-Path -Path $workDirPath -ChildPath "$($appIcon.name).svg"
		$appXml.Save($savePath)
		$savedPaths += $savePath
	}
	foreach ($savePath in $savedPaths) {
		ConvertSvgToPng "$savePath"
	}
}

function PackAppIcon {
	foreach ($appIcon in $appIcons) {
		$pattern = "$($appIcon.name)_*.png"
		$outputPath = Join-Path -Path $iconDirPath -ChildPath "$($appIcon.name).ico"
		PackIcon $workDirPath $pattern $outputPath
	}
}

function ConvertSvgToPng([string] $srcSvgPath) {
	$pngBasePath = Join-Path (Split-Path -Parent $srcSvgPath) ([System.IO.Path]::GetFileNameWithoutExtension($srcSvgPath))
	Write-Host "[SRC] $srcSvgPath";
	foreach ($size in $iconSize) {
		$pngPath = "${pngBasePath}_${size}.png"
		Write-Host "   -> $pngPath"

		$incspaceArgumentList = @(
			"--export-dpi=96",
			"--export-width=$size",
			"--export-height=$size",
			"--export-overwrite",
			#"--without-gui", これ入れると動かん
			"--export-type=png",
			"--export-filename=`"$pngPath`"",
			"`"$srcSvgPath`""
		)

		$incspaceProcess = Start-Process -FilePath $exeIncspace -ArgumentList $incspaceArgumentList -WindowStyle Hidden -Wait -PassThru
		if($incspaceProcess.ExitCode -ne 0) {
			throw $exeIncspace + ":" + $incspaceProcess.ExitCode
		}
	}
}

function PackIcon([string] $directoryPath, [string] $pngPattern, [string] $outputPath) {
	Write-Host "$directoryPath $pngPattern"

	$inputFiles = (
		Get-ChildItem -Path $directoryPath -Filter $pngPattern -File `
		| Select-Object -ExpandProperty FullName `
		| Sort-Object { [regex]::Replace($_, '\d+', { $args[0].Value.PadLeft(20) }) }
	)

	$imageMagicArgumentList = @()
	$imageMagicArgumentList += $inputFiles
	$imageMagicArgumentList += $outputPath

	$imageMagicProcess = Start-Process -File $exeImageMagic -ArgumentList $imageMagicArgumentList -WindowStyle Hidden -Wait -PassThru
	if($imageMagicProcess.ExitCode -ne 0) {
		throw $exeImageMagic + ":" + $imageMagicProcess.ExitCode
	}
}

function MoveAppIcon {
	$mainDir = Join-Path -Path $rootDirPath -ChildPath 'Source\Pe\Pe.Main\Resources\Icon'
	foreach ($appIcon in $appIcons) {
		$srcPath = Join-Path -Path $currentDirPath -ChildPath "$($appIcon.name).ico"
		$dstPath = Join-Path -Path $mainDir -ChildPath "$($appIcon.name).ico"
		Write-Host "[COPY] $srcPath -> $dstPath"
		Copy-Item -Path $srcPath -Destination $dstPath
	}

}

while ($true) {
	Write-Host "1: Pe: SVG -> PNG"
	Write-Host "2: Pe: PNG -> ICO"
	Write-Host "x: 終了"
	if ($FirstInput) {
		$inputValue = $FirstInput
		$FirstInput = ''
	}
 else {
		$inputValue = Read-Host "処理"
	}
	try {
		switch ($inputValue) {
			'1' {
				ConvertAppSvgToPng
			}
			'2' {
				PackAppIcon
			}
			'x' {
				exit 0;
			}
			default {
				Write-Host "[$inputValue] は未定義"
			}
		}
	}
 catch {
		Write-Host $Error[0] -ForegroundColor Red -BackgroundColor Black
	}
	Write-Host ''
	if ($BatchMode) {
		exit 0
	}
}


