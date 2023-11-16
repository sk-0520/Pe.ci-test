$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest


function Get-RootDirectory {
	[OutputType([System.IO.DirectoryInfo])]
	Param()

	return [System.IO.DirectoryInfo](Split-Path -Parent $PSScriptRoot | Split-Path -Parent | Split-Path -Parent)
}

function Get-SourceDirectory {
	[OutputType([System.IO.DirectoryInfo])]
	Param(
		[ValidateSet('boot', 'main')][string] $Kind
	)

	$result = switch ($Kind) {
		'boot' {
			Join-Path -Path (Get-RootDirectory) -ChildPath 'Source' | Join-Path -ChildPath 'Pe.Boot'
		}
		'main' {
			Join-Path -Path (Get-RootDirectory) -ChildPath 'Source' | Join-Path -ChildPath 'Pe'
		}
		Default {
			throw "unknown Kind: $Kind"
		}
	}

	return [System.IO.DirectoryInfo]$result
}

function Get-ProjectDirectories {
	[OutputType([System.IO.DirectoryInfo[]])]
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
	[OutputType([System.IO.DirectoryInfo[]])]
	Param(
		[ValidateSet('boot', 'main', 'plugins')][string] $Kind
	)

	return Get-ProjectDirectories -Kind $Kind |
		Where-Object { $_.Name -notlike '*.Test' }
}

function Get-TestProjectDirectories {
	[OutputType([System.IO.DirectoryInfo[]])]
	Param(
		[ValidateSet('boot', 'main', 'plugins')][string] $Kind
	)

	return Get-ProjectDirectories -Kind $Kind |
		Where-Object { $_.Name -like '*.Test' }
}
