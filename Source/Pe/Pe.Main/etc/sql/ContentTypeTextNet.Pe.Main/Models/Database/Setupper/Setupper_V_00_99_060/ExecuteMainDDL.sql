--// [#700] 退避用テーブル KeyActions2 の作成
create table
	KeyActions2
as
	select
		*
	from
		KeyActions
;

--// [#700] 現行テーブル KeyActions 破棄
drop table KeyActions;

--// [#700] table: KeyActions
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
	[UsageCount] integer not null /* 使用回数  */,
	[Comment] text not null /* コメント  */,
	primary key(
		[KeyActionId]
	)
)
;

--// [#700] 退避用テーブル KeyActions2 から KeyActions へデータ移送
insert into
	KeyActions
select
	KeyActions2.KeyActionId,
	KeyActions2.CreatedTimestamp,
	KeyActions2.CreatedAccount,
	KeyActions2.CreatedProgramName,
	KeyActions2.CreatedProgramVersion,
	KeyActions2.UpdatedTimestamp,
	KeyActions2.UpdatedAccount,
	KeyActions2.UpdatedProgramName,
	KeyActions2.UpdatedProgramVersion,
	KeyActions2.UpdatedCount,
	KeyActions2.KeyActionKind,
	KeyActions2.KeyActionContent,
	0,
	KeyActions2.Comment
from
	KeyActions2
;

--// [#700] 退避用テーブル KeyActions2 の破棄
drop table KeyActions2
;

