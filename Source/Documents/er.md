# テーブル一覧

___

## AppSystems

### layout

| PK | NN | FK | 論理カラム名 |     物理カラム名      | 論理データ型 | 実データ型 |  マッピング型   | チェック制約 | コメント |
|:--:|:--:|:---|:-------------|:----------------------|:-------------|:-----------|:----------------|:-------------|:---------|
| o  | o  |    |              | Key                   | text         | text       | System.String   |              |          |
|    | o  |    |              | CreatedTimestamp      | datetime     | text       | System.DateTime |              | UTC      |
|    | o  |    |              | CreatedAccount        | text         | text       | System.String   |              |          |
|    | o  |    |              | CreatedProgramName    | text         | text       | System.String   |              |          |
|    | o  |    |              | CreatedProgramVersion | text         | text       | System.Version  |              |          |
|    | o  |    |              | UpdatedTimestamp      | datetime     | text       | System.DateTime |              | UTC      |
|    | o  |    |              | UpdatedAccount        | text         | text       | System.String   |              |          |
|    | o  |    |              | UpdatedProgramName    | text         | text       | System.String   |              |          |
|    | o  |    |              | UpdatedProgramVersion | text         | text       | System.Version  |              |          |
|    | o  |    |              | UpdatedCount          | integer      | integer    | System.Int64    |              |          |
|    | o  |    |              | Value                 | text         | text       | System.String   |              |          |
|    | o  |    |              | Note                  | text         | text       | System.String   |              |          |

### index

*NONE*


___

## LauncherItems

### layout

| PK | NN | FK | 論理カラム名 |       物理カラム名       | 論理データ型 | 実データ型 |  マッピング型   | チェック制約 | コメント |
|:--:|:--:|:---|:-------------|:-------------------------|:-------------|:-----------|:----------------|:-------------|:---------|
| o  | o  |    |              | LauncherItemId           | text         | text       | System.Guid     |              |          |
|    | o  |    |              | CreatedTimestamp         | datetime     | text       | System.DateTime |              | UTC      |
|    | o  |    |              | CreatedAccount           | text         | text       | System.String   |              |          |
|    | o  |    |              | CreatedProgramName       | text         | text       | System.String   |              |          |
|    | o  |    |              | CreatedProgramVersion    | text         | text       | System.Version  |              |          |
|    | o  |    |              | UpdatedTimestamp         | datetime     | text       | System.DateTime |              | UTC      |
|    | o  |    |              | UpdatedAccount           | text         | text       | System.String   |              |          |
|    | o  |    |              | UpdatedProgramName       | text         | text       | System.String   |              |          |
|    | o  |    |              | UpdatedProgramVersion    | text         | text       | System.Version  |              |          |
|    | o  |    |              | UpdatedCount             | integer      | integer    | System.Int64    |              |          |
|    | o  |    |              | Code                     | text         | text       | System.String   |              |          |
|    | o  |    |              | Name                     | text         | text       | System.String   |              |          |
|    | o  |    |              | Kind                     | text         | text       | System.String   |              |          |
|    | o  |    |              | IconPath                 | text         | text       | System.String   |              |          |
|    | o  |    |              | IconIndex                | integer      | integer    | System.Int64    |              |          |
|    | o  |    |              | IsEnabledCommandLauncher | boolean      | integer    | System.Int64    |              |          |
|    | o  |    |              | ExecuteCount             | integer      | integer    | System.Int64    |              |          |
|    | o  |    |              | LastExecuteTimestamp     | datetime     | text       | System.DateTime |              | UTC      |
|    | o  |    |              | Note                     | text         | text       | System.String   |              |          |

### index

| UK | カラム(CSV) |
|:--:|:------------|
| o  | Code        |