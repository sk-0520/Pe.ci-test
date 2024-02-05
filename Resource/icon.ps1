Param(
	[string] $FirstInput = '',
	[switch] $BatchMode
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$iconDirPath = Join-Path -Path $PSScriptRoot -ChildPath 'Icon'
$workDirPath = Join-Path -Path $iconDirPath -ChildPath '@work'

$exeIncspace = if ($env:INKSCAPE) {
	$env:INKSCAPE
} else {
	[Environment]::ExpandEnvironmentVariables('C:\Program Files\Inkscape\bin\inkscape.exe')
}
$exeImageMagic = if ($env:IMAGEMAGIC) {
	$env:IMAGEMAGIC
} else {
	[Environment]::ExpandEnvironmentVariables('C:\Applications\ImageMagick\convert.exe')
}

$appIcons = @(
	@{
		name = 'App-release'
		color = '#ffffff'
	}
	@{
		name = 'App-debug'
		color = '#ff8888'
	}
	@{
		name = 'App-beta'
		color = '#8888ff'
	}
	@{
		name = 'App-plugin'
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


function New-Png {
	[CmdletBinding(SupportsShouldProcess)]
	Param(
		[Parameter(mandatory = $true)][string] $SvgPath
	)

	$pngBasePath = Join-Path -Path (Split-Path -Parent $SvgPath) -ChildPath ([System.IO.Path]::GetFileNameWithoutExtension($SvgPath))
	Write-Information "[SRC] $SvgPath"
	foreach ($size in $iconSize) {
		$pngPath = "${pngBasePath}_${size}.png"
		Write-Information "   -> $pngPath"

		$incspaceArgumentList = @(
			'--export-dpi=96',
			"--export-width=$size",
			"--export-height=$size",
			'--export-overwrite',
			#"--without-gui", これ入れると動かん
			'--export-type=png',
			"--export-filename=`"$pngPath`"",
			"`"$SvgPath`""
		)

		if ($PSCmdlet.ShouldProcess('SvgPath', '出力')) {
			$incspaceProcess = Start-Process -FilePath $exeIncspace -ArgumentList $incspaceArgumentList -WindowStyle Hidden -Wait -PassThru
			if ($incspaceProcess.ExitCode -ne 0) {
				throw $exeIncspace + ':' + $incspaceProcess.ExitCode
			}
		} else {
			Write-Verbose "Start-Process -FilePath $exeIncspace -ArgumentList $incspaceArgumentList -WindowStyle Hidden -Wait -PassThru"
		}
	}
}

function New-Icon {
	[CmdletBinding(SupportsShouldProcess)]
	Param(
		[Parameter(mandatory = $true)][string] $DirectoryPath,
		[Parameter(mandatory = $true)][string] $PngPattern,
		[Parameter(mandatory = $true)][string] $OutputPath
	)

	Write-Information "$DirectoryPath $PngPattern"

	$inputFiles = (
		Get-ChildItem -Path $DirectoryPath -Filter $PngPattern -File |
			Select-Object -ExpandProperty FullName |
			Sort-Object { [regex]::Replace($_, '\d+', { $args[0].Value.PadLeft(20) }) }
	)

	$imageMagicArgumentList = @()
	$imageMagicArgumentList += $inputFiles
	$imageMagicArgumentList += $OutputPath

	if ($PSCmdlet.ShouldProcess('OutputPath', '出力')) {
		$imageMagicProcess = Start-Process -File $exeImageMagic -ArgumentList $imageMagicArgumentList -WindowStyle Hidden -Wait -PassThru
		if ($imageMagicProcess.ExitCode -ne 0) {
			throw $exeImageMagic + ':' + $imageMagicProcess.ExitCode
		}
	} else {
		Write-Verbose "Start-Process -File $exeImageMagic -ArgumentList $imageMagicArgumentList -WindowStyle Hidden -Wait -PassThru"
	}
}


while ($true) {
	Write-Information '1: Pe: SVG -> PNG'
	Write-Information '2: Pe: PNG -> ICO'
	Write-Information 'x: 終了'
	if ($FirstInput) {
		$inputValue = $FirstInput
		$FirstInput = ''
	} else {
		$inputValue = Read-Host '処理'
	}
	try {
		switch ($inputValue) {
			'1' {
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
						$node.Attributes['style'].Value = $style -replace 'stop-color:(#[0-9a-f]{6})', "stop-color:$($appIcon.color)"
					}
					$savePath = Join-Path -Path $workDirPath -ChildPath "$($appIcon.name).svg"
					$appXml.Save($savePath)
					$savedPaths += $savePath
				}
				foreach ($savePath in $savedPaths) {
					New-Png -SvgPath $savePath
				}
			}
			'2' {
				foreach ($appIcon in $appIcons) {
					$pattern = "$($appIcon.name)_*.png"
					$outputPath = Join-Path -Path $iconDirPath -ChildPath "$($appIcon.name).ico"
					New-Icon -DirectoryPath $workDirPath -PngPattern $pattern -OutputPath $outputPath
				}
			}
			'x' {
				exit 0
			}
			default {
				Write-Information "[$inputValue] は未定義"
			}
		}
	} catch {
		Write-Error $Error[0]
	}
	Write-Information ''
	if ($BatchMode) {
		exit 0
	}
}


