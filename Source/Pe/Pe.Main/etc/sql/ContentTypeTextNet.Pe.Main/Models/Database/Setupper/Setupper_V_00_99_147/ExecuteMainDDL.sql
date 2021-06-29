--// table: AppNoteHiddenSetting
create table [AppNoteHiddenSetting] (
	[Generation] integer not null /* 世代 最大のものを使用する */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[HiddenMode] text not null /* 隠し方  */,
	[WaitTime] text not null /* 待機時間  */,
	primary key(
		[Generation]
	)
)
;


--// index: idx_AppNoteHiddenSetting_1
create unique index [idx_AppNoteHiddenSetting_1] on [AppNoteHiddenSetting](
	[Generation],
	[HiddenMode]
)
;
