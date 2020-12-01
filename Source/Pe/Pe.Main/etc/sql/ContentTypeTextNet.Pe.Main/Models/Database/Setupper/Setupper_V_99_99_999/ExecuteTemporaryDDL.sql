--// table: InstallPlugins
create table [InstallPlugins] (
	[PluginId] text not null /* プラグインID  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	PluginName text not null /* プラグイン名 */,
	PluginVersion text not null /* プラグインバージョン */,
	PluginInstallMode text not null /* プラグインインストール方法 */,
	[ExtractedDirectoryPath] text not null /* 展開ディレクトリパス  */,
	[PluginDirectoryPath] text not null /* プラグインディレクトリパス  */,
	primary key(
		[PluginId]
	)
)
;

