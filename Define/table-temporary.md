
___

## InstallPlugins

### layout

| PK | NN | FK | 論理カラム名                       | 物理カラム名           | 論理データ型 | マッピング型    | チェック制約 | コメント     |
|:--:|:--:|:---|:-----------------------------------|:-----------------------|:-------------|:----------------|:-------------|:-------------|
| o  | o  |    | プラグインID                       | PluginId               | text         | System.Guid     |              |              |
|    | o  |    | 作成タイムスタンプ                 | CreatedTimestamp       | datetime     | System.DateTime |              | UTC          |
|    | o  |    | 作成ユーザー名                     | CreatedAccount         | text         | System.String   |              |              |
|    | o  |    | 作成プログラム名                   | CreatedProgramName     | text         | System.String   |              |              |
|    | o  |    | 作成プログラムバージョン           | CreatedProgramVersion  | text         | System.Version  |              |              |
|    | o  |    | 展開ディレクトリパス               | ExtractedDirectoryPath | text         | System.String   |              |              |
|    | o  |    | プラグインディレクトリパス         | PluginDirectoryPath    | text         | System.String   |              |              |

### index

*NONE*

