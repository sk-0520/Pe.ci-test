
--// [#593] table: AppNotifyLogSetting
create table [AppNotifyLogSetting] (
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
	[IsVisible] boolean not null /* 表示  */,
	[Position] text not null /* 表示位置  */,
	primary key(
		[Generation]
	)
)
;

--// [#592] table: LauncherRedoItems
create table [LauncherRedoItems] (
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
	[RedoMode] text not null /* 再実施待機方法  */,
	[WaitTime] text not null /* 待機時間  */,
	[RetryCount] integer not null /* 再実施回数  */,
	primary key(
		[LauncherItemId]
	),
	foreign key([LauncherItemId]) references [LauncherItems]([LauncherItemId])
)
;

--// [#592] table: LauncherRedoSuccessExitCodes
create table [LauncherRedoSuccessExitCodes] (
	[LauncherItemId] text not null /* ランチャーアイテムID  */,
	[SuccessExitCode] integer not null /* 正常終了コード  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	primary key(
		[LauncherItemId],
		[SuccessExitCode]
	),
	foreign key([LauncherItemId]) references [LauncherItems]([LauncherItemId])
)
;


--// [#591] 退避用テーブル Notes2 を現行テーブル Notes から生成
create table
	Notes2
as
	select
		*
	from
		Notes
;

--// [#591] 現行テーブル Notes 破棄
drop table Notes;

--// [#591] table: Notes
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
	[HiddenMode] text not null /* 隠し方 */,
	primary key(
		[NoteId]
	),
	foreign key([FontId]) references [Fonts]([FontId])
)
;

--// [#591] 退避用テーブル Note2 から Note へデータ移送
insert into
	Notes
	(
		NoteId,
		CreatedTimestamp,
		CreatedAccount,
		CreatedProgramName,
		CreatedProgramVersion,
		UpdatedTimestamp,
		UpdatedAccount,
		UpdatedProgramName,
		UpdatedProgramVersion,
		UpdatedCount,
		Title,
		ScreenName,
		LayoutKind,
		IsVisible,
		FontId,
		ForegroundColor,
		BackgroundColor,
		IsLocked,
		IsTopmost,
		IsCompact,
		TextWrap,
		ContentKind,
		HiddenMode
	)
	select
		*,
		'none'
	from
		Notes2
;

--// [#591] 退避用テーブル Note2 の破棄
drop table Notes2;

