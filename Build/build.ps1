$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path

# 独自変数をいい感じに合わせる
$global:app = @{
    'version' = '0.0.0'
    'revision' = ''
}

$appVarScriptPath = Join-Path $currentDirPath 'set-appenv.ps1'
Invoke-Expression $appVarScriptPath

Write-Output $global:app


# SCM 的に現行状態に未コミットがあれば死ぬ

