# テーブル一覧

___

## AppSystems

### layout

| PK | NN | FK | 論理カラム名 |     物理カラム名      | 論理データ型 |  マッピング型   | チェック制約 | コメント |
|:--:|:--:|:---|:-------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| o  | o  |    |              | Key                   | text         | System.String   |              |          |
|    | o  |    |              | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |    |              | CreatedAccount        | text         | System.String   |              |          |
|    | o  |    |              | CreatedProgramName    | text         | System.String   |              |          |
|    | o  |    |              | CreatedProgramVersion | text         | System.Version  |              |          |
|    | o  |    |              | UpdatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |    |              | UpdatedAccount        | text         | System.String   |              |          |
|    | o  |    |              | UpdatedProgramName    | text         | System.String   |              |          |
|    | o  |    |              | UpdatedProgramVersion | text         | System.Version  |              |          |
|    | o  |    |              | UpdatedCount          | integer      | System.Int64    |              |          |
|    | o  |    |              | Value                 | text         | System.String   |              |          |
|    | o  |    |              | Note                  | text         | System.String   |              |          |

### index

*NONE*


___

## LauncherItems

### layout

| PK | NN | FK | 論理カラム名 |       物理カラム名       | 論理データ型 |  マッピング型   | チェック制約 | コメント |
|:--:|:--:|:---|:-------------|:-------------------------|:-------------|:----------------|:-------------|:---------|
| o  | o  |    |              | LauncherItemId           | text         | System.Guid     |              |          |
|    | o  |    |              | CreatedTimestamp         | datetime     | System.DateTime |              | UTC      |
|    | o  |    |              | CreatedAccount           | text         | System.String   |              |          |
|    | o  |    |              | CreatedProgramName       | text         | System.String   |              |          |
|    | o  |    |              | CreatedProgramVersion    | text         | System.Version  |              |          |
|    | o  |    |              | UpdatedTimestamp         | datetime     | System.DateTime |              | UTC      |
|    | o  |    |              | UpdatedAccount           | text         | System.String   |              |          |
|    | o  |    |              | UpdatedProgramName       | text         | System.String   |              |          |
|    | o  |    |              | UpdatedProgramVersion    | text         | System.Version  |              |          |
|    | o  |    |              | UpdatedCount             | integer      | System.Int64    |              |          |
|    | o  |    |              | Code                     | text         | System.String   |              |          |
|    | o  |    |              | Name                     | text         | System.String   |              |          |
|    | o  |    |              | Kind                     | text         | System.String   |              |          |
|    | o  |    |              | IconPath                 | text         | System.String   |              |          |
|    | o  |    |              | IconIndex                | integer      | System.Int64    |              |          |
|    | o  |    |              | IsEnabledCommandLauncher | boolean      | System.Int64    |              |          |
|    | o  |    |              | ExecuteCount             | integer      | System.Int64    |              |          |
|    | o  |    |              | LastExecuteTimestamp     | datetime     | System.DateTime |              | UTC      |
|    | o  |    |              | Note                     | text         | System.String   |              |          |

### index

| UK | カラム(CSV) |
|:--:|:------------|
| o  | Code        |