
___

## LauncherItemIcons

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| o  | o  |    | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |          |
| o  | o  |    | アイコン種別             | IconBox               | text         | System.String   |              |          |
| o  | o  |    | アイコンスケール         | IconScale             | real         | System.Double   |              |          |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    | o  |    | 画像                     | Image                 | blob         | System.Byte[]   |              |          |
|    | o  |    | 画像最終更新日時         | LastUpdatedTimestamp  | datetime     | System.DateTime |              | UTC      |

### index

*NONE*



___

## NoteFiles

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| o  | o  |    | ノートファイルID         | NoteFileId            | text         | System.Guid     |              |          |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    | o  |    | ファイル内容             | Content               | blob         | System.Byte[]   |              |          |

### index

*NONE*



___

## PluginValues

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント                   |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------------------------|
| o  | o  |    | プラグインID             | PluginId              | text         | System.Guid     |              |                            |
| o  | o  |    | プラグイン設定キー       | PluginSettingKey      | text         | System.String   |              | プラグイン側からのキー指定 |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC                        |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |                            |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |                            |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |                            |
|    | o  |    | データ                   | Data                  | blob         | System.Byte[]   |              |                            |

### index

*NONE*

