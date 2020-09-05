Param(
	[switch] $ProductMode,
	[switch] $IgnoreChanged,
	[string] $BuildType,
	[parameter(mandatory = $true)][string[]] $Platforms
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'command.ps1',
	'version.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path $currentDirPath $scriptFileName
	. $scriptFilePath
}
$rootDirectory = Split-Path -Path $currentDirPath -Parent

Write-Output "ProductMode = $ProductMode"
Write-Output "IgnoreChanged = $IgnoreChanged"
Write-Output "BuildType = $BuildType"
Write-Output "Platforms = $Platforms"
Write-Output ""

Write-Output ("git: " + (git --version))
Write-Output ("msbuild: " + (msbuild -version -noLogo))
Write-Output ("dotnet: " + (dotnet --version))

if (!$IgnoreChanged) {
	# SCM 的に現行状態に未コミットがあれば死ぬ
	try {
		git update-index --assume-unchanged Source/Pe/Pe.Main/doc/help/index.html
		if ((git status -s | Measure-Object).Count -ne 0) {
			git status -s
			throw "変更あり"
		}
	} finally {
		git update-index --no-assume-unchanged Source/Pe/Pe.Main/doc/help/index.html
	}
}

# リリース時のアナライザ系削除
$removePackageTargets = @(
	'ClrHeapAllocationAnalyzer',
	'SonarAnalyzer.CSharp'
)

try {
	Push-Location $rootDirectory

	$version = GetAppVersion
	$revision = (git rev-parse HEAD)

	function InsertElement([string] $value, [xml] $xml, [string] $targetXpath, [string] $parentXpath, [string] $elementName) {
		$element = $xml.SelectSingleNode($targetXpath);
		if ($null -eq $element) {
			$propGroup = $xml.SelectSingleNode($parentXpath)
			$element = $xml.CreateElement($elementName);
			$propGroup.AppendChild($element) | Out-Null;
			$element.InnerText = $value
		}
	}

	function ReplaceElement([hashtable] $map, [xml] $xml, [string] $targetXpath, [string] $parentXpath, [string] $elementName) {
		$element = $xml.SelectSingleNode($targetXpath);
		if ($null -ne $element) {
			$val = $element.InnerText
			foreach ($key in $map.keys) {
				$val = $val.Replace($key, $map[$key])
			}
			$element.InnerText = $val
		}
	}

	$projectFiles = (Get-ChildItem -Path "Source\Pe\" -Recurse -Include *.csproj)
	if (!$IgnoreChanged) {
		Write-Output "change ..."
		foreach ($projectFile in $projectFiles) {
			Write-Output "        -> $projectFile"
		}
		Write-Output "        -> App.ico"
	}

	foreach ($projectFile in $projectFiles) {
		Write-Output $projectFile.Name
		$xml = [XML](Get-Content $projectFile  -Encoding UTF8)

		InsertElement $version $xml '/Project/PropertyGroup[1]/Version[1]' '/Project/PropertyGroup[1]' 'Version'
		InsertElement $revision $xml '/Project/PropertyGroup[1]/InformationalVersion[1]' '/Project/PropertyGroup[1]' 'InformationalVersion'
		$repMap = @{
			'@YYYY@' = '2020'
			'@NAME@' = 'sk'
			'@SITE@' = 'content-type-text.net'
		}
		ReplaceElement $repMap $xml '/Project/PropertyGroup[1]/Copyright[1]' '/Project/PropertyGroup[1]' 'Copyright'

		# リリース版は各種アナライザを除去しておく
		if( $BuildType -eq '') {
			$elements = $xml.SelectNodes('/Project/*/PackageReference')
			foreach($element in $elements) {
				$includeElement = $element.Attributes['Include']
				if($null -eq $includeElement) {
					continue
				}

				$includeValue = $includeElement.Value

				if($removePackageTargets.Contains($includeValue)) {
					Write-Output "remove $includeValue"
					$element.ParentNode.RemoveChild($element)
				}
			}
		}

		$xml.Save($projectFile)
	}

	# アイコンファイルの差し替え
	$appIconName = switch ($BuildType) {
		'BETA' { 'App-beta.ico' }
		'' { 'App-release.ico' }
		Default { 'App-debug.ico' }
	}
	Write-Output "icon: $appIconName"
	$appIconPath = Join-Path 'Resource\Icon' $appIconName

	Copy-Item -Path $appIconPath -Destination 'Source\Pe\Pe.Main\Resources\Icon\App.ico' -Force
	Copy-Item -Path $appIconPath -Destination 'Source\Pe\Pe.Plugins.DefaultTheme\App.ico' -Force

	# ビルド開始
	$defines = @()
	if ( $BuildType ) {
		$defines += $BuildType
	}
	if ( $ProductMode ) {
		$defines += 'PRODUCT'
	}
	# ; を扱う https://docs.microsoft.com/ja-jp/visualstudio/msbuild/msbuild-special-characters?view=vs-2015&redirectedfrom=MSDN
	$define = $defines -join '%3B'

	foreach ($platform in $Platforms) {
		msbuild        Source/Pe.Boot/Pe.Boot.sln       /m                   /p:Configuration=Release /p:Platform=$platform /p:DefineConstants=$define /t:Rebuild
		dotnet build   Source/Pe/Pe.sln                 /m --verbosity normal --configuration Release /p:Platform=$platform /p:DefineConstants=$define --runtime win-$platform
		dotnet publish Source/Pe/Pe.Main/Pe.Main.csproj /m --verbosity normal --configuration Release /p:Platform=$platform /p:DefineConstants=$define --runtime win-$platform --output Output/Release/$platform/Pe/bin --self-contained true

		if ($ProductMode) {
			$productTargets = @('etc', 'doc', 'bat')

			# 本番用データ配置のため不要ファイル破棄
			foreach ($productTarget in $productTargets) {
				$target = Join-Path "Output/Release/$platform/Pe/bin" $productTarget
				Remove-Item -Path $target -Recurse -Force
			}

			# 本番用データ配置のため必要ファイルの移送
			foreach ($productTarget in $productTargets) {
				$src = Join-Path "Source/Pe/Pe.Main"           $productTarget
				$dst = Join-Path "Output/Release/$platform/Pe" $productTarget
				robocopy /MIR /PURGE /R:3 /S "$src" "$dst"
			}

			# 構成ファイル(appsettings.*.json)の整理
			$appSettingRootDir = "Output/Release/$platform/Pe/etc"
			if($BuildType -ne "BETA") {
				Remove-Item -Path (Join-Path $appSettingRootDir "appsettings.beta.json")  -Force
			}
			Remove-Item -Path (Join-Path $appSettingRootDir "@appsettings.debug.json")  -Force
			# ローカルでビルドする場合は高確率で debug.json がいるので対応しておく
			$debugAppSettingPath = Join-Path $appSettingRootDir "appsettings.debug.json"
			if(Test-Path $debugAppSettingPath) {
				Remove-Item -Path $debugAppSettingPath -Force
			}

			# ライブラリの移送
			robocopy /R:3 /S /E "Resource\Library\" "Output\Release\$platform\Pe\bin\lib\"
		}
	}
}
finally {
	if (!$IgnoreChanged) {
		git reset --hard
	}
	Pop-Location
}
