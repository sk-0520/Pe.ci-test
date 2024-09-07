# LauncherItemIcons

## layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | コメント |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:---------|
| x  | x  |    | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |          |
| x  | x  |    | アイコン種別             | IconBox               | text         | System.String   |          |
| x  | x  |    | アイコンスケール         | IconScale             | real         | System.Double   |          |
|    | x  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime | UTC      |
|    | x  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |          |
|    | x  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |          |
|    | x  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |          |
|    | x  |    | 画像                     | Image                 | blob         | System.Byte[]   |          |
|    | x  |    | 画像最終更新日時         | LastUpdatedTimestamp  | datetime     | System.DateTime | UTC      |

## index

*NONE*



___

# NoteFiles

## layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | コメント |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:---------|
| x  | x  |    | ノートファイルID         | NoteFileId            | text         | System.Guid     |          |
|    | x  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime | UTC      |
|    | x  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |          |
|    | x  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |          |
|    | x  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |          |
|    | x  |    | ファイル内容             | Content               | blob         | System.Byte[]   |          |

## index

*NONE*



___

# PluginValues

## layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | コメント                   |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:---------------------------|
| x  | x  |    | プラグインID             | PluginId              | text         | System.Guid     |                            |
| x  | x  |    | プラグイン設定キー       | PluginSettingKey      | text         | System.String   | プラグイン側からのキー指定 |
|    | x  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime | UTC                        |
|    | x  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |                            |
|    | x  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |                            |
|    | x  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |                            |
|    | x  |    | データ                   | Data                  | blob         | System.Byte[]   |                            |

## index

*NONE*

