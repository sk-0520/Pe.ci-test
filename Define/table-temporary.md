# InstallPlugins

## layout

| PK | NN | FK |        論理カラム名        |      物理カラム名      | 論理データ型 |  マッピング型   | コメント |
|:--:|:--:|:---|:---------------------------|:-----------------------|:-------------|:----------------|:---------|
| x  | x  |    | プラグインID               | PluginId               | text         | System.Guid     |          |
|    | x  |    | 作成タイムスタンプ         | CreatedTimestamp       | datetime     | System.DateTime | UTC      |
|    | x  |    | 作成ユーザー名             | CreatedAccount         | text         | System.String   |          |
|    | x  |    | 作成プログラム名           | CreatedProgramName     | text         | System.String   |          |
|    | x  |    | 作成プログラムバージョン   | CreatedProgramVersion  | text         | System.Version  |          |
|    | x  |    | プラグイン名               | PluginName             | text         | System.String   |          |
|    | x  |    | プラグインバージョン       | PluginVersion          | text         | System.Version  |          |
|    | x  |    | プラグインインストール方法 | PluginInstallMode      | text         | System.String   |          |
|    | x  |    | 展開ディレクトリパス       | ExtractedDirectoryPath | text         | System.String   |          |
|    | x  |    | プラグインディレクトリパス | PluginDirectoryPath    | text         | System.String   |          |

## index

*NONE*
