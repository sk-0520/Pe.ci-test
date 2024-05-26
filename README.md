# Pe

![GitHub tag (latest by date)](https://img.shields.io/github/v/tag/sk-0520/Pe?color=orange&label=version)
[![Build CI/CD](https://github.com/sk-0520/Pe/actions/workflows/build-ci-cd.yml/badge.svg?branch=master)](https://github.com/sk-0520/Pe/actions/workflows/build-ci-cd.yml)
[![Build CI](https://github.com/sk-0520/Pe/actions/workflows/build-ci-only.yml/badge.svg)](https://github.com/sk-0520/Pe/actions/workflows/build-ci-only.yml)
![license](https://img.shields.io/github/license/sk-0520/Pe?style=flat)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/sk-0520/Pe)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=sk-0520_Pe&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=sk-0520_Pe)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=sk-0520_Pe&metric=bugs)](https://sonarcloud.io/summary/new_code?id=sk-0520_Pe)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=sk-0520_Pe&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=sk-0520_Pe)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=sk-0520_Pe&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=sk-0520_Pe)

Pe は Windows 10 以上で動作するデスクトップユーティリティです。

各種ドキュメントは最新版の `<Pe>/doc/help.html` が正となります。

# ライセンス

* WTFPL 2

# ダウンロード

* 自動で取得するには[こちら](https://github.com/sk-0520/Pe.Installer/releases/latest/download/Pe.Installer.exe) ※別プロジェクトのインストーラ
* 手動で取得するには[リリース](https://github.com/sk-0520/Pe/releases)を参照。

## 注意

* 0.9.0以前(正しいコミットだと6bcf1830cc3aa5392200bb285888ead7516f2a65くらい)は必要DLLを削除したため基本的に戻し不可。
* 開発環境以外での動作は未確認。
* 0.84.0 から基盤ソフトウェアを .NET Framework を .NET Core に変更したのでファイルサイズが激増しています。

## 開発

* `dotnet`: [8](https://dotnet.microsoft.com/download/dotnet/8.0)
* `node`: `<Pe.git>/.node-version` 参照

### ローカルでのテスト(C#)

1. リポジトリルートへ移動
1. reportgenerator のローカルインストール  
   `PS> dotnet tool install dotnet-reportgenerator-globaltool --tool-path _tools`
1. テスト実行  
   `PS1> .\Build\local-main-test-.ps1 -Project Pe.Main.Test`
   * `Source\Pe` 内の `*.Test` プロジェクトを指定
   * 複数指定は `,` を使用する
   * 全て指定する場合は `*` を使用する
   * ディレクトリ名だけをあてにしているのでゴミが残ってたりすると変になることに注意

### ローカルでのドキュメントコメント生成(C#)

1. リポジトリルートへ移動
1. reportgenerator のローカルインストール  
   `PS> dotnet tool install docfx --tool-path _tools`
1. 生成
   `PS1> .\_tools\docfx docfx.json --template statictoc
