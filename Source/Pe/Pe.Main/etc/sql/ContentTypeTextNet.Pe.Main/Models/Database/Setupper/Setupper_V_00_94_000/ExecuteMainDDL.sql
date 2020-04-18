--// table: AppNotifyLogSetting
create table [AppNotifyLogSetting] (
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
	[IsVisible] boolean not null /* 表示  */,
	[Position] text not null /* 表示位置  */,
	primary key(
		[Generation]
	)
)
;

--// table: LauncherRedoItems
create table [LauncherRedoItems] (
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
	[IsEnabled] boolean not null /* 有効  */,
	[WaitTime] text not null /* 待機時間  */,
	primary key(
		[LauncherItemId]
	),
	foreign key([LauncherItemId]) references [LauncherItems]([LauncherItemId])
)
;

--// table: LauncherRedoSuccessExitCodes
create table [LauncherRedoSuccessExitCodes] (
	[LauncherItemId] text not null /* ランチャーアイテムID  */,
	[SuccessExitCode] integer not null /* 正常終了コード  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	primary key(
		[LauncherItemId],
		[SuccessExitCode]
	),
	foreign key([LauncherItemId]) references [LauncherItems]([LauncherItemId])
)
;

