# Pe

プログラムランチャー。


# ダウンロード

[ダウンロード](https://bitbucket.org/sk_0520/pe/downloads)を参照。


# オンラインヘルプ

[Wiki](https://bitbucket.org/sk_0520/pe/wiki/Home)を参照。


# フォーラム

[Google Groups](https://groups.google.com/d/forum/pe_development)を参照。


# 課題管理

フォーラムやメールでの意見を課題として対応しています。

Bitbucket へログイン可能なアカウントを持っていれば課題を作成して頂いてもかまいません。

[Issues](https://bitbucket.org/sk_0520/pe/issues?status=new&status=open)を参照。


## 注意

* 0.9.0以前(正しいコミットだと6bcf1830cc3aa5392200bb285888ead7516f2a65くらい)は必要DLLを削除したため基本的に戻し不可。
* 開発環境以外での動作は未確認。


## CI

都合により赤かったり404だったり。

 * **current** [![Build status](https://ci.appveyor.com/api/projects/status/5pxaya16d6afx9df?svg=true)](https://ci.appveyor.com/project/sk_0520/pe)
 * **mater** [![Build status](https://ci.appveyor.com/api/projects/status/5pxaya16d6afx9df/branch/master?svg=true)](https://ci.appveyor.com/project/sk_0520/pe/branch/master)

まともなテストしてないから赤かったら時間とか構文エラーとかじゃないすかね。

### β版

[Pe-β](https://ci.appveyor.com/project/sk_0520/pe-4vo6i)

CIとは微妙に独立していますが、β版としてPeを出力しています。
なのでビルド環境が手元になくても最新版が取得できます。

プログラム自体は **リリース版** ですが開発側としてはあくまでリリース用構成で出力された **非リリース版** となります。
さらにビルド自体が Windows Server 2012 R2 という開発環境と異なるOSと謎構成(調べりゃわかるけどそこまでする価値も記載する意味ない)なのでリリース版とは動作異なるかもです。

 * **current** [![Build status](https://ci.appveyor.com/api/projects/status/ll41wy1edp5546hn?svg=true)](https://ci.appveyor.com/project/sk_0520/pe-4vo6i)
 * **mater** [![Build status](https://ci.appveyor.com/api/projects/status/ll41wy1edp5546hn/branch/master?svg=true)](https://ci.appveyor.com/project/sk_0520/pe-4vo6i/branch/master)

