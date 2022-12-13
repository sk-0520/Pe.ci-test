# Pe

[![Build Release](https://github.com/sk-0520/Pe/actions/workflows/build-release.yml/badge.svg)](https://github.com/sk-0520/Pe/actions/workflows/build-release.yml)

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

* `dotnet --version`: [7.0.100](https://dotnet.microsoft.com/download/dotnet/7.0)
* `node -v: 18.4.0`
* nvm:
  * `nvm use 18.4.0`


