Param(
	[Parameter(mandatory = $true)][string] $OutputDirectory,
	[switch] $IgnoreEmpty
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$PSDefaultParameterValues['Out-File:Encoding'] = 'utf8'
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'version.ps1',
	'project.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path -Path $currentDirPath -ChildPath $scriptFileName
	. $scriptFilePath
}
$rootDirPath = Split-Path -Parent $currentDirPath

Import-Module "${PSScriptRoot}/Modules/Project"

# STARTUP

$contentMap = @{
	'note' = 'メモ'
	'features' = '機能'
	'fixes' = '修正'
	'developer' = '開発'
}

if ( Test-Path -Path $OutputDirectory ) {
	Remove-Item -Path $OutputDirectory -Force -Recurse
}
New-Item -Path $OutputDirectory -ItemType Directory
$OutputDirectory = Convert-Path -Path $OutputDirectory


$rawChangelogLinkFile = Join-Path -Path $rootDirPath -ChildPath 'Source' | Join-Path -ChildPath 'Help' | Join-Path -ChildPath 'script' | Join-Path -ChildPath 'changelog-link.ts'
$rawChangelogStyleFile = Join-Path -Path $rootDirPath -ChildPath 'Source' | Join-Path -ChildPath 'Help' | Join-Path -ChildPath 'style' | Join-Path -ChildPath 'changelog.scss'

$compiledChangelogLinkFile = 'changelog-link.js'
$compiledChangelogStyleFile = 'changelog.css'

npx tsc  "$rawChangelogLinkFile" --outFile "$compiledChangelogLinkFile"
if (-not $?) {
	throw "build error: $rawChangelogLinkFile"
}
npx sass "$rawChangelogStyleFile" --style compressed --no-source-map "$compiledChangelogStyleFile"
if (-not $?) {
	throw "build error: $rawChangelogStyleFile"
}

$compiledChangelogLinkFile = Convert-Path -Path $compiledChangelogLinkFile
$compiledChangelogStyleFile = Convert-Path -Path $compiledChangelogStyleFile

# 無理やりjsonにする
$rawChangelogsFile = Join-Path -Path $rootDirPath -ChildPath 'Define' | Join-Path -ChildPath 'changelogs.json'
$rawChangelogsContent = Get-Content $rawChangelogsFile -Raw -Encoding UTF8 | ConvertFrom-Json
$currentVersion = $rawChangelogsContent[0]

#/*[FUNCTIONS]-------------------------------------

# ノード作らず適当に
class Element {
	[string] $elementName;
	[Element[]] $children = @()
	[hashtable] $attributes = @{ }
	[string] $text

	Element([string] $elementName) {
		$this.elementName = $elementName
	}

	[Element] CreateChild([string] $elementName) {
		$elm = New-Object Element $elementName
		$this.children += $elm
		return $elm
	}

	[void] CreateText([string] $text) {
		$elm = New-Object Element ''
		$elm.text = $text
		$this.children += $elm
	}

	[string] Escape([string] $s) {
		$s = $s.Replace('&', '&amp;')
		$s = $s.Replace('<', '&lt;')
		$s = $s.Replace('>', '&gt;')
		$s = $s.Replace('"', '&quot;')
		$s = $s.Replace(' ', '&nbsp;')
		return $s
	}


	[string] ToHtml() {
		if ($this.elementName) {
			$html = @()
			if ( 0 -eq $this.attributes.Count) {
				$html += "<$($this.elementName)>"
			} else {
				$attrs = @()
				foreach ($key in $this.attributes.Keys) {
					$val = $this.attributes[$key]
					if ($val) {
						$attrs += "${key}=`"$($this.Escape(${val}))`""
					} else {
						$attrs += "${key}"
					}
				}

				$html += "<$($this.elementName) $($attrs -join ' ')>"
			}
			foreach ($child in $this.children) {
				$childHtml = $child.ToHtml()
				$html += $childHtml
			}

			$html += "</$($this.elementName)>"
			return $html -join ''
		} else {
			return $this.Escape($this.text)
		}
	}
}

function MakeApplication {
	$templateHtmlFile = Join-Path -Path $currentDirPath -ChildPath 'release-note.html'

	# 速度とかどうでもいい
	$root = New-Object Element 'div'
	$headline = $root.CreateChild('h2') # changelogs.ts のスタイル流用のため
	$headline.CreateText($currentVersion.version);
	$headline.CreateText(': ');
	$headline.CreateText($currentVersion.date);
	$contents = $root.CreateChild('div');
	$contents.attributes['id'] = 'content'
	foreach ($content in $currentVersion.contents) {
		if (!($content.PSObject.Properties.Match('logs').Count)) {
			if (!$IgnoreEmpty) {
				throw '!!empty logs!!'
			}
			continue;
		}

		$section = $contents.CreateChild('section')
		$sectionHeader = $section.CreateChild('h3') # changelogs.ts のスタイル流用のため
		$sectionHeader.CreateText($contentMap[$content.type])
		$sectionHeader.attributes['class'] = $content.type
		$logs = $section.CreateChild('ul')
		foreach ($log in $content.logs) {
			if (!$IgnoreEmpty -And [string]::IsNullOrWhitespace($log.subject)) {
				throw '!!empty log!!'
			}

			$logItem = $logs.CreateChild('li')
			if ($log.PSObject.Properties.Match('class').Count) {
				$logItem.attributes['class'] = $log.class
			}

			$logHeader = $logItem.CreateChild('span')
			$logHeader.attributes['class'] = 'header'

			$logSubject = $logHeader.CreateChild('span')
			$logSubject.CreateText($log.subject)
			$logSubject.attributes['class'] = 'subject'

			if ($log.PSObject.Properties.Match('revision').Count) {
				if ($log.revision.Length) {
					$logRevision = $logHeader.CreateChild('a')
					$logRevision.CreateText($log.revision)
					$logRevision.attributes['class'] = 'revision'
				}
			}


			if ($log.PSObject.Properties.Match('comments').Count) {
				$logComments = $logItem.CreateChild('ul')
				$logComments.attributes['class'] = 'comments'

				foreach ($comment in $log.comments) {
					$commentItem = $logComments.CreateChild('li')
					$commentItem.CreateText($comment)
				}
			}
		}
	}

	$htmlContent = (Get-Content $templateHtmlFile -Encoding UTF8 -Raw)
	$htmlContent = $htmlContent.Replace('<!--CONTENT-->', $root.ToHtml())
	$htmlContent = $htmlContent.Replace('//SCRIPT', (Get-Content $compiledChangelogLinkFile -Raw -Encoding UTF8))
	$htmlContent = $htmlContent.Replace('/*STYLE*/', (Get-Content $compiledChangelogStyleFile -Raw -Encoding UTF8))

	Set-Content (Join-Path -Path $OutputDirectory -ChildPath 'Pe.html') -Value $htmlContent -Encoding UTF8
}

function MakePlugins {
	$pluginTemplateHtmlFile = Join-Path -Path $currentDirPath -ChildPath 'release-note.plugin.html'

	#Set-WinSystemLocale ja-JP

	#$logFile = '@.txt'

	$pluginProjectDirectories = Get-ProjectDirectories -Kind 'plugins'
	foreach ($pluginProjectDirectory in $pluginProjectDirectories) {
		$outputName = $pluginProjectDirectory.Name + '.html'

		Push-Location -Path $pluginProjectDirectory.FullName

		$pluginRoot = New-Object Element 'div'
		$pluginContents = $pluginRoot.CreateChild('div');
		$logSection = $pluginContents.CreateChild('section')
		$logList = $logSection.CreateChild('ul')

		$logItem = $logList.CreateChild('li')
		$logItem.CreateText('プラグイン版リリースノートは使い道ないので未実装');

		$pluginHtmlContent = (Get-Content $pluginTemplateHtmlFile -Encoding UTF8 -Raw)
		$pluginHtmlContent = $pluginHtmlContent.Replace('<!--NAME-->', ($pluginProjectDirectory.Name + ', ' + $currentVersion.version))
		$pluginHtmlContent = $pluginHtmlContent.Replace('<!--CONTENT-->', $pluginRoot.ToHtml());
		$pluginHtmlContent = $pluginHtmlContent.Replace('//SCRIPT', (Get-Content $compiledChangelogLinkFile -Raw -Encoding UTF8))
		$pluginHtmlContent = $pluginHtmlContent.Replace('/*STYLE*/', (Get-Content $compiledChangelogStyleFile -Raw -Encoding UTF8))

		Set-Content (Join-Path -Path $OutputDirectory -ChildPath $outputName) -Value $pluginHtmlContent -Encoding UTF8

		Pop-Location
	}
}

#*/[FUNCTIONS]-------------------------------------


MakeApplication

MakePlugins
