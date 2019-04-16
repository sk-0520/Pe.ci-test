# ファイルテーブル一覧


___

## LauncherItemIcons

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| o  | o  |    | ランチャーアイテムID     | LauncherItemId        | text         | System.Guid     |              |          |
| o  | o  |    | アイコン種別             | IconScale             | text         | System.String   |              |          |
| o  | o  |    | 連結順序                 | Sequence              | integer      | System.Int64    |              |          |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    |    |    | 画像                     | Image                 | blob         | System.Byte[]   |              |          |

### index

*NONE*



___

## NoteFiles

### layout

| PK | NN | FK | 論理カラム名             | 物理カラム名          | 論理データ型 | マッピング型    | チェック制約 | コメント |
|:--:|:--:|:---|:-------------------------|:----------------------|:-------------|:----------------|:-------------|:---------|
| o  | o  |    | ノート埋め込みファイルID | NoteFileId            | text         | System.Guid     |              |          |
| o  | o  |    | 連結順序                 | Sequence              | integer      | System.Int64    |              |          |
|    | o  |    | 作成タイムスタンプ       | CreatedTimestamp      | datetime     | System.DateTime |              | UTC      |
|    | o  |    | 作成ユーザー名           | CreatedAccount        | text         | System.String   |              |          |
|    | o  |    | 作成プログラム名         | CreatedProgramName    | text         | System.String   |              |          |
|    | o  |    | 作成プログラムバージョン | CreatedProgramVersion | text         | System.Version  |              |          |
|    |    |    | ファイル内容             | Content               | blob         | System.Byte[]   |              |          |

### index

*NONE*

