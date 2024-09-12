> [!CAUTION]
> [プラグインテンプレートのリリース周りを node20 追従](https://github.com/sk-0520/Pe/issues/917)

> [!WARNING]
> 作業中

プラグイン生成テンプレートを用いることでプラグインのひな形を簡単に生成する。

[💾テンプレートを取得する](./archives/plugin-template.zip)

# 実行方法

Powershell で <MdPath>create-project.ps1</MdPath> を実行することでプラグインプロジェクトが生成される。

以下コマンドでスクリプトドキュメントを表示。

`Get-Help .\create-project.ps1 -full`

> [!WARNING]
> PowerShell 処理書き換え云々で追い付いていないのでダメかも(0.99.215～)

> [!WARNING]
> ドライブ直下に作れたり作れなかったりする問題ありだけど🐛無視無視🐞

> [!TIP]
> 必要に応じてセキュリティ設定を一時的にファイル読み込み可能にする必要あり。
>
> `Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process`

## パラメータ生成

<!-- 呼び出しコンポーネント側実装 -->
