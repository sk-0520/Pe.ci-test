
___

## AppExecuteSetting

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
|    | o  |    | 使用許諾                 | Accepted              | boolean      | System.Boolean  |              |          |
|    | o  |    | 初回実行バージョン       | FirstVersion          | text         | System.Version  |              |          |
|    | o  |    | 初回実行日時             | FirstTimestamp        | datetime     | System.DateTime |              | UTC      |
|    | o  |    | 最終実行バージョン       | LastVersion           | text         | System.Version  |              |          |
|    | o  |    | 最終実行日時             | LastTimestamp         | datetime     | System.DateTime |              | UTC      |
|    | o  |    | 実行回数                 | ExecuteCount          | integer      | System.Int64    |              | 0始まり  |
|    | o  |    | ユーザー識別子           | UserId                | text         | System.String   |              |          |
|    | o  |    | 使用統計情報送信         | SendUsageStatistics   | boolean      | System.Boolean  |              |          |

### index

*NONE*



___

## AppGeneralSetting

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
|    | o  |    | 使用言語                 | Language              | text         | System.String   |              |          |

### index

*NONE*



___

## AppUpdateSetting

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
|    | o  |    | リリース版をチェック     | CheckReleaseVersion   |              | System.Boolean  |              |                              |
|    | o  |    | RC版をチェック           | CheckRcVersion        |              | System.Boolean  |              |                              |
|    | o  |    | 無視するバージョン       | IgnoreVersion         | text         | System.Version  |              | このバージョン以下を無視する |

### index

*NONE*



___

## AppWindowSetting

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
|    | o  |    | 有効                     | IsEnabled             |              | System.Boolean  |              |          |
|    | o  |    | 保持数                   | Count                 | integer      | System.Int64    |              |          |
|    | o  |    | 保存間隔                 | interval              | text         | System.TimeSpan |              |          |

### index

*NONE*



___

## AppCommandSetting

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
|    | o  |    | フォント                 | FontId                | text         | System.Guid     |              |          |
|    | o  |    | アイコンサイズ           | IconBox               | text         | System.String   |              |          |
|    | o  |    | 非表示待機時間           | HideWaitTime          | text         | System.TimeSpan |              |          |
|    | o  |    | タグ検索                 | FindTag               |              | System.Boolean  |              |          |
|    | o  |    | ファイル検索             | FindFile              |              | System.Boolean  |              |          |

### index

*NONE*



___

## AppNoteSetting

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント  |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:----------|
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC       |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |           |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |           |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |           |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC       |
|    | o  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |           |
|    | o  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |           |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |           |
|    | o  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり   |
|    | o  |    | フォント                 | FontId                | text         | System.Guid     |              |           |
|    | o  |    | タイトル設定             | TitleKind             | text         | System.String   |              |           |
|    | o  |    | 位置種別                 | PositionKind          | text         | System.String   |              |           |
|    | o  |    | 前景色                   | Foreground            | text         | System.String   |              | #AARRGGBB |
|    | o  |    | 背景色                   | Background            | text         | System.String   |              | #AARRGGBB |
|    | o  |    | 最前面                   | IsTopmost             |              | System.Boolean  |              |           |

### index

*NONE*



___

## AppStandardInputOutputSetting

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント  |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:----------|
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC       |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |           |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |           |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |           |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC       |
|    | o  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |           |
|    | o  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |           |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |           |
|    | o  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり   |
|    | o  |    | フォント                 | FontId                | text         | System.Guid     |              |           |
|    | o  |    | 標準出力前景色           | OutputForeground      | text         | System.String   |              | #AARRGGBB |
|    | o  |    | 標準出力背景色           | OutputBackground      | text         | System.String   |              | #AARRGGBB |
|    | o  |    | エラー前景色             | ErrorForeground       | text         | System.String   |              | #AARRGGBB |
|    | o  |    | エラー背景色             | ErrorBackground       | text         | System.String   |              | #AARRGGBB |
|    | o  |    | 最前面                   | IsTopmost             |              | System.Boolean  |              |           |

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
|    | o  |    | アイコン更新日時         | LastIconUpdatedTimestamp | datetime     | System.DateTime |              | UTC      |
|    | o  |    | コマンド入力対象         | IsEnabledCommandLauncher | boolean      | System.Boolean  |              |          |
|    | o  |    | 使用回数                 | ExecuteCount             | integer      | System.Int64    |              |          |
|    | o  |    | 最終仕様日時             | LastExecuteTimestamp     | datetime     | System.DateTime |              | UTC      |
|    | o  |    | コメント                 | Comment                  | text         | System.String   |              |          |

### index

| UK | カラム(CSV) |
|:--:|:------------|
| o  | Code        |



___

## LauncherFiles

### layout

| PK | NN | FK                           | 論理カラム名               | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:-----------------------------|:---------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| o  | o  | LauncherItems.LauncherItemId | ランチャーアイテムID       | LauncherItemId        | text         | System.Guid     |              |          |
|    | o  |                              | 作成タイムスタンプ         | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |                              | 作成ユーザー名             | CreatedAccount        | text         | System.String   |              |          |
|    | o  |                              | 作成プログラム名           | CreatedProgramName    | text         | System.String   |              |          |
|    | o  |                              | 作成プログラムバージョン   | CreatedProgramVersion | text         | System.Version  |              |          |
|    | o  |                              | 更新タイムスタンプ         | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |                              | 更新ユーザー名             | UpdatedAccount        | text         | System.String   |              |          |
|    | o  |                              | 更新プログラム名           | UpdatedProgramName    | text         | System.String   |              |          |
|    | o  |                              | 更新プログラムバージョン   | UpdatedProgramVersion | text         | System.Version  |              |          |
|    | o  |                              | 更新回数                   | UpdatedCount          | integer      | System.Int64    |              | 0始まり  |
|    | o  |                              | コマンド                   | File                  | text         | System.String   |              |          |
|    | o  |                              | コマンドオプション         | Option                | text         | System.String   |              |          |
|    | o  |                              | 作業ディレクトリ           | WorkDirectory         | text         | System.String   |              |          |
|    | o  |                              | 環境変数使用               | IsEnabledCustomEnvVar | boolean      | System.Boolean  |              |          |
|    | o  |                              | 標準入出力使用             | IsEnabledStandardIo   | boolean      | System.Boolean  |              |          |
|    | o  |                              | 標準入出力エンコーディング | StandardIoEncoding    | text         | System.String   |              |          |
|    | o  |                              | 管理者実行                 | RunAdministrator      | boolean      | System.Boolean  |              |          |

### index

*NONE*



___

## LauncherStoreApps

### layout

| PK | NN | FK                           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:-----------------------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| o  | o  | LauncherItems.LauncherItemId | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |          |
|    | o  |                              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |                              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | o  |                              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | o  |                              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    | o  |                              | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |                              | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |          |
|    | o  |                              | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |          |
|    | o  |                              | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |          |
|    | o  |                              | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり  |
|    | o  |                              | プロトコル・エイリアス   | ProtocolAlias         | text         | System.String   |              |          |
|    | o  |                              | コマンドオプション       | Option                | text         | System.String   |              |          |

### index

*NONE*



___

## LauncherAddons

### layout

| PK | NN | FK                           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:-----------------------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| o  | o  | LauncherItems.LauncherItemId | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |          |
|    | o  |                              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |                              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | o  |                              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | o  |                              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    | o  |                              | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |                              | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |          |
|    | o  |                              | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |          |
|    | o  |                              | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |          |
|    | o  |                              | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり  |
|    | o  |                              | プラグインID             | PluginId              | text         | System.Guid     |              |          |

### index

*NONE*



___

## LauncherEnvVars

### layout

| PK | NN | FK                           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:-----------------------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| o  | o  | LauncherItems.LauncherItemId | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |          |
| o  | o  |                              | 環境変数名               | EnvName               | text         | System.String   |              |          |
|    | o  |                              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |                              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | o  |                              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | o  |                              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    | o  |                              | 環境変数値               | EnvValue              | text         | System.String   |              |          |

### index

*NONE*



___

## LauncherTags

### layout

| PK | NN | FK                           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:-----------------------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| o  | o  | LauncherItems.LauncherItemId | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |          |
| o  | o  |                              | タグ名                   | TagName               | text         | System.String   |              |          |
|    | o  |                              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |                              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | o  |                              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | o  |                              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |

### index

*NONE*



___

## LauncherItemHistories

### layout

| PK | NN | FK                           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント                     |
|:--:|:--:|:-----------------------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:-----------------------------|
|    | o  |                              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                          |
|    | o  |                              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                              |
|    | o  |                              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                              |
|    | o  |                              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                              |
|    | o  | LauncherItems.LauncherItemId | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |                              |
|    | o  |                              | 履歴種別                 | Kind                  | text         | System.String   |              | オプション, 作業ディレクトリ |
|    | o  |                              | 値                       | Value                 | text         | System.String   |              |                              |
|    | o  |                              | 最終使用日時             | LastExecuteTimestamp  | datetime     | System.DateTime |              |                              |

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
|    | o  |    | ボールド                 | IsBold                | boolean      | System.Boolean  |              |          |
|    | o  |    | イタリック               | IsItalic              | boolean      | System.Boolean  |              |          |
|    | o  |    | 下線                     | IsUnderline           | boolean      | System.Boolean  |              |          |
|    | o  |    | 取り消し線               | IsStrikeThrough       | boolean      | System.Boolean  |              |          |

### index

*NONE*



___

## LauncherGroups

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント  |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:----------|
| o  | o  |    | ランチャーグループID     | LauncherGroupId       | text         | System.Guid     |              |           |
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
|    | o  |    | 並び順                   | Sequence              | integer      | System.Int64    |              |           |

### index

*NONE*



___

## LauncherGroupItems

### layout

| PK | NN | FK                             | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:-------------------------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
|    | o  |                                | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |                                | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | o  |                                | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | o  |                                | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    | o  | LauncherGroups.LauncherGroupId | ランチャーグループID     | LauncherGroupId       | text         | System.Guid     |              |          |
|    | o  | LauncherItems.LauncherItemId   | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |          |
|    | o  |                                | 並び順                   | Sequence              | integer      | System.Int64    |              |          |

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
| o  | o  |    | ランチャーツールバーID   | LauncherToolbarId     | text         | System.Guid     |              |                                              |
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
|    | o  |    | アイコンサイズ           | IconBox               | text         | System.String   |              |                                              |
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

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント           |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:-------------------|
| o  | o  |    | ノートID                 | NoteId                | text         | System.Guid     |              |                    |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                    |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                    |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                    |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                |
|    | o  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                    |
|    | o  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                    |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                    |
|    | o  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり            |
|    | o  |    | タイトル                 | Title                 | text         | System.String   |              |                    |
|    | o  |    | スクリーン名             | ScreenName            | text         | System.String   |              |                    |
|    | o  |    | レイアウト種別           | LayoutKind            | text         | System.String   |              |                    |
|    | o  |    | 表示                     | IsVisible             | boolean      | System.Boolean  |              |                    |
|    | o  |    | フォントID               | FontId                | text         | System.Guid     |              |                    |
|    | o  |    | 前景色                   | ForegroundColor       | text         | System.String   |              | #AARRGGBB          |
|    | o  |    | 背景色                   | BackgroundColor       | text         | System.String   |              | #AARRGGBB          |
|    | o  |    | ロック状態               | IsLocked              | boolean      | System.Boolean  |              |                    |
|    | o  |    | 最前面                   | IsTopmost             | boolean      | System.Boolean  |              |                    |
|    | o  |    | 最小化                   | IsCompact             | boolean      | System.Boolean  |              |                    |
|    | o  |    | 文字列の折り返し         | TextWrap              | boolean      | System.Boolean  |              |                    |
|    | o  |    | ノート内容種別           | ContentKind           | text         | System.String   |              | プレーン文字列 RTF |

### index

*NONE*



___

## NoteLayouts

### layout

| PK | NN | FK           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント                                       |
|:--:|:--:|:-------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:-----------------------------------------------|
| o  | o  | Notes.NoteId | ノートID                 | NoteId                | text         | System.Guid     |              |                                                |
| o  | o  |              | レイアウト種別           | LayoutKind            | text         | System.String   |              |                                                |
|    | o  |              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                                            |
|    | o  |              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                                                |
|    | o  |              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                                                |
|    | o  |              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                                                |
|    | o  |              | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                                            |
|    | o  |              | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                                                |
|    | o  |              | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                                                |
|    | o  |              | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                                                |
|    | o  |              | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり                                        |
|    | o  |              | X座標                    | X                     | real         | System.Double   |              | 絶対: ディスプレイ基準, 相対: ディスプレイ中央 |
|    | o  |              | Y座標                    | Y                     | real         | System.Double   |              | 絶対: ディスプレイ基準, 相対: ディスプレイ中央 |
|    | o  |              | 横幅                     | Width                 | real         | System.Double   |              | 絶対: ウィンドウサイズ, 相対: ディスプレイ比率 |
|    | o  |              | 高さ                     | Height                | real         | System.Double   |              | 絶対: ウィンドウサイズ, 相対: ディスプレイ比率 |

### index

*NONE*



___

## NoteContents

### layout

| PK | NN | FK           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント           |
|:--:|:--:|:-------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:-------------------|
| o  | o  | Notes.NoteId | ノートID                 | NoteId                | text         | System.Guid     |              |                    |
| o  | o  |              | ノート内容種別           | ContentKind           | text         | System.String   |              | プレーン文字列 RTF |
|    | o  |              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                |
|    | o  |              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                    |
|    | o  |              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                    |
|    | o  |              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                    |
|    | o  |              | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                |
|    | o  |              | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                    |
|    | o  |              | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                    |
|    | o  |              | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                    |
|    | o  |              | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり            |
|    | o  |              | リンク形式か             | IsLink                | boolean      | System.Boolean  |              |                    |
|    | o  |              | 内容                     | Content               | text         | System.String   |              |                    |
|    | o  |              | リンク先                 | Address               | text         | System.String   |              |                    |
|    | o  |              | エンコーディング         | Encoding              | text         | System.String   |              |                    |
|    | o  |              | 変更検知後待機時間       | DelayTime             | text         | System.TimeSpan |              |                    |
|    | o  |              | 変更検知バッファサイズ   | BufferSize            | integer      | System.Int64    |              |                    |
|    | o  |              | 変更検知ミス更新時間     | RefreshTime           | text         | System.TimeSpan |              |                    |
|    | o  |              | 取りこぼしを考慮         | IsEnabledRefresh      | boolean      | System.Boolean  |              |                    |

### index

*NONE*



___

## NoteFiles

### layout

| PK | NN | FK           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント             |
|:--:|:--:|:-------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------------|
| o  | o  | Notes.NoteId | ノートID                 | NoteId                | text         | System.Guid     |              |                      |
|    | o  |              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | o  |              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                      |
|    | o  |              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                      |
|    | o  |              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                      |
|    | o  |              | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | o  |              | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                      |
|    | o  |              | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                      |
|    | o  |              | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                      |
|    | o  |              | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり              |
|    | o  |              | ファイル種別             | FileKind              | text         | System.String   |              | リンク, 埋め込み     |
|    | o  |              | ファイルパス             | FilePath              | text         | System.String   |              |                      |
|    | o  |              | 埋め込みファイルID       | NoteFileId            | text         | System.Guid     |              | 埋め込みの場合に使用 |
|    | o  |              | 並び順                   | Sequence              | integer      | System.Int64    |              |                      |

### index

*NONE*



___

## KeyActions

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント             |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------------|
| o  | o  |    | キーアクションID         | KeyActionId           | text         | System.Guid     |              |                      |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                      |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                      |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                      |
|    | o  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | o  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                      |
|    | o  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                      |
|    | o  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                      |
|    | o  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり              |
|    | o  |    | アクション種別           | KeyActionKind         | text         | System.String   |              | ランチャー, コマンド |
|    | o  |    | アクション内容           | KeyActionContent      | text         | System.String   |              | アクション種別で変動 |
|    | o  |    | コメント                 | Comment               | text         | System.String   |              |                      |

### index

*NONE*



___

## KeyOptions

### layout

| PK | NN | FK                     | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント             |
|:--:|:--:|:-----------------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------------|
| o  | o  | KeyActions.KeyActionId | キーアクションID         | KeyActionId           | text         | System.Guid     |              |                      |
| o  | o  |                        | オプション名             | KeyOptionName         | text         | System.String   |              | アクション種別で変動 |
|    | o  |                        | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | o  |                        | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                      |
|    | o  |                        | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                      |
|    | o  |                        | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                      |
|    | o  |                        | オプション内容           | KeyOptionValue        | text         | System.String   |              | オプション名で変動   |

### index

*NONE*



___

## KeyMappings

### layout

| PK | NN | FK                     | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント       |
|:--:|:--:|:-----------------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------|
| o  | o  | KeyActions.KeyActionId | キーアクションID         | KeyActionId           | text         | System.Guid     |              |                |
| o  | o  |                        | 並び順                   | Sequence              | integer      | System.Int64    |              |                |
|    | o  |                        | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC            |
|    | o  |                        | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                |
|    | o  |                        | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                |
|    | o  |                        | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                |
|    | o  |                        | キー                     | Key                   | text         | System.String   |              | キー           |
|    | o  |                        | Shiftキー                | Shift                 | text         | System.String   |              | 何れか, 左, 右 |
|    | o  |                        | Ctrlキー                 | Control               | text         | System.String   |              | 何れか, 左, 右 |
|    | o  |                        | Altキー                  | Alt                   | text         | System.String   |              | 何れか, 左, 右 |
|    | o  |                        | Winキー                  | Super                 | text         | System.String   |              | 何れか, 左, 右 |

### index

*NONE*

