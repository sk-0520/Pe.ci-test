> [!IMPORTANT]
> プラグインは Pe の実行権限で実行されます。

> [!WARNING]
> インストールされたプラグインが悪意ある処理を実行した場合に Pe 側で防御する術はありません。

# プラグイン機能

プラグイン機能により Pe の機能拡張を行えます。実装については <MdLink page="dev.plugin" /> を確認してください

# 配布サイト

以下がプラグイン配布サイトの総本山です。

[Pe.Server](https://peserver.site/plugin)

# インストール方法

## ローカルインストール

設定 → プラグイン から <MdInline kind="ui">ローカル</MdInline> を選択してアーカイブを選択することで次回起動時に有効になります。

## Webインストール

設定 → プラグイン から <MdInline kind="ui">Web</MdInline> を選択してプラグインIDか更新チェックURLを指定することで次回起動時に有効になります。

## 手動インストール

プラグインを指定ディレクトリに配置することにより(多分)活性化します。

基本の配置先は <MdPath>%LOCALAPPDATA%\Pe\plugin\modules</MdPath> で、<br />
**プラグインID or プラグインディレクトリ名** として配置します。

PLUGIN.dll というプラグインがあれば、

1. <MdPath>%LOCALAPPDATA%\Pe\plugin\modules\プラグインID\PLUGIN.dll</MdPath>
2. <MdPath>%LOCALAPPDATA%\Pe\plugin\modules\PLUGIN\PLUGIN.dll</MdPath>

として配置します。

プラグインディレクトリ名による配置は問題を起こす可能性があるのでプラグインIDによる配置を推奨します。

# 参考実装

プラグインの参考実装成果物は [Pe 配布場所](https://github.com/sk-0520/Pe/releases/latest) に最新版を配置しています。

> [!NOTE]
> 配置されている参考実装プラグインは Pe のリリース時に最新化されます。

> [!WARNING]
> 参考実装プラグインは Pe のリリース(CD/CD処理)に合わせてリリースされるため不具合等があったとしても随時/緊急リリースは行われません。
> 
> とりあえずこうすればこう動くといった実装のため下位互換や効率等は度外視したものになります。

