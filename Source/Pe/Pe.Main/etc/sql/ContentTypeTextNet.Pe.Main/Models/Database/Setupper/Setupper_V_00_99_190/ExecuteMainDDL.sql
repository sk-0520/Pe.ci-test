--// table: NoteFiles
drop table [NoteFiles]
;

--// table: NoteFiles
create table [NoteFiles] (
	[NoteId] text not null /* ノートID  */,
	[NoteFileId] text not null /* 埋め込みファイルID 埋め込みの場合に使用 */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[FileKind] text not null /* ファイル種別 リンク, 埋め込み */,
	[FilePath] text not null /* ファイルパス  */,
	[Sequence] integer not null /* 並び順  */,
	primary key(
		[NoteId],
		[NoteFileId]
	),
	foreign key([NoteId]) references [Notes]([NoteId])
)
;
