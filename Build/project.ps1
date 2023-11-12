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
			throw "unknown Kind: $Kind"
		}
	}

	return [System.IO.DirectoryInfo]$result
}

function Get-ProjectDirectories {
	Param(
		[ValidateSet('boot', 'main', 'plugins')][string] $Kind
	)

	$result = switch ($Kind) {
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
			throw "unknown Kind: $Kind"
		}
	}

	return $result | Select-Object { [System.IO.DirectoryInfo] $_ }
}

function Get-ApplicationProjectDirectories {
	Param(
		[ValidateSet('boot', 'main', 'plugins')][string] $Kind
	)

	return Get-ProjectDirectories -Kind $Kind |
		Where-Object { $_.Name -notlike '*.Test' }
}

function Get-TestProjectDirectories {
	Param(
		[ValidateSet('boot', 'main', 'plugins')][string] $Kind
	)

	return Get-ProjectDirectories -Kind $Kind |
		Where-Object { $_.Name -like '*.Test' }
}
