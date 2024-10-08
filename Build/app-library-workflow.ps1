Param(
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Project"

$dirs = Get-TestProjectDirectory -Kind 'main' |
	Where-Object { $_.Name.StartsWith('Pe.Library.') }

$workflowsFilePath = Join-Path -Path (Get-RootDirectory).FullName -ChildPath '.github' |
	Join-Path -ChildPath 'workflows' |
	Join-Path -ChildPath '_build-template.yml'


$workflows = Get-Content -LiteralPath $workflowsFilePath -Raw
$labels = @(
	'test-app-library:'
	'strategy:'
	'matrix:'
	'TARGET:'
)
$buffer = $workflows
foreach ($label in $labels) {
	$labelIndex = $buffer.IndexOf($label)
	$buffer = $buffer.Substring($labelIndex + $label.Length)
}
$lastIndex = $buffer.IndexOf(':')
$projects = @()
foreach ($line in $buffer.Substring(0, $lastIndex) -split "`r?`n") {
	if ($line -match '\s+-\s([\w\-\.]+)') {
		$projects += $Matches[1].Trim()
	}
}

foreach ($dir in $dirs) {
	if (!$projects.Contains($dir.Name)) {
		throw "notfound: $dir [$projects]"
	}
}

