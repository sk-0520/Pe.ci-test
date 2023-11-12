Param(
	[Parameter(mandatory = $true)][ValidateSet('boot', 'main', 'plugins')][string] $Module,
	[switch] $ProductMode,
	[Parameter(mandatory = $true)][string] $BuildType,
	[Parameter(mandatory = $true)][string] $Revision
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

function Insert-Element {
	[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseApprovedVerbs', '', scope='function')]
	Param(
		[Parameter(mandatory = $true)][string] $Value,
		[Parameter(mandatory = $true)][xml] $Xml,
		[Parameter(mandatory = $true)][string] $TargetXpath,
		[Parameter(mandatory = $true)][string] $ParentXpath,
		[Parameter(mandatory = $true)][string] $ElementName
	)

	$element = $Xml.SelectSingleNode($TargetXpath);
	if ($null -eq $element) {
		$propGroup = $Xml.SelectSingleNode($ParentXpath)
		$element = $Xml.CreateElement($ElementName);
		$propGroup.AppendChild($element) | Out-Null;
		$element.InnerText = $Value
	}
}

function Replace-Element {
	[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseApprovedVerbs', '', scope='function')]
	Param(
		[Parameter(mandatory = $true)][hashtable] $Map,
		[Parameter(mandatory = $true)][xml] $Xml,
		[Parameter(mandatory = $true)][string] $TargetXpath
	)

	$element = $Xml.SelectSingleNode($TargetXpath);
	if ($null -ne $element) {
		$val = $element.InnerText
		foreach ($key in $Map.keys) {
			$val = $val.Replace($key, $Map[$key])
		}
		$element.InnerText = $val
	}
}

function Update-ResourceValue {
	[CmdletBinding(SupportsShouldProcess)]
	Param(
		[Parameter(mandatory = $true)][xml] $CommonXml,
		[Parameter(mandatory = $true)][string] $ResourcePath
	)

	$versionElement = $CommonXml.SelectSingleNode('/Project/PropertyGroup[1]/Version');
	$copyrightElement = $CommonXml.SelectSingleNode('/Project/PropertyGroup[1]/Copyright');
	$revisionElement = $CommonXml.SelectSingleNode('/Project/PropertyGroup[1]/InformationalVersion');

	$version = [version]$versionElement.InnerText
	$versionRevision = if ($version.Revision -eq -1) {
		0
	} else {
		$version.Revision
	}
	$csvVersion = @($version.Major, $version.Minor, $version.Build, $versionRevision) -join ','

	$resourceContent = Get-Content -Path $ResourcePath -Encoding Unicode -Raw

	$replacedResourceContent = $resourceContent `
		-replace '(\s*FILEVERSION)\s+[0-9]+\s*,\s*[0-9]+\s*,\s*[0-9]+\s*,\s*[0-9]+.*', "`$1 ${csvVersion}" `
		-replace '(\s*PRODUCTVERSION)\s+[0-9]+\s*,\s*[0-9]+\s*,\s*[0-9]+\s*,\s*[0-9]+.*', "`$1 ${csvVersion}" `
		-replace '(\s*VALUE\s+"FileVersion"\s*),.*', "`$1,`"$(${versionElement}.InnerText)`"" `
		-replace '(\s*VALUE\s+"LegalCopyright"\s*),.*', "`$1,`"$(${copyrightElement}.InnerText)`"" `
		-replace '(\s*VALUE\s+"ProductVersion"\s*),.*', "`$1,`"$(${revisionElement}.InnerText)`""
	if ($PSCmdlet.ShouldProcess('ResourcePath', "$ResourcePath のテンプレート文字列を置き換え")) {
		Set-Content -Path $ResourcePath -Value $replacedResourceContent -Encoding Unicode
	} else {
		Write-Verbose "`[DRY`] ResourcePath: $ResourcePath -> $replacedResourceContent"
	}

}

#*/[FUNCTIONS]-------------------------------------

$version = Get-ApplicationVersion

$sourceMainDirectoryPath = Get-SourceDirectory -Kind 'main'
$sourceBootDirectoryPath = Get-SourceDirectory -Kind 'boot'

$projectCommonFilePath = Join-Path -Path $sourceMainDirectoryPath -ChildPath 'Directory.Build.props'
$projectCommonXml = [XML](Get-Content $projectCommonFilePath  -Encoding UTF8)

Insert-Element -Value $version  -Xml $projectCommonXml -TargetXpath '/Project/PropertyGroup[1]/Version[1]'              -ParentXpath '/Project/PropertyGroup[1]' -ElementName 'Version'
Insert-Element -Value $Revision -Xml $projectCommonXml -TargetXpath '/Project/PropertyGroup[1]/InformationalVersion[1]' -ParentXpath '/Project/PropertyGroup[1]' -ElementName 'InformationalVersion'
$repMap = @{
	'@YYYY@' = (Get-Date).Year
	'@NAME@' = 'sk'
	'@SITE@' = 'content-type-text.net'
}
Replace-Element -Map $repMap -Xml $projectCommonXml -TargetXpath '/Project/PropertyGroup[1]/Copyright[1]'
$projectCommonXml.Save($projectCommonFilePath)

# アイコンファイルの差し替え
$appIconName = switch ($BuildType) {
	'BETA' {
		'App-beta.ico'
	}
	'' {
		'App-release.ico'
	}
	Default {
		'App-debug.ico'
	}
}
$appIconPath = Join-Path -Path 'Resource' -ChildPath 'Icon' | Join-Path -ChildPath $appIconName
$dstIconPath = Join-Path -Path 'Source' -ChildPath 'Pe' | Join-Path -ChildPath 'Pe.Main' | Join-Path -ChildPath 'Resources' | Join-Path -ChildPath 'Icon' | Join-Path -ChildPath 'App.ico'
Copy-Item -Path $appIconPath -Destination $dstIconPath -Force

if ($Module -eq 'boot') {
	Update-ResourceValue -CommonXml $projectCommonXml -ResourcePath (Join-Path -Path $sourceBootDirectoryPath -ChildPath 'Pe.Boot' | Join-Path -ChildPath 'Resource.rc')
} elseif ($Module -eq 'main') {
	#nop
} elseif ($Module -eq 'plugins') {
	$pluginProjectFiles = Get-ProjectDirectories -Kind 'plugins' |
		Get-ChildItem -File -Recurse -Include '*.csproj'

	foreach ($pluginProjectFile in $pluginProjectFiles) {
		# サポートバージョンを固定
		$assemblyInfoFilePath = Join-Path -Path $pluginProjectFile.Directory -ChildPath 'AssemblyInfo.cs'
		(Get-Content -LiteralPath $assemblyInfoFilePath) |
			ForEach-Object { $_ -replace '"0.0.0"', "`"$version`"" } |
			Set-Content -LiteralPath $assemblyInfoFilePath
	}
} else {
	throw "module error: $Module"
}
