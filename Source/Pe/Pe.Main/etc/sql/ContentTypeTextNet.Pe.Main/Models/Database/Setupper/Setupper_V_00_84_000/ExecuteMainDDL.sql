--// table: AppExecuteSetting
create table [AppExecuteSetting] (
	[Generation] integer not null /* 世代 最大のものを使用する */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[Accepted] boolean not null /* 使用許諾  */,
	[FirstVersion] text not null /* 初回実行バージョン  */,
	[FirstTimestamp] datetime not null /* 初回実行日時 UTC */,
	[LastVersion] text not null /* 最終実行バージョン  */,
	[LastTimestamp] datetime not null /* 最終実行日時 UTC */,
	[ExecuteCount] integer not null /* 実行回数 0始まり */,
	[UserId] text not null /* ユーザー識別子  */,
	[IsEnabledTelemetry] boolean not null /* 使用統計情報送信  */,
	primary key(
		[Generation]
	)
)
;

--// table: AppGeneralSetting
create table [AppGeneralSetting] (
	[Generation] integer not null /* 世代 最大のものを使用する */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[Language] text not null /* 使用言語  */,
	[UserBackupDirectoryPath] text not null /* 明示的バックアップディレクトリ  */,
	primary key(
		[Generation]
	)
)
;

--// table: AppUpdateSetting
create table [AppUpdateSetting] (
	[Generation] integer not null /* 世代 最大のものを使用する */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[UpdateKind] text not null /* アップデート方法  */,
	[IgnoreVersion] text not null /* 無視するバージョン このバージョン以下を無視する */,
	primary key(
		[Generation]
	)
)
;

--// table: AppCommandSetting
create table [AppCommandSetting] (
	[Generation] integer not null /* 世代 最大のものを使用する */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[FontId] text not null /* フォント  */,
	[IconBox] text not null /* アイコンサイズ  */,
	[Width] real not null /* 横幅  */,
	[HideWaitTime] text not null /* 非表示待機時間  */,
	[FindTag]  not null /* タグ検索  */,
	primary key(
		[Generation]
	),
	foreign key([FontId]) references [Fonts]([FontId])
)
;

--// table: AppNoteSetting
create table [AppNoteSetting] (
	[Generation] integer not null /* 世代 最大のものを使用する */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[FontId] text not null /* フォント  */,
	[TitleKind] text not null /* タイトル設定  */,
	[LayoutKind] text not null /* 位置種別  */,
	[ForegroundColor] text not null /* 前景色 #AARRGGBB */,
	[BackgroundColor] text not null /* 背景色 #AARRGGBB */,
	[IsTopmost]  not null /* 最前面  */,
	primary key(
		[Generation]
	),
	foreign key([FontId]) references [Fonts]([FontId])
)
;

--// table: AppStandardInputOutputSetting
create table [AppStandardInputOutputSetting] (
	[Generation] integer not null /* 世代 最大のものを使用する */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[FontId] text not null /* フォント  */,
	[OutputForeground] text not null /* 標準出力前景色 #AARRGGBB */,
	[OutputBackground] text not null /* 標準出力背景色 #AARRGGBB */,
	[ErrorForeground] text not null /* エラー前景色 #AARRGGBB */,
	[ErrorBackground] text not null /* エラー背景色 #AARRGGBB */,
	[IsTopmost]  not null /* 最前面  */,
	primary key(
		[Generation]
	),
	foreign key([FontId]) references [Fonts]([FontId])
)
;

--// table: AppLauncherToolbarSetting
create table [AppLauncherToolbarSetting] (
	[Generation] integer not null /* 世代 最大のものを使用する */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[PositionKind] text not null /* 表示位置 上下左右 */,
	[Direction] text not null /* 方向 アイコンの並びの基点 */,
	[IconBox] text not null /* アイコンサイズ  */,
	[FontId] text not null /* フォント  */,
	[AutoHideTime] text not null /* 自動的に隠す時間  */,
	[TextWidth] integer not null /* 文字幅  */,
	[IsVisible] boolean not null /* 表示  */,
	[IsTopmost] boolean not null /* 最前面  */,
	[IsAutoHide] boolean not null /* 自動的に隠す  */,
	[IsIconOnly] boolean not null /* アイコンのみ  */,
	primary key(
		[Generation]
	),
	foreign key([FontId]) references [Fonts]([FontId])
)
;

--// table: AppPlatformSetting
create table [AppPlatformSetting] (
	[Generation] integer not null /* 世代 最大のものを使用する */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[SuppressSystemIdle] boolean not null /* アイドル抑制  */,
	[SupportExplorer] boolean not null /* Explorer補正  */,
	primary key(
		[Generation]
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
	[Comment] text not null /* コメント  */,
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
	[File] text not null /* コマンド  */,
	[Option] text not null /* コマンドオプション  */,
	[WorkDirectory] text not null /* 作業ディレクトリ  */,
	[IsEnabledCustomEnvVar] boolean not null /* 環境変数使用  */,
	[IsEnabledStandardIo] boolean not null /* 標準入出力使用  */,
	[StandardIoEncoding] text not null /* 標準入出力エンコーディング  */,
	[RunAdministrator] boolean not null /* 管理者実行  */,
	primary key(
		[LauncherItemId]
	),
	foreign key([LauncherItemId]) references [LauncherItems]([LauncherItemId])
)
;

--// table: LauncherStoreApps
create table [LauncherStoreApps] (
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
	[ProtocolAlias] text not null /* プロトコル・エイリアス  */,
	[Option] text not null /* コマンドオプション  */,
	primary key(
		[LauncherItemId]
	),
	foreign key([LauncherItemId]) references [LauncherItems]([LauncherItemId])
)
;

--// table: LauncherAddons
create table [LauncherAddons] (
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
	[PluginId] text not null /* プラグインID  */,
	primary key(
		[LauncherItemId]
	),
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
	[EnvValue] text not null /* 環境変数値  */,
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
	[LauncherItemId] text not null /* ランチャーアイテムID  */,
	[Kind] text not null /* 履歴種別 オプション, 作業ディレクトリ */,
	[Value] text not null /* 値  */,
	[LastExecuteTimestamp] datetime not null /* 最終使用日時  */,
	foreign key([LauncherItemId]) references [LauncherItems]([LauncherItemId])
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
	[IsBold] boolean not null /* ボールド  */,
	[IsItalic] boolean not null /* イタリック  */,
	[IsUnderline] boolean not null /* 下線  */,
	[IsStrikeThrough] boolean not null /* 取り消し線  */,
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
	[Sequence] integer not null /* 並び順  */,
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
	[LauncherGroupId] text not null /* ランチャーグループID  */,
	[LauncherItemId] text not null /* ランチャーアイテムID  */,
	[Sequence] integer not null /* 並び順  */,
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
	[IconBox] text not null /* アイコンサイズ  */,
	[FontId] text not null /* フォント  */,
	[AutoHideTime] text not null /* 自動的に隠す時間  */,
	[TextWidth] integer not null /* 文字幅  */,
	[IsVisible] boolean not null /* 表示  */,
	[IsTopmost] boolean not null /* 最前面  */,
	[IsAutoHide] boolean not null /* 自動的に隠す  */,
	[IsIconOnly] boolean not null /* アイコンのみ  */,
	primary key(
		[LauncherToolbarId]
	),
	foreign key([FontId]) references [Fonts]([FontId])
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
	[ForegroundColor] text not null /* 前景色 #AARRGGBB */,
	[BackgroundColor] text not null /* 背景色 #AARRGGBB */,
	[IsLocked] boolean not null /* ロック状態  */,
	[IsTopmost] boolean not null /* 最前面  */,
	[IsCompact] boolean not null /* 最小化  */,
	[TextWrap] boolean not null /* 文字列の折り返し  */,
	[ContentKind] text not null /* ノート内容種別 プレーン文字列 RTF */,
	primary key(
		[NoteId]
	),
	foreign key([FontId]) references [Fonts]([FontId])
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
	),
	foreign key([NoteId]) references [Notes]([NoteId])
)
;

--// table: NoteContents
create table [NoteContents] (
	[NoteId] text not null /* ノートID  */,
	[ContentKind] text not null /* ノート内容種別 プレーン文字列 RTF */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[IsLink] boolean not null /* リンク形式か  */,
	[Content] text not null /* 内容  */,
	[Address] text not null /* リンク先  */,
	[Encoding] text not null /* エンコーディング  */,
	[DelayTime] text not null /* 変更検知後待機時間  */,
	[BufferSize] integer not null /* 変更検知バッファサイズ  */,
	[RefreshTime] text not null /* 変更検知ミス更新時間  */,
	[IsEnabledRefresh] boolean not null /* 取りこぼしを考慮  */,
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
	[Sequence] integer not null /* 並び順  */,
	primary key(
		[NoteId]
	),
	foreign key([NoteId]) references [Notes]([NoteId])
)
;

--// table: KeyActions
create table [KeyActions] (
	[KeyActionId] text not null /* キーアクションID  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[KeyActionKind] text not null /* アクション種別 ランチャー, コマンド */,
	[KeyActionContent] text not null /* アクション内容 アクション種別で変動 */,
	[Comment] text not null /* コメント  */,
	primary key(
		[KeyActionId]
	)
)
;

--// table: KeyOptions
create table [KeyOptions] (
	[KeyActionId] text not null /* キーアクションID  */,
	[KeyOptionName] text not null /* オプション名 アクション種別で変動 */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[KeyOptionValue] text not null /* オプション内容 オプション名で変動 */,
	primary key(
		[KeyActionId],
		[KeyOptionName]
	),
	foreign key([KeyActionId]) references [KeyActions]([KeyActionId])
)
;

--// table: KeyMappings
create table [KeyMappings] (
	[KeyActionId] text not null /* キーアクションID  */,
	[Sequence] integer not null /* 並び順  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[Key] text not null /* キー キー */,
	[Shift] text not null /* Shiftキー 何れか, 左, 右 */,
	[Control] text not null /* Ctrlキー 何れか, 左, 右 */,
	[Alt] text not null /* Altキー 何れか, 左, 右 */,
	[Super] text not null /* Winキー 何れか, 左, 右 */,
	primary key(
		[KeyActionId],
		[Sequence]
	),
	foreign key([KeyActionId]) references [KeyActions]([KeyActionId])
)
;









--// index: idx_LauncherItems_1
create unique index [idx_LauncherItems_1] on [LauncherItems](
	[Code]
)
;


















