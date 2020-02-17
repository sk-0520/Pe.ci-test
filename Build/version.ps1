$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$projectFile = Join-Path (Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)) "Source/Pe/Pe.Main/Pe.Main.csproj"

function GetAppVersion {
	$projectXml = [XML](Get-Content -Path $projectFile -Encoding UTF8)
	$projectNav = $projectXml.CreateNavigator()
	$vesion = $projectNav.Select('/Project/PropertyGroup/Version').Value

	return $vesion
}

function ConvertVersion([version] $version, [string] $separator) {
	$values = @(
		"{0}"     -f $version.Major
		"{0:00}"  -f $version.Minor
		"{0:000}" -f $version.Build
	)
	return $values -join $separator
}

function ConvertFileName([string] $head, [version] $version, [string] $tail, [string] $extension) {
	if( ! $head ) {
		throw "empty head";
	}

	$nameBuffer = @(
		$head,
		"_"
		(ConvertVersion $version '-')
	)
	if( $tail ) {
		$nameBuffer += '_'
		$nameBuffer += $tail
	}

	$nameBuffer += '.'
	$nameBuffer += $extension

	return $nameBuffer -join ''
}

function ConvertAppArchiveFileName([version] $version, [string] $platform) {
	return ConvertFileName 'Pe' $version $platform 'zip'
}

function ConvertReleaseNoteFileName([version] $version) {
	return ConvertFileName 'Pe' $version '' 'html'
}
