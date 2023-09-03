<#
.SYNOPSIS
Pe プラグインのテンプレート作成処理。


.DESCRIPTION
Pe リポジトリからいい感じのあれこれを取ってきてあれこれするんよ。


.PARAMETER ProjectDirectory
プラグインテンプレートプロジェクトのルートディレクトリパス

.PARAMETER PluginName
プロジェクト名

.PARAMETER PluginId
プラグインID

.PARAMETER DefaultNamespace
名前空間

.PARAMETER AppTargetBranch
対象 Pe のブランチ

.PARAMETER GitPath
環境変数PATH に割り当てる git がインストールされているパス

.PARAMETER DotNetPath
環境変数PATH に割り当てる dotnet がインストールされているパス

.LINK
https://github.com/sk-0520/Pe
#>
# Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process
Param(
	[Parameter(mandatory = $true)][string] $ProjectDirectory,
	[Parameter(mandatory = $true)][string] $PluginName,
	[Guid] $PluginId,
	[string] $DefaultNamespace,
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
class PluginIdentity {
	[Guid] $PluginId;
	[string] $PluginName;

	PluginIdentity([Guid] $pluginId, [string] $pluginName) {
		$this.PluginId = $pluginId
		$this.PluginName = $pluginName
	}
}
$reservedPluginIds = [PluginIdentity[]]@(
	[PluginIdentity]::new('4524FC23-EBB9-4C79-A26B-8F472C05095E', 'ContentTypeTextNet.Pe.Plugins.DefaultTheme'),
	# ------------------------------------------
	[PluginIdentity]::new('67F0FA7D-52D3-4889-B595-BE3703B224EB', 'ContentTypeTextNet.Pe.Plugins.Reference.ClassicTheme'),
	[PluginIdentity]::new('2E5C72C5-270F-4B05-AFB9-C87F3966ECC5', 'ContentTypeTextNet.Pe.Plugins.Reference.Clock'),
	[PluginIdentity]::new('799CE8BD-8F49-4E8F-9E47-4D4873084081', 'ContentTypeTextNet.Pe.Plugins.Reference.Eyes'),
	[PluginIdentity]::new('9DCF441D-9F8E-494F-89C1-814678BBC42C', 'ContentTypeTextNet.Pe.Plugins.Reference.FileFinder'),
	[PluginIdentity]::new('4FA1A634-6B32-4762-8AE8-3E1CF6DF9DB1', 'ContentTypeTextNet.Pe.Plugins.Reference.Html')
)

$customPluginId = $PluginId
$enabledGuid = $false
while (!$enabledGuid) {
	$guid = [System.Guid]::empty
	$isGuid = [System.Guid]::TryParse($customPluginId, [System.Management.Automation.PSReference]$guid)

	if ($isGuid) {
		$reservedIds = $reservedPluginIds | Select-Object -ExpandProperty 'PluginId'
		if ($reservedIds.Contains([Guid]$customPluginId)) {
			Write-Warning ('予約済みプラグインID: ' + $customPluginId)
		}
		else {
			$enabledGuid = $true
		}
	}
 else {
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

$reservedNames = $reservedPluginIds | Select-Object -ExpandProperty 'PluginName'
if ($reservedNames.Contains($PluginName)) {
	throw ('予約済みプラグイン名: ' + $PluginName)
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
			url  = [uri]'https://github.com/sk-0520/Pe.git'
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

#---------------------------------------------------
function Update-TemplateValue([string] $value) {
	return [Regex]::Replace($value, '\bTEMPLATE_([\w\d_]+)\b', {
			$namespace = 'TEMPLATE_Namespace'
			if (![string]::IsNullOrEmpty($DefaultNamespace)) {
				$namespace = $DefaultNamespace.Trim();
			}

			$pluginShortName = $parameters.pluginName
			if ($parameters.pluginName.Contains('.')) {
				$pluginShortName = $parameters.pluginName.Split('.')[-1]
			}

			$map = @{
				'Namespace'       = $namespace
				'PluginName'      = $parameters.pluginName
				'PluginShortName' = $pluginShortName
				'PluginId'        = $parameters.pluginId
			}

			return $map[$args.Groups[1].Value]
		})
}

function Update-TemplateFileContent([System.IO.FileInfo] $file) {
	Write-Verbose $file.FullName
	$encoding = switch ($file.Extension.ToLowerInvariant()) {
		".bat" {
			'oem'
		}
		Default {
			'UTF8'
		}
	}

	$newContents = Get-Content -Path $file.FullName -Encoding $encoding | ForEach-Object { Update-TemplateValue $_ }
	Set-Content -Path $file.FullName -Value $newContents -Encoding $encoding
}

function Rename-TemplateFileName([System.IO.DirectoryInfo] $parentDirectory, [string] $name) {
	$newName = Update-TemplateValue $name
	if ($newName.StartsWith('__.')) {
		$newName = $newName.SubString(2)
	}
	if ($name -ne $newName) {
		Write-Verbose "Rename-TemplateFileName: [$($parentDirectory.FullName)] $name -> $newName"
		$src = (Join-Path -Path $parentDirectory.FullName -ChildPath $name)
		$dst = (Join-Path -Path $parentDirectory.FullName -ChildPath $newName)
		Move-Item -Path $src -Destination $dst
	}
}

function Rename-Names([System.IO.DirectoryInfo] $directory) {
	Write-Verbose "Rename-Names: $($directory.FullName)"
	$dirs = Get-ChildItem -Path $directory.FullName -Directory
	foreach ($dir in $dirs) {
		Rename-Names($dir)
		Rename-TemplateFileName $directory $dir.Name
	}

	$files = Get-ChildItem -Path $directory.FullName -File
	foreach ($file in $files) {
		Update-TemplateFileContent $file
		Rename-TemplateFileName $directory $file.Name
	}
}

#===================================================
# プロジェクトディレクトリ構築
$parameters.directory.Refresh()
if (!$parameters.directory.Exists) {
	$parameters.directory.Create()
}
if ((Get-ChildItem -Path $parameters.directory -Recurse -Force | Measure-Object).Count -ne 0) {
	throw '指定ディレクトリが空じゃない'
}

Write-Host "プロジェクトディレクトリ生成: " + $parameters.directory
if (!$suppressScm) {
	& $parameters.git init $parameters.directory
}

Copy-Item -Path (Join-Path -Path $currentDirPath -ChildPath 'Template\*') -Destination ($parameters.directory.FullName + '\') -Force -Recurse

Rename-Names $parameters.directory

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
		& $parameters.git submodule add --branch $branch $uri $path
	}
 finally {
		Pop-Location
	}
}

# Pe をサブモジュールとしてとってくる
if (!$suppressScm) {
	New-Submodule $parameters.repository.application.path $parameters.repository.application.url $AppTargetBranch
}

try {
	Push-Location $parameters.source

	$pluginTargets = @(
		Join-Path -Path $parameters.source -ChildPath $PluginName
		Join-Path -Path $parameters.source -ChildPath "${PluginName}.Test"
	)

	foreach ($pluginTarget in $pluginTargets) {
		Write-Verbose "プロジェクトを追加: $pluginTarget"
		& $parameters.dotnet sln add $pluginTarget
	}

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
			project   = 'Pe.Standard.Base'
			directory = 'Pe\lib\standard'
		},
		@{
			project   = 'Pe.Standard.Database'
			directory = 'Pe\lib\standard'
		},
		@{
			project   = 'Pe.Standard.DependencyInjection'
			directory = 'Pe\lib\standard'
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

	$solutionFileName = "${PluginName}.sln"
	$solutionPathName = Join-Path $parameters.source -ChildPath $solutionFileName

	# Write-Verbose "ソリューションからAnyCPUを破棄"
	# $solutionFileName = Update-TemplateValue 'TEMPLATE_PluginShortName.sln'
	# $solutionContent = Get-Content -Path $solutionFileName `
	# 	| Out-String -Stream `
	# 	| Where-Object { !$_.Contains('Any CPU') }
	# Set-Content -Path $solutionFileName -Value $solutionContent


	Write-Verbose "NuGet 復元"
	if (!$suppressBuild) {
		& $parameters.dotnet restore
	}

	Write-Verbose "プラグイン起動設定追加"
	$pluginPropertyDir = Join-Path -Path $parameters.source -ChildPath $PluginName | Join-Path -ChildPath 'Properties'
	Copy-Item -Path (Join-Path -Path $pluginPropertyDir -ChildPath 'dev-launchSettings.json') -Destination (Join-Path -Path $pluginPropertyDir -ChildPath 'launchSettings.json')

	Write-Verbose "アプリケーションデバッグ構成ファイル適用"
	$appEtcDir = Join-Path -Path $appDir -ChildPath 'Pe.Main' | Join-Path -ChildPath 'etc'
	Copy-Item -Path (Join-Path -Path $appEtcDir -ChildPath '@appsettings.debug.json') -Destination (Join-Path -Path $appEtcDir -ChildPath 'appsettings.debug.json')

	Write-Verbose "ソリューションのショートカットをルートに作成"
	$wsShell = New-Object -ComObject WScript.Shell
	$shortcut = $wsShell.CreateShortcut((Join-Path $parameters.directory ($parameters.pluginName + ".lnk")))
	$shortcut.TargetPath = $solutionPathName
	$shortcut.Save()

	if (!$suppressBuild) {
		Write-Verbose "とりあえずのデバッグ全ビルド"
		& $parameters.dotnet build --configuration Debug /p:Platform=x64 -Rebuild
	}

	if (!$suppressScm) {
		Write-Verbose "はいコミット"
		& $parameters.git add --all
		& $parameters.git commit --message "initialize $PluginName"
	}
}
finally {
	Pop-Location
}
