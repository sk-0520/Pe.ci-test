<#
.SYNOPSIS
Pe プラグインのテンプレート作成処理。

<CommonParameters> は使用しない。


.DESCRIPTION
Pe リポジトリからいい感じのあれこれを取ってきてあれこれするんよ。


.EXAMPLE
PS C:\DIR> .\create-project.ps1 -Param あれこれ


.PARAMETER ProjectDirectory
プラグインテンプレートプロジェクトのルートディレクトリパス

.PARAMETER PluginName
プロジェクト名

.PARAMETER PluginId
プラグインID

.PARAMETER AppTargetBranch
対象 Pe のブランチ

.PARAMETER GitPath
環境変数PATH に割り当てる git がインストールされているパス

.PARAMETER DotNetPath
環境変数PATH に割り当てる dotnet がインストールされているパス

.LINK
https://bitbucket.org/sk_0520/pe.plugins.template/
https://bitbucket.org/sk_0520/pe/
#>
Param(
	[Parameter(mandatory = $true)][string] $ProjectDirectory,
	[Parameter(mandatory = $true)][string] $PluginName,
	[Guid] $PluginId,
	[string] $AppTargetBranch = 'master',
	[string] $GitPath = '%PROGRAMFILES%\Git\bin',
	[string] $DotNetPath = '%PROGRAMFILES%\dotnet\'
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path

$suppressBuild = $false
$suppressScm = $false

#===================================================
#プラグインIDのチェック・採番
$reservedPluginIds = [Guid[]]@(
	[Guid]'4524FC23-EBB9-4C79-A26B-8F472C05095E', # DefaultTheme
	# ------------------------------------------
	[Guid]'67F0FA7D-52D3-4889-B595-BE3703B224EB', # ClassicTheme
	[Guid]'2E5C72C5-270F-4B05-AFB9-C87F3966ECC5', # Clock
	[Guid]'799CE8BD-8F49-4E8F-9E47-4D4873084081', # Eyes
	[Guid]'9DCF441D-9F8E-494F-89C1-814678BBC42C', # FileFinder
	[Guid]'4FA1A634-6B32-4762-8AE8-3E1CF6DF9DB1'  # Html
)

$customPluginId = $PluginId
$enabledGuid = $false
while (!$enabledGuid) {
	$guid = [System.Guid]::empty
	$isGuid = [System.Guid]::TryParse($customPluginId, [System.Management.Automation.PSReference]$guid)

	if ($isGuid) {
		if ($reservedPluginIds.Contains([Guid]$customPluginId)) {
			Write-Warning ('予約済みプラグインID: ' + $customPluginId)
		}
		else {
			$enabledGuid = $true
		}
	} else {
		Write-Warning ('プラグインIDとして不正: ' + $customPluginId)
	}

	if (!$enabledGuid) {
		$generatedGuid = $false
		do {
			Write-Host 'プラグインIDを生成方法: [1] 自動, [2] 手動'
			switch ((Read-Host)) {
				1 {
					$customPluginId = New-Guid
					$generatedGuid = $true
				}
				2 {
					$customPluginId = Read-Host -Prompt "GUID入力"
					$generatedGuid = $true
				}
			}
		} while (!$generatedGuid)
	}
}

#===================================================
# 各種諸々の生成
$parameters = @{
	pluginName = $PluginName
	directory  = [System.IO.DirectoryInfo][Environment]::ExpandEnvironmentVariables($ProjectDirectory)
	git        = [Environment]::ExpandEnvironmentVariables((Join-Path $GitPath 'git.exe'))
	dotnet     = [Environment]::ExpandEnvironmentVariables((Join-Path $DotNetPath 'dotnet.exe'))
	pluginId   = $customPluginId
	source     = [System.IO.DirectoryInfo][Environment]::ExpandEnvironmentVariables((Join-Path $ProjectDirectory 'Source'))
	repository = @{
		application = @{
			path = 'Source/Pe'
			url  = [uri]'https://sk_0520@bitbucket.org/sk_0520/pe.git'
		}
	}
}

Write-Verbose 'パラメータ:'
Write-Verbose "`tProjectDirectory: ${ProjectDirectory}"
Write-Verbose "`tPluginId: ${PluginId}"
Write-Verbose "`tAppTargetBranch: ${AppTargetBranch}"
Write-Verbose "`tGitPath: ${GitPath}"
Write-Verbose "`tDotNetPath: ${DotNetPath}"

Write-Host '情報:'
$parameters | Format-Table -AutoSize
Write-Host ('git: ' + (& $parameters.git --version))
Write-Host ('dotnet: ' + (& $parameters.dotnet --version))

#===================================================
# プロジェクトディレクトリ構築
$parameters.directory.Refresh()
if (!$parameters.directory.Exists) {
	$parameters.directory.Create()
}
if ((Get-ChildItem -Path $parameters.directory -Recurse -Force | Measure-Object).Count -ne 0) {
	#	throw '指定ディレクトリが空じゃない'
}

Write-Host "プロジェクトディレクトリ生成: " + $parameters.directory
if(!$suppressScm) {
	& $parameters.git init $parameters.directory
}

# テンプレート的なのを複製(1)
$pluginProjectDirPath = Join-Path $parameters.source $PluginName
$pluginProjectFilePath = Join-Path -Path $pluginProjectDirPath -ChildPath "$PluginName.csproj"

Copy-Item -Path (Join-Path -Path $currentDirPath -ChildPath 'Template\*') -Destination ($parameters.directory.FullName + '\') -Force -Recurse
Move-Item -Path (Join-Path -Path $parameters.source 'Project' | Join-Path -ChildPath 'Plugin.csproj') -Destination (Join-Path -Path $parameters.source 'Project' | Join-Path -ChildPath "$PluginName.csproj") -Force
Move-Item -Path (Join-Path -Path $parameters.source 'Project') -Destination (Join-Path -Path $parameters.source $PluginName) -Force

function New-Submodule {
	param (
		[string] $path,
		[uri] $uri,
		[string] $branch
	)

	try {
		Push-Location $parameters.directory

		$targetPath = Join-Path $parameters.directory $path
		Write-Host "サブモジュール親ディレクトリ生成: " + $targetPath
		& $parameters.git submodule add --depth 1 --branch $branch $uri $path
	} finally {
		Pop-Location
	}
}

# Pe をサブモジュールとしてとってくる
if(!$suppressScm) {
	New-Submodule $parameters.repository.application.path $parameters.repository.application.url $AppTargetBranch
}

try {
	Push-Location $parameters.source

	Write-Verbose "ソリューション $PluginName を生成"
	& $parameters.dotnet new sln --force --name "$PluginName"

	Write-Verbose "ソリューションからAnyCPUを破棄"
	$solutionFileName = $PluginName + '.sln'
	$solutionContent = Get-Content -Path $solutionFileName `
		| Out-String -Stream `
		| Where-Object { !$_.Contains('Any CPU') }
	Set-Content -Path $solutionFileName -Value $solutionContent

	Write-Verbose "プロジェクトを追加"
	& $parameters.dotnet sln add $pluginProjectFilePath

	Write-Verbose "Peを追加"
	$appDir = Join-Path $parameters.source 'Pe' | Join-Path -ChildPath 'Source' | Join-Path -ChildPath 'Pe'
	$items = @(
		@{
			project   = 'Pe.Bridge'
			directory = 'Pe\bridge'
		},
		@{
			project   = 'Pe.Embedded'
			directory = 'Pe\bridge'
		},
		@{
			project   = 'Pe.PInvoke'
			directory = 'Pe\lib'
		},
		@{
			project   = 'Pe.Core'
			directory = 'Pe\lib'
		},
		@{
			project   = 'Pe.Plugins.DefaultTheme'
			directory = 'Pe\lib\plugins'
		},
		@{
			project   = 'Pe.Main'
			directory = 'Pe'
		}
	)
	foreach ($item in $items) {
		$projectFilePath = (Join-Path $appDir $item.project) | Join-Path -ChildPath ($item.project + '.csproj')
		& $parameters.dotnet sln add $projectFilePath --solution-folder $item.directory
	}

	Write-Verbose "NuGet 復元"
	if(!$suppressBuild) {
		& $parameters.dotnet restore
	}

	# Write-Verbose "テンプレート的なのを複製(2)"
	# $propertiesDirPath = Join-Path -Path $pluginProjectDirPath -ChildPath 'Properties'
	# New-Item -Path $propertiesDirPath -ItemType Directory -Force
	# Copy-Item -Path (Join-Path $currentDirPath 'Plugin.launchSettings.json') -Destination (Join-Path -Path $propertiesDirPath -ChildPath 'launchSettings.json') -Force

	# $appEtcDir = Join-Path -Path $appDir -ChildPath 'Pe.Main' | Join-Path -ChildPath 'etc'
	# Copy-Item -Path (Join-Path -Path $appEtcDir -ChildPath '@appsettings.debug.json') -Destination (Join-Path -Path $appEtcDir -ChildPath 'appsettings.debug.json')

	Write-Verbose "ソリューションのショートカットをルートに作成"
	$wsShell = New-Object -ComObject WScript.Shell
	$shortcut = $wsShell.CreateShortcut((Join-Path $parameters.directory ($parameters.pluginName + ".lnk")))
	$shortcut.TargetPath = Join-Path $parameters.source $solutionFileName
	$shortcut.Save()

	Write-Verbose "とりあえずのデバッグ全ビルド"
	if(!$suppressBuild) {
		& $parameters.dotnet build --configuration Debug #--runtime win-x64
	}

	Write-Verbose "はいコミット"
	if(!$suppressScm) {
		& $parameters.git add --all
		& $parameters.git commit --message "initialize $PluginName"
	}
} finally {
	Pop-Location
}
