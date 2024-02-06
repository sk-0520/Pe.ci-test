#//# んもう Requires -Modules @{ ModuleName="Pester"; ModuleVersion="3.4.0"; MaximumVersion="3.4.0" }

$targets = @(
	'Build'
)

$testFiles = @()

foreach($target in $targets) {
	$testEntryDirectory = Join-Path -Path $PSScriptRoot -ChildPath $target
	$testTargetFiles = Get-ChildItem -Path $testEntryDirectory -Recurse -File -Include '*.Test.ps1'
	$testFiles += $testTargetFiles
}

Invoke-Pester -Path $testFiles

