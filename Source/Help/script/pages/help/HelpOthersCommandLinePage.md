# コマンドライン書式

コマンドライン引数として識別するパターンは以下で始まるものです。

* `--OPTION`
* `/OPTION`

値を指定する場合は以下の形式をサポートします

* `␣` (半角スペース)
* `=`

値に␣(半角スペース)を含む場合、"で囲うことにより一つの値として扱えます

スイッチ形式は値を指定する必要はありません。

## 例

* `--option value`
* `--option=value`
* `/option=value`
* `--option "space value2"`
* `--option="space value2"`
* `"--option=space value2"`
* `--switch`

> [!WARNING]
> `/option` 形式は将来的に整理予定なので `--option` の使用を推奨します。

# 各値

| キー | 値 | 説明 |
|---|---|---|
| user-dir | 必要 | 設定ディレクトリパス |
| machine-dir | 必要 | 端末ディレクトリパス |
| temp-dir | 必要 | 一時ディレクトリパス |
| app-log-limit | 必要 | o	Pe 内部で保持するログ数を設定<br />1 未満を指定するか数値として不正な値を指定した場合は自動設定される。 |
| log | 必要 | ログ出力先。 拡張子が存在すればファイルと判断してログ出力を行い、拡張子がなければ指定パスを親ディレクトリとしてYYYY-MM-DD_hhmmss.logディレクトリが存在しない場合、出力は行われない<br />拡張子ありの場合、その拡張子に合わせて出力形式が変動する<br /> <dl><dt>**log**</dt><dd>テキストファイルとしてログ出力</dd><dt>**xml**</dt><dd>XMLとしてログ出力</dd> | <!-- </dl> 付けるとおっかしなことになる -->
| with-log | 必要 | `log` で出力するログ形式に合わせて出力する形式<br /><MdInline kind="sample">--log X:\logs\output.log --with-log xml</MdInline> と指定した場合、<br />`X:\logs\output.log` にログを出しつつ、<br />XML形式ログ `X:\logs\output.xml` も出力する |
| full-trace-log | 不要 | めっちゃくちゃログを取得するか。SQL文からキー押下までありとあらゆる何かを出力する<br />開発中に使用する想定 |
| force-log | 不要 | 通常では指定ログディレクトリが存在しない場合にログを出力しないが、本スイッチによりログディレクトリを生成する |
| skip-accept | 不要 | 使用許諾をスキップする<br />指定した場合、使用許諾は承諾されたものとする<br />開発中(or β版実行)に使用する想定 |
| beta-version | 不要 | β版を明示的に使用する旨を Pe に通知。 |

# 動作解説

諸々の面倒な事情によりパラメータは `<Pe>\Pe.exe` から `<Pe>\bin\Pe.Main.exe` に流されていきます。

アップデート時は起動時のコマンドライン引数を `<Pe>\bin\Pe.Main.exe` から `<Pe>\etc\script\update\update-application.ps1` に渡して、最終的に `<Pe>\Pe.exe` に流されます。

色々あるねん。
