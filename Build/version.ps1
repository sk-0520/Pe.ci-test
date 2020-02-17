$ErrorActionPreference = 'Stop'

$projectFile = Join-Path (Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)) "Source/Pe/Pe.Main/Pe.Main.csproj"

function GetAppVersion {
	$projectXml = [XML](Get-Content -Path $projectFile -Encoding UTF8)
	$projectNav = $projectXml.CreateNavigator()
	$vesion = $projectNav.Select('/Project/PropertyGroup/Version').Value

	return $vesion
}

function ConvertFileName([string] $header, [version] $version, [string] $tail, [string] $extension) {
	if( ! $header ) {
		throw "empty header";
	}

	$nameBuffer = @(
		$header,
		"_"
		"{0}"    -f $version.Major
		"-"
		"{0:00}" -f $version.Minor
		"-"
		"{0:00}" -f $version.Build
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
