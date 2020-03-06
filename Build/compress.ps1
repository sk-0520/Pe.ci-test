Param(
	[switch] $Diet,
	[parameter(mandatory = $true)][ValidateSet("zip", "7z")][string] $Archive,
	[parameter(mandatory = $true)][string[]] $Platforms
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'command.ps1',
	'version.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path $currentDirPath $scriptFileName
	. $scriptFilePath
}
$rootDirectory = Split-Path -Path $currentDirPath -Parent
$builToolDirPath = Join-Path $rootDirectory "Output\tools"

$version = GetAppVersion

foreach($platform in $Platforms) {
	$archiveFileName = (ConvertAppArchiveFileName $version $platform $Archive)
	$binRootDirPath = "Output\Release\$platform\Pe"

	if($Diet) {
		$removeTargets = @(
			Get-ChildItem -Path $binRootDirPath -File -Filter "*.ilk"
			Get-ChildItem -Path $binRootDirPath -File -Filter "*.iobj"
			Get-ChildItem -Path $binRootDirPath -File -Filter "*.ipdb"
			Get-ChildItem -Path $binRootDirPath -File -Filter "*.pdb" -Recurse
			Get-ChildItem -Path (Join-Path $binRootDirPath 'bin') -File -Filter "*.xml" -Recurse
		) | Select-Object -ExpandProperty FullName

		$unsupportPlatform = switch ($platform) {
			'x64' { 'x86' }
			'x86' { 'x64' }
		}
		$removeTargets += (Join-Path $binRootDirPath "bin\$unsupportPlatform")
		$removeTargets += (Join-Path $binRootDirPath "bin\lib\Redist.MSVC.CRT\$unsupportPlatform")

		foreach($removeTarget in $removeTargets) {
			Write-Output "DIET: $removeTarget"
			Remove-Item $removeTarget -Recurse -Force
		}

		Write-Output "pack SQL"
		$sqlDilrPath = Join-Path $binRootDirPath "etc\sql"
		$packSqlName = "sql.sqlite3"
		$packSqlPath = Join-Path $sqlDilrPath $packSqlName
		if(Test-Path $packSqlPath) {
			Remove-Item -Path $packSqlPath
		}
		& "$builToolDirPath\SqlPack.exe" --sql-root-dir $sqlDilrPath --output $packSqlPath
		Remove-Item -Path $sqlDilrPath -Recurse  -Force -Exclude $packSqlName
	}

	switch ($Archive) {
		'zip' {
			$destinationPath = Join-Path 'Output' $archiveFileName
			Compress-Archive -Force -Path (Join-Path $binRootDirPath "*") -DestinationPath $destinationPath
		}
		'7z' {
			try {
				Push-Location $binRootDirPath
				7z a -t7z -m0=lzma2 -mx=9 -mfb=64 -md=64m -ms=on "$archiveFileName" * -r -bsp1
			} finally {
				Pop-Location
			}
			Move-Item -Path (Join-Path $binRootDirPath $archiveFileName) -Destination 'Output'
		}
	}
}
