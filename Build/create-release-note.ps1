Param(
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$PSDefaultParameterValues['Out-File:Encoding'] = 'utf8'
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'version.ps1',
	'command.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path $currentDirPath $scriptFileName
	. $scriptFilePath
}
$rootDirPath = Split-Path -Parent $currentDirPath
$outputDirectory = Join-Path $rootDirPath 'Output'

$rawChangelogsFile = Join-Path $rootDirPath "Define/changelogs.json"
$rawChangelogLinkFile = Join-Path $rootDirPath "Source/Help/script/changelog-link.ts"
$rawChangelogStyleFile = Join-Path $rootDirPath "Source/Help/style/changelog.scss"
$templateHtmlFile = Join-Path $currentDirPath 'release-note.html'
$pluginTemplateHtmlFile = Join-Path $currentDirPath 'release-note.plugin.html'

# -------------------------------
# Pe
# -------------------------------

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
			}
			else {
				$attrs = @()
				foreach ($key in $this.attributes.Keys) {
					$val = $this.attributes[$key]
					if ($val) {
						$attrs += "${key}=`"$($this.Escape(${val}))`""
					}
					else {
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
		}
		else {
			return $this.Escape($this.text)
		}
	}

}

$contentMap = @{
	'note'      = 'メモ'
	'features'  = '機能'
	'fixes'     = '修正'
	'developer' = '開発'
}

# 無理やりjsonにする
$rawChangelogsContent = Get-Content $rawChangelogsFile -Raw -Encoding UTF8

$json = $rawChangelogsContent | ConvertFrom-Json
Write-Output $json
$currentVersion = $json[0]

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
		continue;
	}

	$section = $contents.CreateChild('section')
	$sectionHeader = $section.CreateChild('h3') # changelogs.ts のスタイル流用のため
	$sectionHeader.CreateText($contentMap[$content.type])
	$sectionHeader.attributes['class'] = $content.type
	$logs = $section.CreateChild('ul')
	foreach ($log in $content.logs) {
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

$compiledChangelogLinkFile = Join-Path $outputDirectory 'changelog-link.js'
$compiledChangelogStyleFile = Join-Path $outputDirectory 'changelog.css'

npx tsc  "$rawChangelogLinkFile" --outFile "$compiledChangelogLinkFile"
npx sass "$rawChangelogStyleFile" --style compressed --no-source-map "$compiledChangelogStyleFile"

$htmlContent = (Get-Content $templateHtmlFile -Encoding UTF8 -Raw)
$htmlContent = $htmlContent.Replace('<!--CONTENT-->', $root.ToHtml())
$htmlContent = $htmlContent.Replace('//SCRIPT', (Get-Content $compiledChangelogLinkFile -Raw -Encoding UTF8))
$htmlContent = $htmlContent.Replace('/*STYLE*/', (Get-Content $compiledChangelogStyleFile -Raw -Encoding UTF8))

$version = GetAppVersion
Set-Content (Join-Path $outputDirectory (ConvertReleaseNoteFileName $version 'html')) -Value $htmlContent -Encoding UTF8


# -------------------------------
# プラグイン
# -------------------------------
$pluginProjectDirectoryPath = Join-Path $rootDirPath 'Source/Pe'

#Set-WinSystemLocale ja-JP

$logFile = '@.txt'
$scriptFilePath = Join-Path $currentDirPath 'git-log-output.bat'

$pluginProjectDirectories = Get-ChildItem -Path $pluginProjectDirectoryPath -Filter ('Pe.Plugins.Reference.*') -Directory
foreach ($pluginProjectDirectory in $pluginProjectDirectories) {
	$outputPath = $pluginProjectDirectory.Name + '.html'

	Push-Location -Path $pluginProjectDirectory.FullName

	Get-Location
	& $scriptFilePath "$($pluginProjectDirectory.FullName)" "$logFile"

	$pluginRoot = New-Object Element 'div'
	$pluginContents = $pluginRoot.CreateChild('div');
	$logSection = $pluginContents.CreateChild('section')
	$logList = $logSection.CreateChild('ul')

	$logContents = Get-Content $logFile -Encoding UTF8

	foreach($logContent in $logContents) {
		$values = $logContent.Split("`t", 3, [System.StringSplitOptions]::None)
		$hash = $values[0]
		$timestamp = $values[1]
		$message = $values[2]

		$logItem = $logList.CreateChild('li')

		$logHeader = $logItem.CreateChild('span')
		$logHeader.attributes['class'] = 'header'

		$logSubject = $logHeader.CreateChild('time')
		$logSubject.CreateText('[' + $timestamp + '] ')
		$logSubject.attributes['datetime'] = $timestamp
		$logSubject.attributes['class'] = 'timestamp'

		$logSubject = $logHeader.CreateChild('span')
		$logSubject.CreateText($message)
		$logSubject.attributes['class'] = 'subject'

		$logRevision = $logHeader.CreateChild('a')
		$logRevision.CreateText($hash)
		$logRevision.attributes['class'] = 'revision'
	}

	$pluginHtmlContent = (Get-Content $pluginTemplateHtmlFile -Encoding UTF8 -Raw)
	$pluginHtmlContent = $pluginHtmlContent.Replace('<!--NAME-->', ($pluginProjectDirectory.Name + ', ' + $currentVersion.version))
	$pluginHtmlContent = $pluginHtmlContent.Replace('<!--CONTENT-->', $pluginRoot.ToHtml());
	$pluginHtmlContent = $pluginHtmlContent.Replace('//SCRIPT', (Get-Content $compiledChangelogLinkFile -Raw -Encoding UTF8))
	$pluginHtmlContent = $pluginHtmlContent.Replace('/*STYLE*/', (Get-Content $compiledChangelogStyleFile -Raw -Encoding UTF8))

	Set-Content (Join-Path $outputDirectory $outputPath) -Value $pluginHtmlContent -Encoding UTF8

	Pop-Location
}
