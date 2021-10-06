--// table: PluginVersionChecks
create table [PluginVersionChecks] (
	[PluginId] text not null /* プラグインID  */,
	[Sequence] integer not null /* シーケンス  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[CheckUrl] text not null /* 更新確認URL  */,
	primary key(
		[PluginId],
		[Sequence]
	),
	foreign key([PluginId]) references [Plugins]([PluginId])
)
;
