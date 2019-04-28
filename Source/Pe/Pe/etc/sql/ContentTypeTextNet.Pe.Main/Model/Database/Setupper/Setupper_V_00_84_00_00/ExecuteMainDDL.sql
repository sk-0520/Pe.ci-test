--// table: AppSystems
create table [AppSystems] (
	[Key] text not null /* キー  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[Value] text not null /* 値  */,
	[Comment] text not null /* コメント Peからは使用しないメモ */,
	primary key(
		[Key]
	)
)
;

--// table: LauncherItems
create table [LauncherItems] (
	[LauncherItemId] text not null /* ランチャーアイテムID  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[Code] text not null /* ランチャーアイテムコード  */,
	[Name] text not null /* 名称  */,
	[Kind] text not null /* ランチャー種別  */,
	[IconPath] text not null /* アイコンパス  */,
	[IconIndex] integer not null /* アイコンインデックス  */,
	[IsEnabledCommandLauncher] boolean not null /* コマンド入力対象  */,
	[ExecuteCount] integer not null /* 使用回数  */,
	[LastExecuteTimestamp] datetime not null /* 最終仕様日時 UTC */,
	[Comment] text not null /* メモ  */,
	primary key(
		[LauncherItemId]
	)
)
;

--// table: LauncherFiles
create table [LauncherFiles] (
	[LauncherItemId] text not null /* ランチャーアイテムID  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[Command] text not null /* コマンド  */,
	[Option] text not null /* コマンドオプション  */,
	[WorkDirectory] text not null /* 作業ディレクトリ  */,
	[IsEnabledCustomEnvVar] boolean not null /* 環境変数使用  */,
	[IsEnabledStandardOutput] boolean not null /* 標準出力使用 標準エラーも同じ扱い */,
	[IsEnabledStandardInput] boolean not null /* 標準入力使用  */,
	[Permission] text not null /* 実行権限 通常, 管理者, 別ユーザー */,
	[CredentId] text not null /* 別ユーザーアカウント 実行権限が別ユーザーの場合に有効, まぁほとんど予約かな */,
	primary key(
		[LauncherItemId]
	),
	foreign key([LauncherItemId]) references [LauncherItems]([LauncherItemId])
)
;

--// table: LauncherCommands
create table [LauncherCommands] (
	[LauncherItemId] text not null /* ランチャーアイテムID  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[Command] text not null /* コマンド  */,
	[Option] text not null /* コマンドオプション  */,
	[WorkDirectory] text not null /* 作業ディレクトリ  */,
	[IsEnabledCustomEnvVar] boolean not null /* 環境変数使用  */,
	[IsEnabledStandardOutput] boolean not null /* 標準出力使用 標準エラーも同じ扱い */,
	[IsEnabledStandardInput] boolean not null /* 標準入力使用  */,
	[Permission] text not null /* 実行権限 通常, 管理者, 別ユーザー */,
	[CredentId] text not null /* 別ユーザーアカウント 実行権限が別ユーザーの場合に有効, まぁほとんど予約かな */,
	primary key(
		[LauncherItemId]
	),
	foreign key([LauncherItemId]) references [LauncherItems]([LauncherItemId])
)
;

--// table: LauncherDirectories
create table [LauncherDirectories] (
	[LauncherItemId] text not null /* ランチャーアイテムID  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[Directory] text not null /* ディレクトリパス  */,
	primary key(
		[LauncherItemId]
	),
	foreign key([LauncherItemId]) references [LauncherItems]([LauncherItemId])
)
;

--// table: LauncherDirectoryItems
create table [LauncherDirectoryItems] (
	[LauncherItemId] text not null /* ランチャーアイテムID  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[Pattern] text not null /* パターン 正規表現 */,
	[Target] text not null /* 対象 ファイル・ディレクトリ・両方 */,
	[Whitelist] boolean not null /* ホワイトリスト  */,
	[Sort] integer not null /* 並び順  */,
	foreign key([LauncherItemId]) references [LauncherItems]([LauncherItemId])
)
;

--// table: LauncherEnvVars
create table [LauncherEnvVars] (
	[LauncherItemId] text not null /* ランチャーアイテムID  */,
	[EnvName] text not null /* 環境変数名  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[Kind] text not null /* 使用種別 追加, 置き換え, 削除 */,
	[EnvValue] text not null /* 環境変数値 追加, 置き換え で使用 */,
	primary key(
		[LauncherItemId],
		[EnvName]
	),
	foreign key([LauncherItemId]) references [LauncherItems]([LauncherItemId])
)
;

--// table: LauncherTags
create table [LauncherTags] (
	[LauncherItemId] text not null /* ランチャーアイテムID  */,
	[TagName] text not null /* タグ名  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	primary key(
		[LauncherItemId],
		[TagName]
	),
	foreign key([LauncherItemId]) references [LauncherItems]([LauncherItemId])
)
;

--// table: LauncherItemHistories
create table [LauncherItemHistories] (
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[LauncherItemId] text not null /* ランチャーアイテムID  */,
	[Kind] text not null /* 履歴種別 オプション, 作業ディレクトリ */,
	[Value] text not null /* 値  */,
	[LastExecuteTimestamp] datetime not null /* 最終使用日時  */,
	foreign key([LauncherItemId]) references [LauncherItems]([LauncherItemId])
)
;

--// table: Credents
create table [Credents] (
	[CredentId] text not null /* 資格情報ID  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[Name] text not null /* 名前  */,
	[Password] text not null /* パスワード DB上でパスワードの保護までは行わない。アプリ側でマスタパスワードを基に暗号化する方針 */,
	primary key(
		[CredentId]
	)
)
;

--// table: Fonts
create table [Fonts] (
	[FontId] text not null /* フォントID  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[FamilyName] text not null /* フォントファミリー  */,
	[Height] real not null /* 高さ  */,
	[isBold] boolean not null /* ボールド  */,
	[isItalic] boolean not null /* イタリック  */,
	[isUnderline] boolean not null /* 下線  */,
	[isStrikeThrough] boolean not null /* 取り消し線  */,
	primary key(
		[FontId]
	)
)
;

--// table: LauncherGroups
create table [LauncherGroups] (
	[LauncherGroupId] text not null /* ランチャーグループID  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[Name] text not null /* グループ名  */,
	[Kind] text not null /* グループ種別  */,
	[ImageName] text not null /* イメージ  */,
	[ImageColor] text not null /* 色 #AARRGGBB */,
	[Sort] integer not null /* 並び順  */,
	primary key(
		[LauncherGroupId]
	)
)
;

--// table: LauncherGroupItems
create table [LauncherGroupItems] (
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[LauncherGroupId] text not null /* ランチャーグループID  */,
	[LauncherItemId] text not null /* ランチャーアイテムID  */,
	[Sort] integer not null /* 並び順  */,
	foreign key([LauncherGroupId]) references [LauncherGroups]([LauncherGroupId]),
	foreign key([LauncherItemId]) references [LauncherItems]([LauncherItemId])
)
;

--// table: Screens
create table [Screens] (
	[ScreenName] text not null /* スクリーン名  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[ScreenX] integer not null /* X座標  */,
	[ScreenY] integer not null /* Y座標  */,
	[ScreenWidth] integer not null /* 横幅  */,
	[ScreenHeight] integer not null /* 高さ  */,
	primary key(
		[ScreenName]
	)
)
;

--// table: LauncherToolbars
create table [LauncherToolbars] (
	[LauncherToolbarId] text not null /* ランチャーツールバーID  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[ScreenName] text not null /* スクリーン名 ドライバアップデートとかもろもろでよく変わる */,
	[LauncherGroupId] text not null /* ランチャーグループID  */,
	[PositionKind] text not null /* 表示位置 上下左右 */,
	[Direction] text not null /* 方向 アイコンの並びの基点 */,
	[IconScale] text not null /* アイコンサイズ  */,
	[FontId] text not null /* フォント  */,
	[AutoHideTimeout] text not null /* 自動的に隠す時間  */,
	[TextWidth] integer not null /* 文字幅  */,
	[IsVisible] boolean not null /* 表示  */,
	[IsTopmost] boolean not null /* 最前面  */,
	[IsAutoHide] boolean not null /* 自動的に隠す  */,
	[IsIconOnly] boolean not null /* アイコンのみ  */,
	primary key(
		[LauncherToolbarId]
	)
)
;

--// table: Notes
create table [Notes] (
	[NoteId] text not null /* ノートID  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[Title] text not null /* タイトル  */,
	[ScreenName] text not null /* スクリーン名  */,
	[LayoutKind] text not null /* レイアウト種別  */,
	[IsVisible] boolean not null /* 表示  */,
	[FontId] text not null /* フォントID  */,
	[ForegdoundColor] text not null /* 前景色 #AARRGGBB */,
	[BackgroundColor] text not null /* 背景色 #AARRGGBB */,
	[IsLocked] boolean not null /* ロック状態  */,
	[IsTopmost] boolean not null /* 最前面  */,
	[IsCompact] boolean not null /* 最小化  */,
	[TextWrap] boolean not null /* 文字列の折り返し  */,
	[ContentKind] text not null /* ノート内容種別 プレーン文字列 RTF, KeyValue */,
	primary key(
		[NoteId]
	)
)
;

--// table: NoteLayouts
create table [NoteLayouts] (
	[NoteId] text not null /* ノートID  */,
	[LayoutKind] text not null /* レイアウト種別  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[X] real not null /* X座標 絶対: ディスプレイ基準, 相対: ディスプレイ中央 */,
	[Y] real not null /* Y座標 絶対: ディスプレイ基準, 相対: ディスプレイ中央 */,
	[Width] real not null /* 横幅 絶対: ウィンドウサイズ, 相対: ディスプレイ比率 */,
	[Height] real not null /* 高さ 絶対: ウィンドウサイズ, 相対: ディスプレイ比率 */,
	primary key(
		[NoteId],
		[LayoutKind]
	)
)
;

--// table: NoteContents
create table [NoteContents] (
	[NoteId] text not null /* ノートID  */,
	[ContentKind] text not null /* ノート内容種別  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[Content] text not null /* 内容  */,
	primary key(
		[NoteId],
		[ContentKind]
	),
	foreign key([NoteId]) references [Notes]([NoteId])
)
;

--// table: NoteFiles
create table [NoteFiles] (
	[NoteId] text not null /* ノートID  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[FileKind] text not null /* ファイル種別 リンク, 埋め込み */,
	[FilePath] text not null /* ファイルパス  */,
	[NoteFileId] text not null /* 埋め込みファイルID 埋め込みの場合に使用 */,
	[Sort] integer not null /* 並び順  */,
	primary key(
		[NoteId]
	),
	foreign key([NoteId]) references [Notes]([NoteId])
)
;


--// index: idx_LauncherItems_1
create unique index [idx_LauncherItems_1] on [LauncherItems](
	[Code]
)
;

















