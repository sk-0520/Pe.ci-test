Param(
	[Parameter(mandatory = $true)][ValidateSet('boot', 'main', 'plugins')][string] $Module,
	[switch] $ProductMode,
	[string] $BuildType,
	[string] $Revision
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'version.ps1'
	'project.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path -Path $currentDirPath -ChildPath $scriptFileName
	. $scriptFilePath
}

#/*[FUNCTIONS]-------------------------------------

function InsertElement([string] $value, [xml] $xml, [string] $targetXpath, [string] $parentXpath, [string] $elementName) {
	$element = $xml.SelectSingleNode($targetXpath);
	if ($null -eq $element) {
		$propGroup = $xml.SelectSingleNode($parentXpath)
		$element = $xml.CreateElement($elementName);
		$propGroup.AppendChild($element) | Out-Null;
		$element.InnerText = $value
	}
}

function ReplaceElement([hashtable] $map, [xml] $xml, [string] $targetXpath, [string] $parentXpath, [string] $elementName) {
	$element = $xml.SelectSingleNode($targetXpath);
	if ($null -ne $element) {
		$val = $element.InnerText
		foreach ($key in $map.keys) {
			$val = $val.Replace($key, $map[$key])
		}
		$element.InnerText = $val
	}
}

function ReplaceResourceValue([xml] $commonXml, [string] $resourcePath) {
	$versionElement = $commonXml.SelectSingleNode('/Project/PropertyGroup[1]/Version');
	$copyrightElement = $commonXml.SelectSingleNode('/Project/PropertyGroup[1]/Copyright');
	$revisionElement = $commonXml.SelectSingleNode('/Project/PropertyGroup[1]/InformationalVersion');

	$version = [version]$versionElement.InnerText
	$versionRevision = if ($version.Revision -eq -1) { 0 } else { $version.Revision }
	$csvVersion = @($version.Major, $version.Minor, $version.Build, $versionRevision) -join ','

	$resourceContent = Get-Content -Path $resourcePath -Encoding Unicode -Raw

	$replacedResourceContent = $resourceContent `
		-replace '(\s*FILEVERSION)\s+[0-9]+\s*,\s*[0-9]+\s*,\s*[0-9]+\s*,\s*[0-9]+.*', "`$1 ${csvVersion}" `
		-replace '(\s*PRODUCTVERSION)\s+[0-9]+\s*,\s*[0-9]+\s*,\s*[0-9]+\s*,\s*[0-9]+.*', "`$1 ${csvVersion}" `
		-replace '(\s*VALUE\s+"FileVersion"\s*),.*', "`$1,`"$(${versionElement}.InnerText)`"" `
		-replace '(\s*VALUE\s+"LegalCopyright"\s*),.*', "`$1,`"$(${copyrightElement}.InnerText)`"" `
		-replace '(\s*VALUE\s+"ProductVersion"\s*),.*', "`$1,`"$(${revisionElement}.InnerText)`""

	Set-Content -Path $resourcePath -Value $replacedResourceContent -Encoding Unicode
}

#*/[FUNCTIONS]-------------------------------------

$version = GetAppVersion

$sourceMainDirectoryPath = GetSourceDirectory 'main'
$sourceBootDirectoryPath = GetSourceDirectory 'boot'

$projectCommonFilePath = Join-Path -Path $sourceMainDirectoryPath -ChildPath "Directory.Build.props"
$projectCommonXml = [XML](Get-Content $projectCommonFilePath  -Encoding UTF8)

InsertElement $version $projectCommonXml '/Project/PropertyGroup[1]/Version[1]' '/Project/PropertyGroup[1]' 'Version'
InsertElement $Revision $projectCommonXml '/Project/PropertyGroup[1]/InformationalVersion[1]' '/Project/PropertyGroup[1]' 'InformationalVersion'
$repMap = @{
	'@YYYY@' = (Get-Date).Year
	'@NAME@' = 'sk'
	'@SITE@' = 'content-type-text.net'
}
ReplaceElement $repMap $projectCommonXml '/Project/PropertyGroup[1]/Copyright[1]' '/Project/PropertyGroup[1]' 'Copyright'
$projectCommonXml.Save($projectCommonFilePath)

# アイコンファイルの差し替え
$appIconName = switch ($BuildType) {
	'BETA' { 'App-beta.ico' }
	'' { 'App-release.ico' }
	Default { 'App-debug.ico' }
}
$appIconPath = Join-Path -Path 'Resource\Icon' -ChildPath $appIconName
Copy-Item -Path $appIconPath -Destination 'Source\Pe\Pe.Main\Resources\Icon\App.ico' -Force

if ($Module -eq 'boot') {
	ReplaceResourceValue $projectCommonXml (Join-Path -Path $sourceBootDirectoryPath -ChildPath 'Pe.Boot' | Join-Path -ChildPath 'Resource.rc')

}
elseif ($Module -eq 'main') {

}
elseif ($Module -eq 'plugins') {
	$pluginProjectFiles = GetProjectDirectories 'plugins' `
	| Get-ChildItem -File -Recurse -Include '*.csproj'

	foreach ($pluginProjectFile in $pluginProjectFiles) {
		# サポートバージョンを固定
		$assemblyInfoFilePath = Join-Path -Path $pluginProjectFile.Directory -ChildPath 'AssemblyInfo.cs'
		(Get-Content -LiteralPath $assemblyInfoFilePath) `
		| ForEach-Object { $_ -replace '"0.0.0"', "`"$version`"" } `
		| Set-Content -LiteralPath $assemblyInfoFilePath
	}
}
else {
	throw 'うわわわわ'
}
