--// table: AppProxySetting
create table [AppProxySetting] (
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
	[ProxyIsEnabled] boolean not null /* 有効状態  */,
	[ProxyUrl] text not null /* プロキシURL  */,
	[CredentialIsEnabled] boolean not null /* 認証有効状態  */,
	[CredentialUser] text not null /* 認証ユーザー  */,
	[CredentialPassword] blob not null /* 認証パスワード  */,
	primary key(
		[Generation]
	)
)
;

