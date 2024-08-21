--// [#959] 現行テーブル破棄
drop table
	LauncherItemIcons
;

drop table
	NoteFiles
;

drop table
	PluginValues
;


--// table: LauncherItemIcons
create table [LauncherItemIcons] (
	[LauncherItemId] text not null /* ランチャーアイテムID  */,
	[IconBox] text not null /* アイコン種別  */,
	[IconScale] real not null /* アイコンスケール  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[Image] blob not null /* 画像  */,
	primary key(
		[LauncherItemId],
		[IconBox],
		[IconScale]
	)
)
;

--// table: NoteFiles
create table [NoteFiles] (
	[NoteFileId] text not null /* ノートファイルID  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[Content] blob not null /* ファイル内容  */,
	primary key(
		[NoteFileId]
	)
)
;

--// table: PluginValues
create table [PluginValues] (
	[PluginId] text not null /* プラグインID  */,
	[PluginSettingKey] text not null /* プラグイン設定キー プラグイン側からのキー指定 */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[Data] blob not null /* データ  */,
	primary key(
		[PluginId],
		[PluginSettingKey]
	)
)
;
