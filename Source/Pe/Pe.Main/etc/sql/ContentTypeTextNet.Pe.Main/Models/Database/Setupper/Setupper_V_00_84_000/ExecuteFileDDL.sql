--// table: LauncherItemIcons
create table [LauncherItemIcons] (
	[LauncherItemId] text not null /* ランチャーアイテムID  */,
	[IconBox] text not null /* アイコン種別  */,
	[Sequence] integer not null /* 連結順序  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[Image] blob /* 画像  */,
	primary key(
		[LauncherItemId],
		[IconBox],
		[Sequence]
	)
)
;

--// table: LauncherItemIconStatus
create table [LauncherItemIconStatus] (
	[LauncherItemId] text not null /* ランチャーアイテムID  */,
	[IconBox] text not null /* アイコン種別  */,
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
		[IconBox]
	)
)
;

--// table: NoteFiles
create table [NoteFiles] (
	[NoteFileId] text not null /* ノート埋め込みファイルID  */,
	[Sequence] integer not null /* 連結順序  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[Content] blob /* ファイル内容  */,
	primary key(
		[NoteFileId],
		[Sequence]
	)
)
;



