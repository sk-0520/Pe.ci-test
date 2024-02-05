Param(
	[Parameter(mandatory = $true)][string] $Revision,
	[Parameter(mandatory = $true)][string] $ProjectFilePath
)

function Insert-Element {
	[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseApprovedVerbs', '', scope='function')]
	Param(
		[string] $Value,
		[xml] $Xml,
		[string] $TargetXpath,
		[string] $ParentXpath,
		[string] $ElementName
	)
	$element = $Xml.SelectSingleNode($TargetXpath)
	if ($null -eq $element) {
		$propGroup = $Xml.SelectSingleNode($ParentXpath)
		$element = $Xml.CreateElement($ElementName)
		$propGroup.AppendChild($element) | Out-Null
		$element.InnerText = $Value
	}
}


function Replace-Element {
	[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseApprovedVerbs', '', scope='function')]
	Param(
		[Parameter(mandatory = $true)][hashtable] $Map,
		[Parameter(mandatory = $true)][xml] $Xml,
		[Parameter(mandatory = $true)][string] $TargetXpath,
		[Parameter(mandatory = $true)][string] $ParentXpath,
		[Parameter(mandatory = $true)][string] $ElementName
	)
	$element = $Xml.SelectSingleNode($TargetXpath)
	if ($null -ne $element) {
		$val = $element.InnerText
		foreach ($key in $Map.keys) {
			$val = $val.Replace($key, $Map[$key])
		}
		$element.InnerText = $val
	}
}

$projectXml = [XML](Get-Content $ProjectFilePath  -Encoding UTF8)

Insert-Element -Value $Revision -Xml $projectXml -TargetXpath '/Project/PropertyGroup[1]/InformationalVersion[1]' -ParentXpath '/Project/PropertyGroup[1]' -ElementName 'InformationalVersion'

$repMap = @{
	'@YYYY@' = (Get-Date).Year
	'@NAME@' = 'あなたの名前'
	'@SITE@' = 'あなたのサイト'
}
Replace-Element -Map $repMap -Xml $projectXml -TargetXpath '/Project/PropertyGroup[1]/Copyright[1]' -ParentXpath '/Project/PropertyGroup[1]' -ElementName 'Copyright'

$projectXml.Save($ProjectFilePath)

