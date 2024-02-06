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

	switch ($Kind) {
		'boot' {
			$result = Join-Path -Path (Get-RootDirectory) -ChildPath 'Source' | Join-Path -ChildPath 'Pe.Boot'
		}
		'main' {
			$result = Join-Path -Path (Get-RootDirectory) -ChildPath 'Source' | Join-Path -ChildPath 'Pe'
		}
		Default {
			throw "unknown Kind: $Kind"
		}
	}

	return [System.IO.DirectoryInfo]$result
}

function Get-ProjectDirectory {
	[OutputType([System.IO.DirectoryInfo[]])]
	Param(
		[ValidateSet('boot', 'main', 'plugins')][string] $Kind
	)

	switch ($Kind) {
		'boot' {
			$result = Get-ChildItem -Path (Join-Path -Path (Get-SourceDirectory -Kind 'boot') -ChildPath '*') -Directory
		}
		'main' {
			$result = Get-ChildItem -Path (Join-Path -Path (Get-SourceDirectory -Kind 'main') -ChildPath '*') -Directory | Where-Object { $_.Name -notlike 'Pe.Plugins.Reference.*' } | Where-Object { $_.Name -notlike 'Test*' }
		}
		'plugins' {
			$result = Get-ChildItem -Path (Join-Path -Path (Get-SourceDirectory -Kind 'main') -ChildPath '*') -Directory | Where-Object { $_.Name -like 'Pe.Plugins.Reference.*' }
		}
		Default {
			throw "unknown Kind: $Kind"
		}
	}

	return [System.IO.DirectoryInfo[]]$result
}

function Get-ApplicationProjectDirectory {
	[OutputType([System.IO.DirectoryInfo[]])]
	Param(
		[ValidateSet('boot', 'main', 'plugins')][string] $Kind
	)

	return Get-ProjectDirectory -Kind $Kind |
		Where-Object { $_.Name -notlike '*.Test' }
}

function Get-TestProjectDirectory {
	[OutputType([System.IO.DirectoryInfo[]])]
	Param(
		[ValidateSet('boot', 'main', 'plugins')][string] $Kind
	)

	return Get-ProjectDirectory -Kind $Kind |
		Where-Object { $_.Name -like '*.Test' }
}
