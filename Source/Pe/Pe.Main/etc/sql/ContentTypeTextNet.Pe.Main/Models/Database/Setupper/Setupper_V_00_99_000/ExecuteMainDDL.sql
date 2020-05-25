--// table: Plugins
create table [Plugins] (
	[PluginId] text not null /* プラグインID  */,
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
	[State] text not null /* 状態 読み込み状態 */,
	[LastUseTimestamp] datetime not null /* 最終使用タイムスタンプ  */,
	[LastUsePluginVersion] text not null /* 最終使用プラグインバージョン  */,
	[LastUseAppVersion] text not null /* 最終使用アプリケーションバージョン  */,
	[ExecuteCount] integer not null /* 使用回数  */,
	primary key(
		[PluginId]
	)
)
;

--// table: PluginSettings
create table [PluginSettings] (
	[PluginId] text not null /* プラグインID  */,
	[PluginSettingKey] text not null /* プラグイン設定キー プラグイン側からのキー指定 */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[DataType] text not null /* データ種別  */,
	[DataValue] text not null /* データ値 xmlとかjsonとかとか */,
	primary key(
		[PluginId],
		[PluginSettingKey]
	),
	foreign key([PluginId]) references [Plugins]([PluginId])
)
;

--// table: PluginLauncherItemSettings
create table [PluginLauncherItemSettings] (
	[PluginId] text not null /* プラグインID  */,
	[LauncherItemId] text not null /* ランチャーアイテムID  */,
	[PluginSettingKey] text not null /* プラグイン設定キー プラグイン側からのキー指定 */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[DataType] text not null /* データ種別  */,
	[DataValue] text not null /* データ値 xmlとかjsonとかとか */,
	primary key(
		[PluginId],
		[LauncherItemId],
		[PluginSettingKey]
	),
	foreign key([PluginId]) references [Plugins]([PluginId]),
	foreign key([LauncherItemId]) references [LauncherItems]([LauncherItemId])
)
;

