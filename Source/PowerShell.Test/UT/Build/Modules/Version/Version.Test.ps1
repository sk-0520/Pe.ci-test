$root = Split-Path -Path $PSScriptRoot -Parent | Split-Path -Parent | Split-Path -Parent | Split-Path -Parent | Split-Path -Parent | Split-Path -Parent
Import-Module "$root/Build/Modules/Version" -Force

Describe "Get-ApplicationVersion" {
	It "動けばいい" {
		Get-ApplicationVersion | Should BeOfType [System.Version]
	}

	It 'Convert-Version [<Expected>] = [<Version>] [<Separator>]' -TestCases @(
		@{
			Expected = '1.02.003'
			Version = '1.2.3'
			Separator = '.'
		},
		@{
			Expected = '102003'
			Version = '1.2.3'
			Separator = ''
		},
		@{
			Expected = '1-02-003'
			Version = '1.2.3'
			Separator = '-'
		},
		@{
			Expected = '1.20.003'
			Version = '1.20.3'
			Separator = '.'
		},
		@{
			Expected = '1.02.030'
			Version = '1.2.30'
			Separator = '.'
		},
		@{
			Expected = '1.02.300'
			Version = '1.2.300'
			Separator = '.'
		},
		@{
			Expected = '1.02.3000'
			Version = '1.02.3000'
			Separator = '.'
		},
		@{
			Expected = '1.200.003'
			Version = '1.200.3'
			Separator = '.'
		},
		@{
			Expected = '10.02.003'
			Version = '10.2.3'
			Separator = '.'
		}
	) {
		Param($Expected, $Version, $Separator)
		Convert-Version -Version $Version -Separator $Separator | Should Be $Expected
	}
}
