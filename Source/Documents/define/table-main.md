# メインテーブル一覧





___

## AppSystems

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント               |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:-----------------------|
| o  | o  |    | キー                     | Key                   | text         | System.String   |              |                        |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                    |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                        |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                        |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                        |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                    |
|    | o  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                        |
|    | o  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                        |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                        |
|    | o  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり                |
|    | o  |    | 値                       | Value                 | text         | System.String   |              |                        |
|    | o  |    | コメント                 | Comment               | text         | System.String   |              | Peからは使用しないメモ |

### index

*NONE*



___

## LauncherItems

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名             | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:---|:-------------------------|:-------------------------|:-------------|:----------------|:-------------|:---------|
| o  | o  |    | ランチャーアイテムID     | LauncherItemId           | text         | System.Guid     |              |          |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp         | datetime     | System.DateTime |              | UTC      |
|    | o  |    | 作成ユーザー名           | CreatedAccount           | text         | System.String   |              |          |
|    | o  |    | 作成プログラム名         | CreatedProgramName       | text         | System.String   |              |          |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion    | text         | System.Version  |              |          |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp         | datetime     | System.DateTime |              | UTC      |
|    | o  |    | 更新ユーザー名           | UpdatedAccount           | text         | System.String   |              |          |
|    | o  |    | 更新プログラム名         | UpdatedProgramName       | text         | System.String   |              |          |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion    | text         | System.Version  |              |          |
|    | o  |    | 更新回数                 | UpdatedCount             | integer      | System.Int64    |              | 0始まり  |
|    | o  |    | ランチャーアイテムコード | Code                     | text         | System.String   |              |          |
|    | o  |    | 名称                     | Name                     | text         | System.String   |              |          |
|    | o  |    | ランチャー種別           | Kind                     | text         | System.String   |              |          |
|    | o  |    | アイコンパス             | IconPath                 | text         | System.String   |              |          |
|    | o  |    | アイコンインデックス     | IconIndex                | integer      | System.Int64    |              |          |
|    | o  |    | コマンド入力対象         | IsEnabledCommandLauncher | boolean      | System.Boolean  |              |          |
|    | o  |    | 使用回数                 | ExecuteCount             | integer      | System.Int64    |              |          |
|    | o  |    | 最終仕様日時             | LastExecuteTimestamp     | datetime     | System.DateTime |              | UTC      |
|    | o  |    | メモ                     | Comment                  | text         | System.String   |              |          |

### index

| UK | カラム(CSV) |
|:--:|:------------|
| o  | Code        |



___

## LauncherFiles

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名            | 論理データ型 | マッピング型    | チェック制約 | コメント                                               |
|:--:|:--:|:---|:-------------------------|:------------------------|:-------------|:----------------|:-------------|:-------------------------------------------------------|
| o  | o  |    | ランチャーアイテムID     | LauncherItemId          | text         | System.Guid     |              |                                                        |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp        | datetime     | System.DateTime |              | UTC                                                    |
|    | o  |    | 作成ユーザー名           | CreatedAccount          | text         | System.String   |              |                                                        |
|    | o  |    | 作成プログラム名         | CreatedProgramName      | text         | System.String   |              |                                                        |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion   | text         | System.Version  |              |                                                        |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp        | datetime     | System.DateTime |              | UTC                                                    |
|    | o  |    | 更新ユーザー名           | UpdatedAccount          | text         | System.String   |              |                                                        |
|    | o  |    | 更新プログラム名         | UpdatedProgramName      | text         | System.String   |              |                                                        |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion   | text         | System.Version  |              |                                                        |
|    | o  |    | 更新回数                 | UpdatedCount            | integer      | System.Int64    |              | 0始まり                                                |
|    | o  |    | コマンド                 | Command                 | text         | System.String   |              |                                                        |
|    | o  |    | コマンドオプション       | Option                  | text         | System.String   |              |                                                        |
|    | o  |    | 作業ディレクトリ         | WorkDirectory           | text         | System.String   |              |                                                        |
|    | o  |    | 環境変数使用             | IsEnabledCustomEnvVar   | boolean      | System.Boolean  |              |                                                        |
|    | o  |    | 標準出力使用             | IsEnabledStandardOutput | boolean      | System.Boolean  |              | 標準エラーも同じ扱い                                   |
|    | o  |    | 標準入力使用             | IsEnabledStandardInput  | boolean      | System.Boolean  |              |                                                        |
|    | o  |    | 実行権限                 | Permission              | text         | System.String   |              | 通常, 管理者, 別ユーザー                               |
|    | o  |    | 別ユーザーアカウント     | CredentId               | text         | System.Guid     |              | 実行権限が別ユーザーの場合に有効, まぁほとんど予約かな |

### index

*NONE*



___

## LauncherCommands

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名            | 論理データ型 | マッピング型    | チェック制約 | コメント                                               |
|:--:|:--:|:---|:-------------------------|:------------------------|:-------------|:----------------|:-------------|:-------------------------------------------------------|
| o  | o  |    | ランチャーアイテムID     | LauncherItemId          | text         | System.Guid     |              |                                                        |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp        | datetime     | System.DateTime |              | UTC                                                    |
|    | o  |    | 作成ユーザー名           | CreatedAccount          | text         | System.String   |              |                                                        |
|    | o  |    | 作成プログラム名         | CreatedProgramName      | text         | System.String   |              |                                                        |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion   | text         | System.Version  |              |                                                        |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp        | datetime     | System.DateTime |              | UTC                                                    |
|    | o  |    | 更新ユーザー名           | UpdatedAccount          | text         | System.String   |              |                                                        |
|    | o  |    | 更新プログラム名         | UpdatedProgramName      | text         | System.String   |              |                                                        |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion   | text         | System.Version  |              |                                                        |
|    | o  |    | 更新回数                 | UpdatedCount            | integer      | System.Int64    |              | 0始まり                                                |
|    | o  |    | コマンド                 | Command                 | text         | System.String   |              |                                                        |
|    | o  |    | コマンドオプション       | Option                  | text         | System.String   |              |                                                        |
|    | o  |    | 作業ディレクトリ         | WorkDirectory           | text         | System.String   |              |                                                        |
|    | o  |    | 環境変数使用             | IsEnabledCustomEnvVar   | boolean      | System.Boolean  |              |                                                        |
|    | o  |    | 標準出力使用             | IsEnabledStandardOutput | boolean      | System.Boolean  |              | 標準エラーも同じ扱い                                   |
|    | o  |    | 標準入力使用             | IsEnabledStandardInput  | boolean      | System.Boolean  |              |                                                        |
|    | o  |    | 実行権限                 | Permission              | text         | System.String   |              | 通常, 管理者, 別ユーザー                               |
|    | o  |    | 別ユーザーアカウント     | CredentId               | text         | System.Guid     |              | 実行権限が別ユーザーの場合に有効, まぁほとんど予約かな |

### index

*NONE*



___

## LauncherDirectories

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| o  | o  |    | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |          |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |          |
|    | o  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |          |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |          |
|    | o  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり  |
|    | o  |    | ディレクトリパス         | Directory             | text         | System.String   |              |          |

### index

*NONE*



___

## LauncherDirectoryItems

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント                     |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:-----------------------------|
|    | o  |    | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |                              |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                          |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                              |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                              |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                              |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                          |
|    | o  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                              |
|    | o  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                              |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                              |
|    | o  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり                      |
|    | o  |    | パターン                 | Pattern               | text         | System.String   |              | 正規表現                     |
|    | o  |    | 対象                     | Target                | text         | System.String   |              | ファイル・ディレクトリ・両方 |
|    | o  |    | ホワイトリスト           | Whitelist             | boolean      | System.Boolean  |              |                              |
|    | o  |    | 並び順                   | Sort                  | integer      | System.Int64    |              |                              |

### index

*NONE*



___

## LauncherEnvVars

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント              |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:----------------------|
| o  | o  |    | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |                       |
| o  | o  |    | 環境変数名               | EnvName               | text         | System.String   |              |                       |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                   |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                       |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                       |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                       |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                   |
|    | o  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                       |
|    | o  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                       |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                       |
|    | o  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり               |
|    | o  |    | 使用種別                 | Kind                  | text         | System.String   |              | 追加, 置き換え, 削除  |
|    | o  |    | 環境変数値               | EnvValue              | text         | System.String   |              | 追加, 置き換え で使用 |

### index

*NONE*



___

## LauncherTags

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| o  | o  |    | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |          |
| o  | o  |    | タグ名                   | TagName               | text         | System.String   |              |          |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |          |
|    | o  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |          |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |          |
|    | o  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり  |

### index

*NONE*



___

## LauncherItemHistories

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント                     |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:-----------------------------|
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                          |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                              |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                              |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                              |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                          |
|    | o  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                              |
|    | o  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                              |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                              |
|    | o  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり                      |
|    | o  |    | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |                              |
|    | o  |    | 履歴種別                 | Kind                  | text         | System.String   |              | オプション, 作業ディレクトリ |
|    | o  |    | 値                       | Value                 | text         | System.String   |              |                              |
|    | o  |    | 最終使用日時             | LastExecuteTimestamp  | datetime     | System.DateTime |              |                              |

### index

*NONE*



___

## Credents

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント                                                                             |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:-------------------------------------------------------------------------------------|
| o  | o  |    | 資格情報ID               | CredentId             | text         | System.Guid     |              |                                                                                      |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                                                                                  |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                                                                                      |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                                                                                      |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                                                                                      |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                                                                                  |
|    | o  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                                                                                      |
|    | o  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                                                                                      |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                                                                                      |
|    | o  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり                                                                              |
|    | o  |    | 名前                     | Name                  | text         | System.String   |              |                                                                                      |
|    | o  |    | パスワード               | Password              | text         | System.String   |              | DB上でパスワードの保護までは行わない。アプリ側でマスタパスワードを基に暗号化する方針 |

### index

*NONE*



___

## Fonts

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| o  | o  |    | フォントID               | FontId                | text         | System.Guid     |              |          |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |          |
|    | o  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |          |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |          |
|    | o  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり  |
|    | o  |    | フォントファミリー       | FamilyName            | text         | System.String   |              |          |
|    | o  |    | 高さ                     | Height                | real         | System.Double   |              |          |
|    | o  |    | ボールド                 | Bold                  | boolean      | System.Boolean  |              |          |
|    | o  |    | イタリック               | Italic                | boolean      | System.Boolean  |              |          |
|    | o  |    | 下線                     | Underline             | boolean      | System.Boolean  |              |          |
|    | o  |    | 取り消し線               | Strike                | boolean      | System.Boolean  |              |          |

### index

*NONE*



___

## LauncherGroups

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント  |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:----------|
| o  | o  |    |                          | LauncherGroupId       | text         | System.Guid     |              |           |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC       |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |           |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |           |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |           |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC       |
|    | o  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |           |
|    | o  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |           |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |           |
|    | o  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり   |
|    | o  |    | グループ名               | Name                  | text         | System.String   |              |           |
|    | o  |    | グループ種別             | Kind                  | text         | System.String   |              |           |
|    | o  |    | イメージ                 | ImageName             | text         | System.String   |              |           |
|    | o  |    | 色                       | ImageColor            | text         | System.String   |              | #AARRGGBB |
|    | o  |    | 並び順                   | Sort                  | integer      | System.Int64    |              |           |

### index

*NONE*



___

## LauncherGroupItems

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |          |
|    | o  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |          |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |          |
|    | o  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり  |
|    | o  |    | ランチャーグループID     | LauncherGroupId       | text         | System.Guid     |              |          |
|    | o  |    | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |          |
|    | o  |    | 並び順                   | Sort                  | integer      | System.Int64    |              |          |

### index

*NONE*



___

## Screens

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| o  | o  |    | スクリーン名             | ScreenName            | text         | System.Guid     |              |          |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |          |
|    | o  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |          |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |          |
|    | o  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり  |
|    | o  |    | X座標                    | ScreenX               | integer      | System.Int64    |              |          |
|    | o  |    | Y座標                    | ScreenY               | integer      | System.Int64    |              |          |
|    | o  |    | 横幅                     | ScreenWidth           | integer      | System.Int64    |              |          |
|    | o  |    | 高さ                     | ScreenHeight          | integer      | System.Int64    |              |          |

### index

*NONE*



___

## LauncherToolbars

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント                                     |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------------------------------------|
| o  | o  |    |                          | LauncherToolbarId     | text         | System.Guid     |              |                                              |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                                          |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                                              |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                                              |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                                              |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                                          |
|    | o  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                                              |
|    | o  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                                              |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                                              |
|    | o  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり                                      |
|    | o  |    | スクリーン名             | ScreenName            | text         | System.String   |              | ドライバアップデートとかもろもろでよく変わる |
|    | o  |    | ランチャーグループID     | LauncherGroupId       | text         | System.Guid     |              |                                              |
|    | o  |    | 表示位置                 | PositionKind          | text         | System.String   |              | 上下左右                                     |
|    | o  |    | 方向                     | Direction             | text         | System.String   |              | アイコンの並びの基点                         |
|    | o  |    | アイコンサイズ           | IconScale             | text         | System.String   |              |                                              |
|    | o  |    | フォント                 | FontId                | text         | System.Guid     |              |                                              |
|    | o  |    | 自動的に隠す時間         | AutoHideTimeout       | text         | System.TimeSpan |              |                                              |
|    | o  |    | 文字幅                   | TextWidth             | integer      | System.Int64    |              |                                              |
|    | o  |    | 表示                     | IsVisible             | boolean      | System.Boolean  |              |                                              |
|    | o  |    | 最前面                   | IsTopmost             | boolean      | System.Boolean  |              |                                              |
|    | o  |    | 自動的に隠す             | IsAutoHide            | boolean      | System.Boolean  |              |                                              |
|    | o  |    | アイコンのみ             | IsIconOnly            | boolean      | System.Boolean  |              |                                              |

### index

*NONE*



___

## Notes

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント                     |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:-----------------------------|
| o  | o  |    | ノートID                 | NoteId                | text         | System.Guid     |              |                              |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                          |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                              |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                              |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                              |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                          |
|    | o  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                              |
|    | o  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                              |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                              |
|    | o  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり                      |
|    | o  |    | タイトル                 | Title                 | text         | System.String   |              |                              |
|    | o  |    | スクリーン名             | ScreenName            | text         | System.String   |              |                              |
|    | o  |    | レイアウト種別           | LayoutKind            | text         | System.String   |              |                              |
|    | o  |    | 表示                     | IsVisible             | boolean      | System.Boolean  |              |                              |
|    | o  |    | フォントID               | FontId                | text         | System.Guid     |              |                              |
|    | o  |    | 前景色                   | ForegdoundColor       | text         | System.String   |              | #AARRGGBB                    |
|    | o  |    | 背景色                   | BackgroundColor       | text         | System.String   |              | #AARRGGBB                    |
|    | o  |    | ロック状態               | IsLocked              | boolean      | System.Boolean  |              |                              |
|    | o  |    | 最前面                   | IsTopmost             | boolean      | System.Boolean  |              |                              |
|    | o  |    | 最小化                   | IsCompact             | boolean      | System.Boolean  |              |                              |
|    | o  |    | 文字列の折り返し         | TextWrap              | boolean      | System.Boolean  |              |                              |
|    | o  |    | ノート内容種別           | ContentKind           | text         | System.String   |              | プレーン文字列 RTF, KeyValue |

### index

*NONE*



___

## NoteLayouts

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント                                                      |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:--------------------------------------------------------------|
| o  | o  |    | ノートID                 | NoteId                | text         | System.Guid     |              |                                                               |
| o  | o  |    | レイアウト種別           | LayoutKind            | text         | System.String   |              |                                                               |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                                                           |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                                                               |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                                                               |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                                                               |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                                                           |
|    | o  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                                                               |
|    | o  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                                                               |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                                                               |
|    | o  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり                                                       |
|    | o  |    | X座標                    | X                     | real         | System.Double   |              | 絶対: プライマリディスプレイ(0,0)基準, 相対: ディスプレイ中央 |
|    | o  |    | Y座標                    | Y                     | real         | System.Double   |              | 絶対: プライマリディスプレイ(0,0)基準, 相対: ディスプレイ中央 |
|    | o  |    | 横幅                     | Width                 | real         | System.Double   |              | 絶対: ウィンドウサイズ, 相対: ディスプレイ比率                |
|    | o  |    | 高さ                     | Height                | real         | System.Double   |              | 絶対: ウィンドウサイズ, 相対: ディスプレイ比率                |

### index

*NONE*



___

## NoteContents

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| o  | o  |    | ノートID                 | NoteId                | text         | System.Guid     |              |          |
| o  | o  |    | ノート内容種別           | ContentKind           | text         | System.String   |              |          |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |          |
|    | o  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |          |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |          |
|    | o  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり  |
|    | o  |    | 内容                     | Content               | text         | System.String   |              |          |

### index

*NONE*



___

## NoteFiles

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント             |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------------|
| o  | o  |    | ノートID                 | NoteId                | text         | System.Guid     |              |                      |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                      |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                      |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                      |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | o  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                      |
|    | o  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                      |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                      |
|    | o  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり              |
|    | o  |    | ファイル種別             | FileKind              | text         | System.String   |              | リンク, 埋め込み     |
|    | o  |    | ファイルパス             | FilePath              | text         | System.String   |              |                      |
|    | o  |    | 埋め込みファイルID       | NoteFileId            | text         | System.Guid     |              | 埋め込みの場合に使用 |
|    | o  |    | 並び順                   | Sort                  | integer      | System.Int64    |              |                      |

### index

*NONE*

