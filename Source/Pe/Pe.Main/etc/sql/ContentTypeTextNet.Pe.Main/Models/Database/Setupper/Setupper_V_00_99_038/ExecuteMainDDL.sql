--// [#685] 退避用テーブル AppNoteSetting2 の作成
create table
	AppNoteSetting2
as
	select
		*
	from
		AppNoteSetting
;

--// [#685] 現行テーブル AppNoteSetting 破棄
drop table AppNoteSetting;

--// [#685] table: AppNoteSetting
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
	[IsTopmost] boolean not null /* 最前面  */,
	[CaptionPosition] text not null /* タイトル位置  */,
	primary key(
		[Generation]
	),
	foreign key([FontId]) references [Fonts]([FontId])
)
;

--// [#685] 退避用テーブル AppNoteSetting2 から AppNoteSetting へデータ移送
insert into
	AppNoteSetting
select
	AppNoteSetting2.Generation,
	AppNoteSetting2.CreatedTimestamp,
	AppNoteSetting2.CreatedAccount,
	AppNoteSetting2.CreatedProgramName,
	AppNoteSetting2.CreatedProgramVersion,
	AppNoteSetting2.UpdatedTimestamp,
	AppNoteSetting2.UpdatedAccount,
	AppNoteSetting2.UpdatedProgramName,
	AppNoteSetting2.UpdatedProgramVersion,
	AppNoteSetting2.UpdatedCount,
	AppNoteSetting2.FontId,
	AppNoteSetting2.TitleKind,
	AppNoteSetting2.LayoutKind,
	AppNoteSetting2.ForegroundColor,
	AppNoteSetting2.BackgroundColor,
	AppNoteSetting2.IsTopmost,
	'top'
from
	AppNoteSetting2
;

--// [#685] 退避用テーブル AppNoteSetting2 の破棄
drop table AppNoteSetting2
;


--// [#685] 退避用テーブル Notes2 の作成
create table
	Notes2
as
	select
		*
	from
		Notes
;

--// [#685] 現行テーブル Notes 破棄
drop table Notes;

--// [#685] table: Notes
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
	[HiddenMode] text not null /* 隠し方  */,
	[CaptionPosition] text not null /* タイトル位置  */,
	primary key(
		[NoteId]
	),
	foreign key([FontId]) references [Fonts]([FontId])
)
;

--// [#685] 退避用テーブル Notes2 から Notes へデータ移送
insert into
	Notes
select
	Notes2.NoteId,
	Notes2.CreatedTimestamp,
	Notes2.CreatedAccount,
	Notes2.CreatedProgramName,
	Notes2.CreatedProgramVersion,
	Notes2.UpdatedTimestamp,
	Notes2.UpdatedAccount,
	Notes2.UpdatedProgramName,
	Notes2.UpdatedProgramVersion,
	Notes2.UpdatedCount,
	Notes2.Title,
	Notes2.ScreenName,
	Notes2.LayoutKind,
	Notes2.IsVisible,
	Notes2.FontId,
	Notes2.ForegroundColor,
	Notes2.BackgroundColor,
	Notes2.IsLocked,
	Notes2.IsTopmost,
	Notes2.IsCompact,
	Notes2.TextWrap,
	Notes2.ContentKind,
	Notes2.HiddenMode,
	'top'
from
	Notes2
;

--// [#685] 退避用テーブル Notes2 の破棄
drop table Notes2
;



