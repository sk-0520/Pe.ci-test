Pe が起動する際にアプリケーション構成ファイルを読み取り、各種固定値を設定します。アプリケーション構成ファイルに対するエラー、不正な設定値は Pe の実行に悪影響を与えます。

配置場所は `<Pe>\etc\` で、以下の順序で読み込みを行います。


| 順序 | ファイル名 | 備考 |
|--:|---|---|
| 1 | `appsettings.json` | **必須ファイルです** |
| 2 | `appsettings.debug.json` | デバッグで(`#define DEBUG`) 読み込まれ、それ以外では無視される。<br />開発時に `@appsettings.debug.json` の @ を外して使用する想定 |
| 2 | `appsettings.beta.json` | β版で(`#define BETA`) 読み込まれ、それ以外では無視される |
| 3 | `appsettings.user.json` | ユーザー独自設定 |
| 4 | `appsettings.<X-XX-XXX>.json` | 指定バージョン限定設定<br />基本的にユーザー使用は想定せず |

あとから読み込まれたデータが優先されます。

> [!TIP]
> 順序 2 の debug/beta は共生しない。
