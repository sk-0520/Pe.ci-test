$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootDirPath = Split-Path -Parent $currentDirPath

function Get-RootDirectory {
	[OutputType([System.IO.DirectoryInfo])]
	Param()

	return [System.IO.DirectoryInfo](Split-Path -Parent $PSScriptRoot)
}

function Get-SourceDirectory {
	[OutputType([System.IO.DirectoryInfo])]
	Param(
		[ValidateSet('boot', 'main')][string] $Kind
	)

	$result = switch ($Kind) {
		'boot' {
			Join-Path -Path $rootDirPath -ChildPath 'Source/Pe.Boot'
		}
		'main' {
			Join-Path -Path $rootDirPath -ChildPath 'Source/Pe'
		}
		Default {
			throw "kind: $Kind"
		}
	}

	return [System.IO.DirectoryInfo]$result
}

function GetProjectDirectories([ValidateSet('boot', 'main', 'plugins')][string] $kind) {
	switch ($kind) {
		'boot' {
			return Get-ChildItem -Path (Join-Path -Path (Get-SourceDirectory -Kind 'boot') -ChildPath '*') -Directory
		}
		'main' {
			return Get-ChildItem -Path (Join-Path -Path (Get-SourceDirectory -Kind 'main') -ChildPath '*') -Directory | Where-Object { $_.Name -notlike 'Pe.Plugins.Reference.*' } | Where-Object { $_.Name -notlike 'Test*' }
		}
		'plugins' {
			return Get-ChildItem -Path (Join-Path -Path (Get-SourceDirectory -Kind 'main') -ChildPath '*') -Directory | Where-Object { $_.Name -like 'Pe.Plugins.Reference.*' }
		}
		Default {
			throw "unknown kind: $kind"
		}
	}
}

function GetApplicationProjectDirectories([ValidateSet('boot', 'main', 'plugins')][string] $kind) {
	return GetProjectDirectories $kind |
		Where-Object { $_.Name -notlike '*.Test' }
}

function GetTestProjectDirectories([ValidateSet('boot', 'main', 'plugins')][string] $kind) {
	return GetProjectDirectories $kind |
		Where-Object { $_.Name -like '*.Test' }
}
