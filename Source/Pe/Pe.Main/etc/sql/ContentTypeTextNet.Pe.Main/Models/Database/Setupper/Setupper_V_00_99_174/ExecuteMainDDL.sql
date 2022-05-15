--// [#810] 退避用テーブル LauncherFiles2 の作成
create table
	LauncherFiles2
as
	select
		*
	from
		LauncherFiles
;

--// [#810] 現行テーブル LauncherFiles 破棄
drop table LauncherFiles;

--// [#810] table: LauncherFiles
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
	[ShowMode] text not null /* 表示方法  */,
	[IsEnabledCustomEnvVar] boolean not null /* 環境変数使用  */,
	[IsEnabledStandardIo] boolean not null /* 標準入出力使用  */,
	[StandardIoEncoding] text not null /* 標準入出力エンコーディング  */,
	[RunAdministrator] boolean not null /* 管理者実行  */,
	primary key(
		[LauncherItemId]
	),
	foreign key([LauncherItemId]) references [LauncherItems]([LauncherItemId])
)
;;

--// [#810] 退避用テーブル LauncherFiles2 から LauncherFiles へデータ移送
insert into
	LauncherFiles
select
	LauncherFiles2.LauncherItemId,
	LauncherFiles2.CreatedTimestamp,
	LauncherFiles2.CreatedAccount,
	LauncherFiles2.CreatedProgramName,
	LauncherFiles2.CreatedProgramVersion,
	LauncherFiles2.UpdatedTimestamp,
	LauncherFiles2.UpdatedAccount,
	LauncherFiles2.UpdatedProgramName,
	LauncherFiles2.UpdatedProgramVersion,
	LauncherFiles2.UpdatedCount,
	LauncherFiles2.File,
	LauncherFiles2.Option,
	LauncherFiles2.WorkDirectory,
	'normal',
	LauncherFiles2.IsEnabledCustomEnvVar,
	LauncherFiles2.IsEnabledStandardIo,
	LauncherFiles2.StandardIoEncoding,
	LauncherFiles2.RunAdministrator
from
	LauncherFiles2
;

--// [#810] 退避用テーブル LauncherFiles2 の破棄
drop table LauncherFiles2
;

