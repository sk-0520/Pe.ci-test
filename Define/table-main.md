# AppExecuteSetting

## layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント             |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------------|
| x  | x  |    | 世代                     | Generation            | integer      | System.Int64    |              | 最大のものを使用する |
|    | x  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                      |
|    | x  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                      |
|    | x  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                      |
|    | x  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                      |
|    | x  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり              |
|    | x  |    | 使用許諾                 | Accepted              | boolean      | System.Boolean  |              |                      |
|    | x  |    | 初回実行バージョン       | FirstVersion          | text         | System.Version  |              |                      |
|    | x  |    | 初回実行日時             | FirstTimestamp        | datetime     | System.DateTime |              | UTC                  |
|    | x  |    | 最終実行バージョン       | LastVersion           | text         | System.Version  |              |                      |
|    | x  |    | 最終実行日時             | LastTimestamp         | datetime     | System.DateTime |              | UTC                  |
|    | x  |    | 実行回数                 | ExecuteCount          | integer      | System.Int64    |              | 0始まり              |
|    | x  |    | ユーザー識別子           | UserId                | text         | System.String   |              |                      |
|    | x  |    | 使用統計情報送信         | IsEnabledTelemetry    | boolean      | System.Boolean  |              |                      |

## index

*NONE*



___

# AppGeneralSetting

## layout

| PK | NN | FK | 論理カラム名                   | 物理カラム名            | 論理データ型 | マッピング型    | チェック制約 | コメント             |
|:--:|:--:|:---|:-------------------------------|:------------------------|:-------------|:----------------|:-------------|:---------------------|
| x  | x  |    | 世代                           | Generation              | integer      | System.Int64    |              | 最大のものを使用する |
|    | x  |    | 作成タイムスタンプ             | CreatedTimestamp        | datetime     | System.DateTime |              | UTC                  |
|    | x  |    | 作成ユーザー名                 | CreatedAccount          | text         | System.String   |              |                      |
|    | x  |    | 作成プログラム名               | CreatedProgramName      | text         | System.String   |              |                      |
|    | x  |    | 作成プログラムバージョン       | CreatedProgramVersion   | text         | System.Version  |              |                      |
|    | x  |    | 更新タイムスタンプ             | UpdatedTimestamp        | datetime     | System.DateTime |              | UTC                  |
|    | x  |    | 更新ユーザー名                 | UpdatedAccount          | text         | System.String   |              |                      |
|    | x  |    | 更新プログラム名               | UpdatedProgramName      | text         | System.String   |              |                      |
|    | x  |    | 更新プログラムバージョン       | UpdatedProgramVersion   | text         | System.Version  |              |                      |
|    | x  |    | 更新回数                       | UpdatedCount            | integer      | System.Int64    |              | 0始まり              |
|    | x  |    | 使用言語                       | Language                | text         | System.String   |              |                      |
|    | x  |    | 明示的バックアップディレクトリ | UserBackupDirectoryPath | text         | System.String   |              |                      |
|    | x  |    | テーマプラグインID             | ThemePluginId           | text         | System.String   |              | ※一応外部参照しない |

## index

*NONE*



___

# AppUpdateSetting

## layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント                     |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:-----------------------------|
| x  | x  |    | 世代                     | Generation            | integer      | System.Int64    |              | 最大のものを使用する         |
|    | x  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                          |
|    | x  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                              |
|    | x  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                              |
|    | x  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                              |
|    | x  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                          |
|    | x  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                              |
|    | x  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                              |
|    | x  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                              |
|    | x  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり                      |
|    | x  |    | アップデート方法         | UpdateKind            | text         | System.String   |              |                              |
|    | x  |    | 無視するバージョン       | IgnoreVersion         | text         | System.Version  |              | このバージョン以下を無視する |

## index

*NONE*



___

# AppCommandSetting

## layout

| PK | NN | FK           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント             |
|:--:|:--:|:-------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------------|
| x  | x  |              | 世代                     | Generation            | integer      | System.Int64    |              | 最大のものを使用する |
|    | x  |              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                      |
|    | x  |              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                      |
|    | x  |              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |              | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |              | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                      |
|    | x  |              | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                      |
|    | x  |              | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |              | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり              |
|    | x  | Fonts.FontId | フォント                 | FontId                | text         | System.Guid     |              |                      |
|    | x  |              | アイコンサイズ           | IconBox               | text         | System.String   |              |                      |
|    | x  |              | 横幅                     | Width                 | real         | System.Double   |              |                      |
|    | x  |              | 非表示待機時間           | HideWaitTime          | text         | System.TimeSpan |              |                      |

## index

*NONE*



___

# AppNoteSetting

## layout

| PK | NN | FK           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント             |
|:--:|:--:|:-------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------------|
| x  | x  |              | 世代                     | Generation            | integer      | System.Int64    |              | 最大のものを使用する |
|    | x  |              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                      |
|    | x  |              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                      |
|    | x  |              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |              | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |              | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                      |
|    | x  |              | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                      |
|    | x  |              | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |              | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり              |
|    | x  | Fonts.FontId | フォント                 | FontId                | text         | System.Guid     |              |                      |
|    | x  |              | タイトル設定             | TitleKind             | text         | System.String   |              |                      |
|    | x  |              | 位置種別                 | LayoutKind            | text         | System.String   |              |                      |
|    | x  |              | 前景色                   | ForegroundColor       | text         | System.String   |              | #AARRGGBB            |
|    | x  |              | 背景色                   | BackgroundColor       | text         | System.String   |              | #AARRGGBB            |
|    | x  |              | 最前面                   | IsTopmost             | boolean      | System.Boolean  |              |                      |
|    | x  |              | タイトル位置             | CaptionPosition       | text         | System.String   |              |                      |

## index

*NONE*



___

# AppNoteHiddenSetting

## layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント             |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------------|
| x  | x  |    | 世代                     | Generation            | integer      | System.Int64    |              | 最大のものを使用する |
|    | x  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                      |
|    | x  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                      |
|    | x  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                      |
|    | x  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                      |
|    | x  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり              |
|    | x  |    | 隠し方                   | HiddenMode            | text         | System.String   |              |                      |
|    | x  |    | 待機時間                 | WaitTime              | text         | System.TimeSpan |              |                      |

## index

| UK | 名前 | カラム(CSV)            |
|:--:|:-----|:-----------------------|
| x  | idx_AppNoteHiddenSetting_1 | Generation, HiddenMode |



___

# AppStandardInputOutputSetting

## layout

| PK | NN | FK           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント             |
|:--:|:--:|:-------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------------|
| x  | x  |              | 世代                     | Generation            | integer      | System.Int64    |              | 最大のものを使用する |
|    | x  |              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                      |
|    | x  |              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                      |
|    | x  |              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |              | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |              | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                      |
|    | x  |              | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                      |
|    | x  |              | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |              | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり              |
|    | x  | Fonts.FontId | フォント                 | FontId                | text         | System.Guid     |              |                      |
|    | x  |              | 標準出力前景色           | OutputForeground      | text         | System.String   |              | #AARRGGBB            |
|    | x  |              | 標準出力背景色           | OutputBackground      | text         | System.String   |              | #AARRGGBB            |
|    | x  |              | エラー前景色             | ErrorForeground       | text         | System.String   |              | #AARRGGBB            |
|    | x  |              | エラー背景色             | ErrorBackground       | text         | System.String   |              | #AARRGGBB            |
|    | x  |              | 最前面                   | IsTopmost             | boolean      | System.Boolean  |              |                      |

## index

*NONE*



___

# AppLauncherToolbarSetting

## layout

| PK | NN | FK           | 論理カラム名              | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント             |
|:--:|:--:|:-------------|:--------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------------|
| x  | x  |              | 世代                      | Generation            | integer      | System.Int64    |              | 最大のものを使用する |
|    | x  |              | 作成タイムスタンプ        | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |              | 作成ユーザー名            | CreatedAccount        | text         | System.String   |              |                      |
|    | x  |              | 作成プログラム名          | CreatedProgramName    | text         | System.String   |              |                      |
|    | x  |              | 作成プログラムバージョン  | CreatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |              | 更新タイムスタンプ        | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |              | 更新ユーザー名            | UpdatedAccount        | text         | System.String   |              |                      |
|    | x  |              | 更新プログラム名          | UpdatedProgramName    | text         | System.String   |              |                      |
|    | x  |              | 更新プログラムバージョン  | UpdatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |              | 更新回数                  | UpdatedCount          | integer      | System.Int64    |              | 0始まり              |
|    | x  |              | 表示位置                  | PositionKind          | text         | System.String   |              | 上下左右             |
|    | x  |              | 方向                      | Direction             | text         | System.String   |              | アイコンの並びの基点 |
|    | x  |              | アイコンサイズ            | IconBox               | text         | System.String   |              |                      |
|    | x  | Fonts.FontId | フォント                  | FontId                | text         | System.Guid     |              |                      |
|    | x  |              | 表示するまでの抑制時間    | DisplayDelayTime      | text         | System.TimeSpan |              |                      |
|    | x  |              | 自動的に隠す時間          | AutoHideTime          | text         | System.TimeSpan |              |                      |
|    | x  |              | 文字幅                    | TextWidth             | integer      | System.Int64    |              |                      |
|    | x  |              | 表示                      | IsVisible             | boolean      | System.Boolean  |              |                      |
|    | x  |              | 最前面                    | IsTopmost             | boolean      | System.Boolean  |              |                      |
|    | x  |              | 自動的に隠す              | IsAutoHide            | boolean      | System.Boolean  |              |                      |
|    | x  |              | アイコンのみ              | IsIconOnly            | boolean      | System.Boolean  |              |                      |
|    | x  |              | ツールバーへのD&D処理     | ContentDropMode       | text         | System.String   |              |                      |
|    | x  |              | グループメニュー表示位置  | GroupMenuPosition     | text         | System.String   |              |                      |
|    | x  |              | ショートカットD&D時の挙動 | ShortcutDropMode      | text         | System.String   |              |                      |

## index

*NONE*



___

# AppPlatformSetting

## layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント             |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------------|
| x  | x  |    | 世代                     | Generation            | integer      | System.Int64    |              | 最大のものを使用する |
|    | x  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                      |
|    | x  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                      |
|    | x  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                      |
|    | x  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                      |
|    | x  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり              |
|    | x  |    | アイドル抑制             | SuppressSystemIdle    | boolean      | System.Boolean  |              |                      |
|    | x  |    | Explorer補正             | SupportExplorer       | boolean      | System.Boolean  |              |                      |

## index

*NONE*



___

# AppProxySetting

## layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント             |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------------|
| x  | x  |    | 世代                     | Generation            | integer      | System.Int64    |              | 最大のものを使用する |
|    | x  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                      |
|    | x  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                      |
|    | x  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                      |
|    | x  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                      |
|    | x  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり              |
|    | x  |    | 有効状態                 | ProxyIsEnabled        | boolean      | System.Boolean  |              |                      |
|    | x  |    | プロキシURL              | ProxyUrl              | text         | System.String   |              |                      |
|    | x  |    | 認証有効状態             | CredentialIsEnabled   | boolean      | System.Boolean  |              |                      |
|    | x  |    | 認証ユーザー             | CredentialUser        | text         | System.String   |              |                      |
|    | x  |    | 認証パスワード           | CredentialPassword    | blob         | System.Byte[]   |              |                      |

## index

*NONE*



___

# AppNotifyLogSetting

## layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント             |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------------|
| x  | x  |    | 世代                     | Generation            | integer      | System.Int64    |              | 最大のものを使用する |
|    | x  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                      |
|    | x  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                      |
|    | x  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                      |
|    | x  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                      |
|    | x  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり              |
|    | x  |    | 表示                     | IsVisible             | boolean      | System.Boolean  |              |                      |
|    | x  |    | 表示位置                 | Position              | text         | System.String   |              |                      |

## index

*NONE*



___

# LauncherItems

## layout

| PK | NN | FK | 論理カラム名             | 物理カラム名             | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:---|:-------------------------|:-------------------------|:-------------|:----------------|:-------------|:---------|
| x  | x  |    | ランチャーアイテムID     | LauncherItemId           | text         | System.Guid     |              |          |
|    | x  |    | 作成タイムスタンプ       | CreatedTimestamp         | datetime     | System.DateTime |              | UTC      |
|    | x  |    | 作成ユーザー名           | CreatedAccount           | text         | System.String   |              |          |
|    | x  |    | 作成プログラム名         | CreatedProgramName       | text         | System.String   |              |          |
|    | x  |    | 作成プログラムバージョン | CreatedProgramVersion    | text         | System.Version  |              |          |
|    | x  |    | 更新タイムスタンプ       | UpdatedTimestamp         | datetime     | System.DateTime |              | UTC      |
|    | x  |    | 更新ユーザー名           | UpdatedAccount           | text         | System.String   |              |          |
|    | x  |    | 更新プログラム名         | UpdatedProgramName       | text         | System.String   |              |          |
|    | x  |    | 更新プログラムバージョン | UpdatedProgramVersion    | text         | System.Version  |              |          |
|    | x  |    | 更新回数                 | UpdatedCount             | integer      | System.Int64    |              | 0始まり  |
|    | x  |    | 名称                     | Name                     | text         | System.String   |              |          |
|    | x  |    | ランチャー種別           | Kind                     | text         | System.String   |              |          |
|    | x  |    | アイコンパス             | IconPath                 | text         | System.String   |              |          |
|    | x  |    | アイコンインデックス     | IconIndex                | integer      | System.Int64    |              |          |
|    | x  |    | コマンド入力対象         | IsEnabledCommandLauncher | boolean      | System.Boolean  |              |          |
|    | x  |    | 使用回数                 | ExecuteCount             | integer      | System.Int64    |              |          |
|    | x  |    | 最終仕様日時             | LastExecuteTimestamp     | datetime     | System.DateTime |              | UTC      |
|    | x  |    | コメント                 | Comment                  | text         | System.String   |              |          |

## index

| UK | 名前 | カラム(CSV) |
|:--:|:-----|:------------|
| x  | idx_LauncherItems_1 | Code        |



___

# LauncherFiles

## layout

| PK | NN | FK                           | 論理カラム名               | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:-----------------------------|:---------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| x  | x  | LauncherItems.LauncherItemId | ランチャーアイテムID       | LauncherItemId        | text         | System.Guid     |              |          |
|    | x  |                              | 作成タイムスタンプ         | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | x  |                              | 作成ユーザー名             | CreatedAccount        | text         | System.String   |              |          |
|    | x  |                              | 作成プログラム名           | CreatedProgramName    | text         | System.String   |              |          |
|    | x  |                              | 作成プログラムバージョン   | CreatedProgramVersion | text         | System.Version  |              |          |
|    | x  |                              | 更新タイムスタンプ         | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | x  |                              | 更新ユーザー名             | UpdatedAccount        | text         | System.String   |              |          |
|    | x  |                              | 更新プログラム名           | UpdatedProgramName    | text         | System.String   |              |          |
|    | x  |                              | 更新プログラムバージョン   | UpdatedProgramVersion | text         | System.Version  |              |          |
|    | x  |                              | 更新回数                   | UpdatedCount          | integer      | System.Int64    |              | 0始まり  |
|    | x  |                              | コマンド                   | File                  | text         | System.String   |              |          |
|    | x  |                              | コマンドオプション         | Option                | text         | System.String   |              |          |
|    | x  |                              | 作業ディレクトリ           | WorkDirectory         | text         | System.String   |              |          |
|    | x  |                              | 表示方法                   | ShowMode              | text         | System.String   |              |          |
|    | x  |                              | 環境変数使用               | IsEnabledCustomEnvVar | boolean      | System.Boolean  |              |          |
|    | x  |                              | 標準入出力使用             | IsEnabledStandardIo   | boolean      | System.Boolean  |              |          |
|    | x  |                              | 標準入出力エンコーディング | StandardIoEncoding    | text         | System.String   |              |          |
|    | x  |                              | 管理者実行                 | RunAdministrator      | boolean      | System.Boolean  |              |          |

## index

*NONE*



___

# LauncherRedoItems

## layout

| PK | NN | FK                           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:-----------------------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| x  | x  | LauncherItems.LauncherItemId | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |          |
|    | x  |                              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | x  |                              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | x  |                              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | x  |                              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    | x  |                              | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | x  |                              | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |          |
|    | x  |                              | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |          |
|    | x  |                              | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |          |
|    | x  |                              | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり  |
|    | x  |                              | 再実施待機方法           | RedoMode              | text         | System.Version  |              |          |
|    | x  |                              | 待機時間                 | WaitTime              | text         | System.TimeSpan |              |          |
|    | x  |                              | 再試行回数               | RetryCount            | integer      | System.Int64    |              |          |

## index

*NONE*



___

# LauncherRedoSuccessExitCodes

## layout

| PK | NN | FK                           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:-----------------------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| x  | x  | LauncherItems.LauncherItemId | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |          |
| x  | x  |                              | 正常終了コード           | SuccessExitCode       | integer      | System.Int64    |              |          |
|    | x  |                              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | x  |                              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | x  |                              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | x  |                              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |

## index

*NONE*



___

# LauncherStoreApps

## layout

| PK | NN | FK                           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:-----------------------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| x  | x  | LauncherItems.LauncherItemId | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |          |
|    | x  |                              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | x  |                              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | x  |                              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | x  |                              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    | x  |                              | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | x  |                              | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |          |
|    | x  |                              | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |          |
|    | x  |                              | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |          |
|    | x  |                              | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり  |
|    | x  |                              | プロトコル・エイリアス   | ProtocolAlias         | text         | System.String   |              |          |
|    | x  |                              | コマンドオプション       | Option                | text         | System.String   |              |          |

## index

*NONE*



___

# LauncherAddons

## layout

| PK | NN | FK                           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:-----------------------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| x  | x  | LauncherItems.LauncherItemId | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |          |
|    | x  |                              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | x  |                              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | x  |                              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | x  |                              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    | x  |                              | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | x  |                              | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |          |
|    | x  |                              | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |          |
|    | x  |                              | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |          |
|    | x  |                              | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり  |
|    | x  |                              | プラグインID             | PluginId              | text         | System.Guid     |              |          |

## index

*NONE*



___

# LauncherSeparators

## layout

| PK | NN | FK                           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント    |
|:--:|:--:|:-----------------------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:------------|
| x  | x  | LauncherItems.LauncherItemId | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |             |
|    | x  |                              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC         |
|    | x  |                              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |             |
|    | x  |                              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |             |
|    | x  |                              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |             |
|    | x  |                              | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC         |
|    | x  |                              | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |             |
|    | x  |                              | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |             |
|    | x  |                              | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |             |
|    | x  |                              | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり     |
|    | x  |                              | 区切り種別               | Kind                  | text         | System.String   |              |             |
|    | x  |                              | 区切り幅                 | Width                 | integer      |                 |              | Kind に依存 |

## index

*NONE*



___

# LauncherEnvVars

## layout

| PK | NN | FK                           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:-----------------------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| x  | x  | LauncherItems.LauncherItemId | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |          |
| x  | x  |                              | 環境変数名               | EnvName               | text         | System.String   |              |          |
|    | x  |                              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | x  |                              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | x  |                              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | x  |                              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    | x  |                              | 環境変数値               | EnvValue              | text         | System.String   |              |          |

## index

*NONE*



___

# LauncherTags

## layout

| PK | NN | FK                           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:-----------------------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| x  | x  | LauncherItems.LauncherItemId | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |          |
| x  | x  |                              | タグ名                   | TagName               | text         | System.String   |              |          |
|    | x  |                              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | x  |                              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | x  |                              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | x  |                              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |

## index

*NONE*



___

# LauncherItemHistories

## layout

| PK | NN | FK                           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント                     |
|:--:|:--:|:-----------------------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:-----------------------------|
|    | x  |                              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                          |
|    | x  |                              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                              |
|    | x  |                              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                              |
|    | x  |                              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                              |
|    | x  | LauncherItems.LauncherItemId | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |                              |
|    | x  |                              | 履歴種別                 | Kind                  | text         | System.String   |              | オプション, 作業ディレクトリ |
|    | x  |                              | 値                       | Value                 | text         | System.String   |              |                              |
|    | x  |                              | 最終使用日時             | LastExecuteTimestamp  | datetime     | System.DateTime |              |                              |

## index

*NONE*



___

# Fonts

## layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| x  | x  |    | フォントID               | FontId                | text         | System.Guid     |              |          |
|    | x  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | x  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | x  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | x  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    | x  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | x  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |          |
|    | x  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |          |
|    | x  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |          |
|    | x  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり  |
|    | x  |    | フォントファミリー       | FamilyName            | text         | System.String   |              |          |
|    | x  |    | 高さ                     | Height                | real         | System.Double   |              |          |
|    | x  |    | ボールド                 | IsBold                | boolean      | System.Boolean  |              |          |
|    | x  |    | イタリック               | IsItalic              | boolean      | System.Boolean  |              |          |
|    | x  |    | 下線                     | IsUnderline           | boolean      | System.Boolean  |              |          |
|    | x  |    | 取り消し線               | IsStrikeThrough       | boolean      | System.Boolean  |              |          |

## index

*NONE*



___

# LauncherGroups

## layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント  |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:----------|
| x  | x  |    | ランチャーグループID     | LauncherGroupId       | text         | System.Guid     |              |           |
|    | x  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC       |
|    | x  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |           |
|    | x  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |           |
|    | x  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |           |
|    | x  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC       |
|    | x  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |           |
|    | x  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |           |
|    | x  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |           |
|    | x  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり   |
|    | x  |    | グループ名               | Name                  | text         | System.String   |              |           |
|    | x  |    | グループ種別             | Kind                  | text         | System.String   |              |           |
|    | x  |    | イメージ                 | ImageName             | text         | System.String   |              |           |
|    | x  |    | 色                       | ImageColor            | text         | System.String   |              | #AARRGGBB |
|    | x  |    | 並び順                   | Sequence              | integer      | System.Int64    |              |           |

## index

*NONE*



___

# LauncherGroupItems

## layout

| PK | NN | FK                             | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:-------------------------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
|    | x  |                                | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | x  |                                | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | x  |                                | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | x  |                                | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    | x  | LauncherGroups.LauncherGroupId | ランチャーグループID     | LauncherGroupId       | text         | System.Guid     |              |          |
|    | x  | LauncherItems.LauncherItemId   | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |          |
|    | x  |                                | 並び順                   | Sequence              | integer      | System.Int64    |              |          |

## index

*NONE*



___

# Screens

## layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| x  | x  |    | スクリーン名             | ScreenName            | text         | System.Guid     |              |          |
|    | x  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | x  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | x  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | x  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    | x  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | x  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |          |
|    | x  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |          |
|    | x  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |          |
|    | x  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり  |
|    | x  |    | X座標                    | ScreenX               | integer      | System.Int64    |              |          |
|    | x  |    | Y座標                    | ScreenY               | integer      | System.Int64    |              |          |
|    | x  |    | 横幅                     | ScreenWidth           | integer      | System.Int64    |              |          |
|    | x  |    | 高さ                     | ScreenHeight          | integer      | System.Int64    |              |          |

## index

*NONE*



___

# LauncherToolbars

## layout

| PK | NN | FK           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント                                     |
|:--:|:--:|:-------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------------------------------------|
| x  | x  |              | ランチャーツールバーID   | LauncherToolbarId     | text         | System.Guid     |              |                                              |
|    | x  |              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                                          |
|    | x  |              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                                              |
|    | x  |              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                                              |
|    | x  |              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                                              |
|    | x  |              | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                                          |
|    | x  |              | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                                              |
|    | x  |              | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                                              |
|    | x  |              | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                                              |
|    | x  |              | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり                                      |
|    | x  |              | スクリーン名             | ScreenName            | text         | System.String   |              | ドライバアップデートとかもろもろでよく変わる |
|    | x  |              | ランチャーグループID     | LauncherGroupId       | text         | System.Guid     |              |                                              |
|    | x  |              | 表示位置                 | PositionKind          | text         | System.String   |              | 上下左右                                     |
|    | x  |              | 方向                     | Direction             | text         | System.String   |              | アイコンの並びの基点                         |
|    | x  |              | アイコンサイズ           | IconBox               | text         | System.String   |              |                                              |
|    | x  | Fonts.FontId | フォント                 | FontId                | text         | System.Guid     |              |                                              |
|    | x  |              | 表示するまでの抑制時間   | DisplayDelayTime      | text         | System.TimeSpan |              |                                              |
|    | x  |              | 自動的に隠す時間         | AutoHideTime          | text         | System.TimeSpan |              |                                              |
|    | x  |              | 文字幅                   | TextWidth             | integer      | System.Int64    |              |                                              |
|    | x  |              | 表示                     | IsVisible             | boolean      | System.Boolean  |              |                                              |
|    | x  |              | 最前面                   | IsTopmost             | boolean      | System.Boolean  |              |                                              |
|    | x  |              | 自動的に隠す             | IsAutoHide            | boolean      | System.Boolean  |              |                                              |
|    | x  |              | アイコンのみ             | IsIconOnly            | boolean      | System.Boolean  |              |                                              |

## index

*NONE*



___

# Notes

## layout

| PK | NN | FK           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント           |
|:--:|:--:|:-------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:-------------------|
| x  | x  |              | ノートID                 | NoteId                | text         | System.Guid     |              |                    |
|    | x  |              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                |
|    | x  |              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                    |
|    | x  |              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                    |
|    | x  |              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                    |
|    | x  |              | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                |
|    | x  |              | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                    |
|    | x  |              | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                    |
|    | x  |              | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                    |
|    | x  |              | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり            |
|    | x  |              | タイトル                 | Title                 | text         | System.String   |              |                    |
|    | x  |              | スクリーン名             | ScreenName            | text         | System.String   |              |                    |
|    | x  |              | レイアウト種別           | LayoutKind            | text         | System.String   |              |                    |
|    | x  |              | 表示                     | IsVisible             | boolean      | System.Boolean  |              |                    |
|    | x  | Fonts.FontId | フォントID               | FontId                | text         | System.Guid     |              |                    |
|    | x  |              | 前景色                   | ForegroundColor       | text         | System.String   |              | #AARRGGBB          |
|    | x  |              | 背景色                   | BackgroundColor       | text         | System.String   |              | #AARRGGBB          |
|    | x  |              | ロック状態               | IsLocked              | boolean      | System.Boolean  |              |                    |
|    | x  |              | 最前面                   | IsTopmost             | boolean      | System.Boolean  |              |                    |
|    | x  |              | 最小化                   | IsCompact             | boolean      | System.Boolean  |              |                    |
|    | x  |              | 文字列の折り返し         | TextWrap              | boolean      | System.Boolean  |              |                    |
|    | x  |              | ノート内容種別           | ContentKind           | text         | System.String   |              | プレーン文字列 RTF |
|    | x  |              | 隠し方                   | HiddenMode            | text         | System.String   |              |                    |
|    | x  |              | タイトル位置             | CaptionPosition       | text         | System.String   |              |                    |

## index

*NONE*



___

# NoteLayouts

## layout

| PK | NN | FK           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント                                       |
|:--:|:--:|:-------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:-----------------------------------------------|
| x  | x  | Notes.NoteId | ノートID                 | NoteId                | text         | System.Guid     |              |                                                |
| x  | x  |              | レイアウト種別           | LayoutKind            | text         | System.String   |              |                                                |
|    | x  |              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                                            |
|    | x  |              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                                                |
|    | x  |              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                                                |
|    | x  |              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                                                |
|    | x  |              | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                                            |
|    | x  |              | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                                                |
|    | x  |              | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                                                |
|    | x  |              | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                                                |
|    | x  |              | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり                                        |
|    | x  |              | X座標                    | X                     | real         | System.Double   |              | 絶対: ディスプレイ基準, 相対: ディスプレイ中央 |
|    | x  |              | Y座標                    | Y                     | real         | System.Double   |              | 絶対: ディスプレイ基準, 相対: ディスプレイ中央 |
|    | x  |              | 横幅                     | Width                 | real         | System.Double   |              | 絶対: ウィンドウサイズ, 相対: ディスプレイ比率 |
|    | x  |              | 高さ                     | Height                | real         | System.Double   |              | 絶対: ウィンドウサイズ, 相対: ディスプレイ比率 |

## index

*NONE*



___

# NoteContents

## layout

| PK | NN | FK           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント           |
|:--:|:--:|:-------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:-------------------|
| x  | x  | Notes.NoteId | ノートID                 | NoteId                | text         | System.Guid     |              |                    |
| x  | x  |              | ノート内容種別           | ContentKind           | text         | System.String   |              | プレーン文字列 RTF |
|    | x  |              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                |
|    | x  |              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                    |
|    | x  |              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                    |
|    | x  |              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                    |
|    | x  |              | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                |
|    | x  |              | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                    |
|    | x  |              | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                    |
|    | x  |              | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                    |
|    | x  |              | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり            |
|    | x  |              | リンク形式か             | IsLink                | boolean      | System.Boolean  |              |                    |
|    | x  |              | 内容                     | Content               | text         | System.String   |              |                    |
|    | x  |              | リンク先                 | Address               | text         | System.String   |              |                    |
|    | x  |              | エンコーディング         | Encoding              | text         | System.String   |              |                    |
|    | x  |              | 変更検知後待機時間       | DelayTime             | text         | System.TimeSpan |              |                    |
|    | x  |              | 変更検知バッファサイズ   | BufferSize            | integer      | System.Int64    |              |                    |
|    | x  |              | 変更検知ミス更新時間     | RefreshTime           | text         | System.TimeSpan |              |                    |
|    | x  |              | 取りこぼしを考慮         | IsEnabledRefresh      | boolean      | System.Boolean  |              |                    |

## index

*NONE*



___

# NoteFiles

## layout

| PK | NN | FK           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント         |
|:--:|:--:|:-------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:-----------------|
| x  | x  | Notes.NoteId | ノートID                 | NoteId                | text         | System.Guid     |              |                  |
| x  | x  |              | ノートファイルID         | NoteFileId            | text         | System.Guid     |              |                  |
|    | x  |              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC              |
|    | x  |              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                  |
|    | x  |              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                  |
|    | x  |              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                  |
|    | x  |              | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC              |
|    | x  |              | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                  |
|    | x  |              | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                  |
|    | x  |              | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                  |
|    | x  |              | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり          |
|    | x  |              | ファイル種別             | FileKind              | text         | System.String   |              | リンク, 埋め込み |
|    | x  |              | ファイルパス             | FilePath              | text         | System.String   |              |                  |
|    | x  |              | 並び順                   | Sequence              | integer      | System.Int64    |              |                  |

## index

*NONE*



___

# KeyActions

## layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント             |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------------|
| x  | x  |    | キーアクションID         | KeyActionId           | text         | System.Guid     |              |                      |
|    | x  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                      |
|    | x  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                      |
|    | x  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |    | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |    | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                      |
|    | x  |    | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                      |
|    | x  |    | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |    | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり              |
|    | x  |    | アクション種別           | KeyActionKind         | text         | System.String   |              | ランチャー, コマンド |
|    | x  |    | アクション内容           | KeyActionContent      | text         | System.String   |              | アクション種別で変動 |
|    | x  |    | コメント                 | Comment               | text         | System.String   |              |                      |
|    | x  |    | 使用回数                 | UsageCount            | integer      | System.Int64    |              |                      |

## index

*NONE*



___

# KeyOptions

## layout

| PK | NN | FK                     | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント             |
|:--:|:--:|:-----------------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------------|
| x  | x  | KeyActions.KeyActionId | キーアクションID         | KeyActionId           | text         | System.Guid     |              |                      |
| x  | x  |                        | オプション名             | KeyOptionName         | text         | System.String   |              | アクション種別で変動 |
|    | x  |                        | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                  |
|    | x  |                        | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                      |
|    | x  |                        | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                      |
|    | x  |                        | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                      |
|    | x  |                        | オプション内容           | KeyOptionValue        | text         | System.String   |              | オプション名で変動   |

## index

*NONE*



___

# KeyMappings

## layout

| PK | NN | FK                     | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント       |
|:--:|:--:|:-----------------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------|
| x  | x  | KeyActions.KeyActionId | キーアクションID         | KeyActionId           | text         | System.Guid     |              |                |
| x  | x  |                        | 並び順                   | Sequence              | integer      | System.Int64    |              |                |
|    | x  |                        | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC            |
|    | x  |                        | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                |
|    | x  |                        | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                |
|    | x  |                        | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                |
|    | x  |                        | キー                     | Key                   | text         | System.String   |              | キー           |
|    | x  |                        | Shiftキー                | Shift                 | text         | System.String   |              | 何れか, 左, 右 |
|    | x  |                        | Ctrlキー                 | Control               | text         | System.String   |              | 何れか, 左, 右 |
|    | x  |                        | Altキー                  | Alt                   | text         | System.String   |              | 何れか, 左, 右 |
|    | x  |                        | Winキー                  | Super                 | text         | System.String   |              | 何れか, 左, 右 |

## index

*NONE*



___

# Plugins

## layout

| PK | NN | FK | 論理カラム名                       | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント     |
|:--:|:--:|:---|:-----------------------------------|:----------------------|:-------------|:----------------|:-------------|:-------------|
| x  | x  |    | プラグインID                       | PluginId              | text         | System.Guid     |              |              |
|    | x  |    | 作成タイムスタンプ                 | CreatedTimestamp      | datetime     | System.DateTime |              | UTC          |
|    | x  |    | 作成ユーザー名                     | CreatedAccount        | text         | System.String   |              |              |
|    | x  |    | 作成プログラム名                   | CreatedProgramName    | text         | System.String   |              |              |
|    | x  |    | 作成プログラムバージョン           | CreatedProgramVersion | text         | System.Version  |              |              |
|    | x  |    | 更新タイムスタンプ                 | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC          |
|    | x  |    | 更新ユーザー名                     | UpdatedAccount        | text         | System.String   |              |              |
|    | x  |    | 更新プログラム名                   | UpdatedProgramName    | text         | System.String   |              |              |
|    | x  |    | 更新プログラムバージョン           | UpdatedProgramVersion | text         | System.Version  |              |              |
|    | x  |    | 更新回数                           | UpdatedCount          | integer      | System.Int64    |              | 0始まり      |
|    | x  |    | 名前                               | Name                  | text         | System.String   |              |              |
|    | x  |    | 状態                               | State                 | text         | System.String   |              | 読み込み状態 |
|    | x  |    | 最終使用タイムスタンプ             | LastUseTimestamp      | datetime     | System.DateTime |              |              |
|    | x  |    | 最終使用プラグインバージョン       | LastUsePluginVersion  | text         | System.Version  |              |              |
|    | x  |    | 最終使用アプリケーションバージョン | LastUseAppVersion     | text         | System.Version  |              |              |
|    | x  |    | 使用回数                           | ExecuteCount          | integer      | System.Int64    |              |              |

## index

*NONE*



___

# PluginVersionChecks

## layout

| PK | NN | FK               | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:-----------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| x  | x  | Plugins.PluginId | プラグインID             | PluginId              | text         | System.Guid     |              |          |
| x  | x  |                  | シーケンス               | Sequence              | integer      | System.Int64    |              |          |
|    | x  |                  | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | x  |                  | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | x  |                  | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | x  |                  | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    | x  |                  | 更新確認URL              | CheckUrl              | text         | System.String   |              |          |

## index

*NONE*



___

# PluginSettings

## layout

| PK | NN | FK               | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント                   |
|:--:|:--:|:-----------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------------------|
| x  | x  | Plugins.PluginId | プラグインID             | PluginId              | text         | System.Guid     |              |                            |
| x  | x  |                  | プラグイン設定キー       | PluginSettingKey      | text         | System.Guid     |              | プラグイン側からのキー指定 |
|    | x  |                  | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                        |
|    | x  |                  | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                            |
|    | x  |                  | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                            |
|    | x  |                  | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                            |
|    | x  |                  | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                        |
|    | x  |                  | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                            |
|    | x  |                  | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                            |
|    | x  |                  | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                            |
|    | x  |                  | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり                    |
|    | x  |                  | データ種別               | DataType              | text         | System.String   |              |                            |
|    | x  |                  | データ値                 | DataValue             | text         | System.String   |              | xmlとかjsonとかとか        |

## index

*NONE*



___

# PluginLauncherItemSettings

## layout

| PK | NN | FK                           | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント                   |
|:--:|:--:|:-----------------------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------------------|
| x  | x  | Plugins.PluginId             | プラグインID             | PluginId              | text         | System.Guid     |              |                            |
| x  | x  | LauncherItems.LauncherItemId | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |                            |
| x  | x  |                              | プラグイン設定キー       | PluginSettingKey      | text         | System.Guid     |              | プラグイン側からのキー指定 |
|    | x  |                              | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                        |
|    | x  |                              | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                            |
|    | x  |                              | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                            |
|    | x  |                              | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                            |
|    | x  |                              | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                        |
|    | x  |                              | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                            |
|    | x  |                              | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                            |
|    | x  |                              | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                            |
|    | x  |                              | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり                    |
|    | x  |                              | データ種別               | DataType              | text         | System.String   |              |                            |
|    | x  |                              | データ値                 | DataValue             | text         | System.String   |              | xmlとかjsonとかとか        |

## index

*NONE*



___

# PluginWidgetSettings

## layout

| PK | NN | FK               | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント                                    |
|:--:|:--:|:-----------------|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:--------------------------------------------|
| x  | x  | Plugins.PluginId | プラグインID             | PluginId              | text         | System.Guid     |              |                                             |
|    | x  |                  | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                                         |
|    | x  |                  | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                                             |
|    | x  |                  | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                                             |
|    | x  |                  | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                                             |
|    | x  |                  | 更新タイムスタンプ       | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC                                         |
|    | x  |                  | 更新ユーザー名           | UpdatedAccount        | text         | System.String   |              |                                             |
|    | x  |                  | 更新プログラム名         | UpdatedProgramName    | text         | System.String   |              |                                             |
|    | x  |                  | 更新プログラムバージョン | UpdatedProgramVersion | text         | System.Version  |              |                                             |
|    | x  |                  | 更新回数                 | UpdatedCount          | integer      | System.Int64    |              | 0始まり                                     |
|    |    |                  | X座標                    | X                     | real         | System.Decimal  |              | 原点: プライマリウィンドウ左上 null時は中央 |
|    |    |                  | Y座標                    | Y                     | real         | System.Decimal  |              | 原点: プライマリウィンドウ左上 null時は中央 |
|    |    |                  | 横幅                     | Width                 | real         | System.Decimal  |              | null時はウィジェットの初期サイズ            |
|    |    |                  | 高さ                     | Height                | real         | System.Decimal  |              | null時はウィジェットの初期サイズ            |
|    | x  |                  | 表示                     | IsVisible             | boolean      | System.Boolean  |              |                                             |
|    | x  |                  | 最前面                   | IsTopmost             | boolean      | System.Boolean  |              |                                             |

## index

*NONE*

