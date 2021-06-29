# アーカイブを自己展開形式に変換する
# アーカイバによってはディレクトリ展開に失敗して Pe が必要ファイル読めんで死ぬねん
# ほぼほぼ vector 用のローカル実行処理。おれは勤労精神すごい
# (みたいな) .\local-7z2exe.ps1 -SourceFilePaths X:\10_download\Pe_X-XX-000_x64.7z,X:\10_download\Pe_X-XX-000_x86.7z
Param(
	[Parameter(mandatory = $true)][string[]] $SourceFilePaths
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'command.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path $currentDirPath $scriptFileName
	. $scriptFilePath
}
#$rootDirectory = Split-Path -Path $currentDirPath -Parent
# $7zS = Join-Path $currentDirPath '7z.sfx'
# $configPath = Join-Path $currentDirPath 'config.txt'

foreach($filePath in $SourceFilePaths) {
	try {
		$parentDirPath = [System.IO.Path]::GetDirectoryName($filePath)
		$baseFileName = [System.IO.Path]::GetFileNameWithoutExtension($filePath)
		$sfxName = $baseFileName + '.exe'
		$outputPath = [System.IO.Path]::Combine($parentDirPath, $sfxName)

		# 正直プロンプトでもいいと思うけど。。。
		Push-Location $parentDirPath
		7z x -y "-o$baseFileName" "$filePath"
		7z a $outputPath -sfx -m0=lzma2 -mx=9 -mfb=64 -md=64m -ms=on $baseFileName\* -r

		# x86 版が管理権限で動こうとするのでもうﾏｼﾞむりぃ, Pe としては考慮してない
		# -> そもそもただの展開なら x86 でいいんだけど権限に任せてプログラムディレクトリに飛ばされると後が怖いので処理しない、が実装は残しておきたい
		# $a = [System.IO.File]::ReadAllBytes($7zS)
		# $b = [System.IO.File]::ReadAllBytes($configPath)
		# $c = [System.IO.File]::ReadAllBytes($filePath)
		# Set-Content -Path $outputPath -Value $a -Encoding Byte
		# Add-Content -Path $outputPath -Value $b -Encoding Byte
		# Add-Content -Path $outputPath -Value $c -Encoding Byte
	} finally {
		Pop-Location
	}
}
