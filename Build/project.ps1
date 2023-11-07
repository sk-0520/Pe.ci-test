$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootDirPath = Split-Path -Parent $currentDirPath

function GetSourceDirectory([ValidateSet('boot', 'main')][string] $kind) {
	switch ($kind) {
		'boot' { return Join-Path -Path $rootDirPath -ChildPath "Source/Pe.Boot" }
		'main' { return Join-Path -Path $rootDirPath -ChildPath "Source/Pe" }
		Default {
			throw "kind: $kind"
		}
	}
}

function GetProjectDirectories([ValidateSet('boot', 'main', 'plugins')][string] $kind) {
	switch ($kind) {
		'boot' { return Get-ChildItem -Path (Join-Path -Path (GetSourceDirectory 'boot') -ChildPath '*') -Directory }
		'main' { return Get-ChildItem -Path (Join-Path -Path (GetSourceDirectory 'main') -ChildPath '*') -Directory | Where-Object { $_.Name -notlike 'Pe.Plugins.Reference.*' } | Where-Object { $_.Name -notlike 'Test*' } }
		'plugins' { return Get-ChildItem -Path (Join-Path -Path (GetSourceDirectory 'main') -ChildPath '*') -Directory | Where-Object { $_.Name -like 'Pe.Plugins.Reference.*' } }
		Default { throw "unknown kind: $kind" }
	}
}

function GetApplicationProjectDirectories([ValidateSet('boot', 'main', 'plugins')][string] $kind) {
	return GetProjectDirectories $kind `
		| Where-Object { $_.Name -notlike '*.Test' }
}

function GetTestProjectDirectories([ValidateSet('boot', 'main', 'plugins')][string] $kind) {
	return GetProjectDirectories $kind `
		| Where-Object { $_.Name -like '*.Test' }
}
