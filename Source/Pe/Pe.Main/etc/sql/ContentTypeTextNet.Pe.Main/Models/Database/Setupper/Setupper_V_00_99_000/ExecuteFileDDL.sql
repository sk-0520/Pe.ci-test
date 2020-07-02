--// table: PluginValues
create table [PluginValues] (
	[PluginId] text not null /* プラグインID  */,
	[PluginSettingKey] text not null /* プラグイン設定キー プラグイン側からのキー指定 */,
	[Sequence] integer not null /* 連結順序  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[Data] blob not null /* データ  */,
	primary key(
		[PluginId],
		[PluginSettingKey],
		[Sequence]
	)
)
;
