--// [#936] 退避テーブル作成
create table
	LauncherItems2
as
	select
		*
	from
		LauncherItems
;

--// [#936] 現行テーブル破棄
drop table
	[LauncherItems]
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

insert into
	[LauncherItems]
select
	[LauncherItems2].[LauncherItemId],
	[LauncherItems2].[CreatedTimestamp],
	[LauncherItems2].[CreatedAccount],
	[LauncherItems2].[CreatedProgramName],
	[LauncherItems2].[CreatedProgramVersion],
	[LauncherItems2].[UpdatedTimestamp],
	[LauncherItems2].[UpdatedAccount],
	[LauncherItems2].[UpdatedProgramName],
	[LauncherItems2].[UpdatedProgramVersion],
	[LauncherItems2].[UpdatedCount],
	[LauncherItems2].[Name],
	[LauncherItems2].[Kind],
	[LauncherItems2].[IconPath],
	[LauncherItems2].[IconIndex],
	[LauncherItems2].[IsEnabledCommandLauncher],
	[LauncherItems2].[ExecuteCount],
	[LauncherItems2].[LastExecuteTimestamp],
	[LauncherItems2].[Comment]
from
	[LauncherItems2]
;

-- コードをタグに移動(単純に移動は被るので存在しない場合のみに限定)
insert into
	[LauncherTags]
select
	[LauncherItems2].[LauncherItemId],
	[LauncherItems2].[Code],
	[LauncherItems2].[CreatedTimestamp],
	[LauncherItems2].[CreatedAccount],
	[LauncherItems2].[CreatedProgramName],
	[LauncherItems2].[CreatedProgramVersion]
from
	[LauncherItems2]
where
	not exists (
		select
			*
		from
			[LauncherTags]
		where
			[LauncherTags].[LauncherItemId] = [LauncherItems2].[LauncherItemId]
			and
			[LauncherTags].[TagName] = [LauncherItems2].[Code]
	)
;

--// [#936] 退避テーブル破棄
drop table [LauncherItems2]
;
