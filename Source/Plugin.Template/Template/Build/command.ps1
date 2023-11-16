$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

function Test-CommandExist {
	[OutputType([bool])]
	Param (
		[Parameter(Mandatory = $true)][string] $Command
	)

	$oldPreference = $ErrorActionPreference

	$ErrorActionPreference = 'stop'

	try {
		if (Get-Command $Command) {
			return $true
		}
	} catch {
		return $false
	} finally {
		$ErrorActionPreference = $oldPreference
	}
}

function Set-Command {
	[CmdletBinding(SupportsShouldProcess)]
	Param (
		[Parameter(Mandatory = $true)][string] $Command,
		[Parameter(Mandatory = $true)][string] $EnvironmentVariableName,
		[Parameter(Mandatory = $true)][string] $DefaultPath
	)

	if ( ! ( Test-CommandExist -Command $Command )) {
		#$envValue = env:$envName
		$envValue = Get-ChildItem env: | Where-Object { $_.Name -match $EnvironmentVariableName } | Select-Object -Property Value -First 1
		if ( $null -eq $envValue) {
			if ($PSCmdlet.ShouldProcess('Command', "$DefaultPath を PATH に設定")) {
				$env:Path = [Environment]::ExpandEnvironmentVariables($DefaultPath) + ';' + $env:Path
			} else {
				Write-Verbose "`[DRY`] add $DefaultPath"
			}
		} else {
			if ($PSCmdlet.ShouldProcess('Command', "$EnvironmentVariableName を PATH に設定")) {
				$env:Path = [Environment]::ExpandEnvironmentVariables($EnvironmentVariableName) + ';' + $env:Path
			} else {
				Write-Verbose "`[DRY`] add $EnvironmentVariableName"
			}
		}
	}
}

function TestAliasExists([string] $alias) {
	$oldPreference = $ErrorActionPreference

	$ErrorActionPreference = 'stop'

	try {
		if (Get-Alias $alias) {
			return $true
		}
	} catch {
		return $false
	} finally {
		$ErrorActionPreference = $oldPreference
	}
}

Set-Command -Command 'git'     -EnvironmentVariableName 'BUILD_GIT_PATH'     -DefaultPath '%PROGRAMFILES%\git\bin'
Set-Command -Command 'msbuild' -EnvironmentVariableName 'BUILD_MSBUILD_PATH' -DefaultPath '%PROGRAMFILES%\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin'
Set-Command -Command 'dotnet'  -EnvironmentVariableName 'BUILD_DOTNET_PATH'  -DefaultPath '%PROGRAMFILES%\dotnet'
Set-Command -Command 'node'    -EnvironmentVariableName 'BUILD_NODE_PATH'    -DefaultPath '%PROGRAMFILES%\nodejs'
Set-Command -Command 'npm'     -EnvironmentVariableName 'BUILD_NPM_PATH'     -DefaultPath '%PROGRAMFILES%\nodejs'
Set-Command -Command 'npx'     -EnvironmentVariableName 'BUILD_NPX_PATH'     -DefaultPath '%PROGRAMFILES%\nodejs'
Set-Command -Command '7z'      -EnvironmentVariableName 'BUILD_7ZIP_PATH'    -DefaultPath '%PROGRAMFILES%\7-Zip'

