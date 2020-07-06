--// [#661] 退避用テーブル LauncherItemIconStatus2 の作成
create table
	LauncherItemIconStatus2
as
	select
		*
	from
		LauncherItemIconStatus
;

--// [#661] 現行テーブル LauncherItemIconStatus 破棄
drop table LauncherItemIconStatus;

--// [#661] table: LauncherItemIconStatus
create table [LauncherItemIconStatus] (
	[LauncherItemId] text not null /* ランチャーアイテムID  */,
	[IconBox] text not null /* アイコン種別  */,
	[IconScale] real not null /* アイコンスケール  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[LastUpdatedTimestamp] datetime not null /* 最終更新日時 UTC */,
	primary key(
		[LauncherItemId],
		[IconBox],
		[IconScale]
	)
)
;

--// [#661] 退避用テーブル LauncherItemIconStatus2 から LauncherItemIconStatus へデータ移送
insert into
	LauncherItemIconStatus
select
	LauncherItemIconStatus2.LauncherItemId,
	LauncherItemIconStatus2.IconBox,
	1.0,
	LauncherItemIconStatus2.CreatedTimestamp,
	LauncherItemIconStatus2.CreatedAccount,
	LauncherItemIconStatus2.CreatedProgramName,
	LauncherItemIconStatus2.CreatedProgramVersion,
	LauncherItemIconStatus2.UpdatedTimestamp,
	LauncherItemIconStatus2.UpdatedAccount,
	LauncherItemIconStatus2.UpdatedProgramName,
	LauncherItemIconStatus2.UpdatedProgramVersion,
	LauncherItemIconStatus2.UpdatedCount,
	LauncherItemIconStatus2.LastUpdatedTimestamp
from
	LauncherItemIconStatus2
;

--// [#661] 退避用テーブル LauncherItemIconStatus2 の破棄
drop table LauncherItemIconStatus2
;

--// [#661] 退避用テーブル LauncherItemIcons2 の作成
create table
	LauncherItemIcons2
as
	select
		*
	from
		LauncherItemIcons
;

--// [#661] 現行テーブル LauncherItemIcons 破棄
drop table LauncherItemIcons;

--// [#661] table: LauncherItemIcons
create table [LauncherItemIcons] (
	[LauncherItemId] text not null /* ランチャーアイテムID  */,
	[IconBox] text not null /* アイコン種別  */,
	[IconScale] real not null /* アイコンスケール  */,
	[Sequence] integer not null /* 連結順序  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[Image] blob /* 画像  */,
	primary key(
		[LauncherItemId],
		[IconBox],
		[IconScale],
		[Sequence]
	)
)
;

--// [#661] 退避用テーブル LauncherItemIcons2 から LauncherItemIcons へデータ移送
insert into
	LauncherItemIcons
select
	LauncherItemIcons2.LauncherItemId,
	LauncherItemIcons2.IconBox,
	1.0,
	LauncherItemIcons2.Sequence,
	LauncherItemIcons2.CreatedTimestamp,
	LauncherItemIcons2.CreatedAccount,
	LauncherItemIcons2.CreatedProgramName,
	LauncherItemIcons2.CreatedProgramVersion,
	LauncherItemIcons2.Image
from
	LauncherItemIcons2
;

--// [#661] 退避用テーブル LauncherItemIcons2 の破棄
drop table LauncherItemIcons2
;


