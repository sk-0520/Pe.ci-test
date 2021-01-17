# Pe

Pe は Windows 上で動作するデスクトップユーティリティです。

正となるドキュメントは最新の <Pe>/doc/help.html とし、 Wiki は補足だったりの役割です。


# ライセンス

* WTFPL 2
* 特に断りのない限り提供ソースは WTFPL 2 扱い
* sk_0520/pe で配布されている本ソフトウェアのライセンスはソース提供者に断りなく、変更可能とする
  * 一応まぁ、将来変えるかもしれないので保険
  * WTFPL 以外で提供されたものはこの限りではない


## 注意

* 0.9.0以前(正しいコミットだと6bcf1830cc3aa5392200bb285888ead7516f2a65くらい)は必要DLLを削除したため基本的に戻し不可。
* 開発環境以外での動作は未確認。
* 0.84.0 から基盤ソフトウェアを .NET Framework を .NET Core に変更したのでファイルサイズが激増しています。

## 開発

* `dotnet --version`: [5.0.100](https://dotnet.microsoft.com/download/dotnet/5.0)
* `node -v: 15.6.0`
* `npm  -v: 7.4.0`
* nodesit:
  * `nodist 15.6.0`
  * `nodist npm 7.4.0`


