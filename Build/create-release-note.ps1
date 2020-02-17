Param(
)
$ErrorActionPreference = 'Stop'
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'version.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path $currentDirPath $scriptFileName
	. $scriptFilePath
}
$rootDirPath = Split-Path -Parent $currentDirPath
$outputDirectory = Join-Path $rootDirPath 'Output'

$rawChangelogsFile = Join-Path $rootDirPath "Source/Documents/source/script/changelogs.ts"
$templateHtmlFile = Join-Path $currentDirPath 'release-note.html'

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
$headerMark = '--------RELEASE HEAD--------'
$prevHeaderIndex = $rawChangelogsContent.IndexOf($headerMark)
$prevHeaderContent = $rawChangelogsContent.Substring($prevHeaderIndex + $headerMark.Length)
$json = '[' + $prevHeaderContent.Substring($prevHeaderContent.IndexOf('{')) | ConvertFrom-Json

$currentVersion = $json[0]

# 速度とかどうでもいい
$body = New-Object Element 'body'
$headline = $body.CreateChild('h1')
$headline.CreateText($currentVersion.version);
$headline.CreateText(': ');
$headline.CreateText($currentVersion.date);
$contents = $body.CreateChild('div');
$contents.attributes['id'] = 'content'
foreach ($content in $currentVersion.contents) {
	if (!($content.logs)) {
		continue;
	}

	$section = $contents.CreateChild('section')
	$sectionHeader = $section.CreateChild('h2')
	$sectionHeader.CreateText($contentMap[$content.type])
	$logs = $section.CreateChild('ul')
	foreach ($log in $content.logs) {
		$logItem = $logs.CreateChild('li')
		if ($log.class) {
			$logItem.attributes['class'] = $log.class
		}

		$logHeader = $logItem.CreateChild('span')
		$logHeader.attributes['class'] = 'header'

		$logSubject = $logHeader.CreateChild('span')
		$logSubject.CreateText($log.subject)
		$logSubject.attributes['class'] = 'subject'

		$logRevision = $logHeader.CreateChild('span')
		$logRevision.CreateText($log.revision)
		$logRevision.attributes['class'] = 'revision'

		if ($log.comments) {
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
$htmlContent = $htmlContent.Replace('<body></body>', $body.ToHtml())

$version = GetAppVersion
Set-Content (Join-Path $outputDirectory "Pe_$version.html") -Value $htmlContent -Encoding UTF8
