--// 退避用テーブル AppStandardInputOutputSetting2 の作成
create table
	AppStandardInputOutputSetting2
as
	select
		*
	from
		AppStandardInputOutputSetting
;

--// 現行テーブル AppStandardInputOutputSetting 破棄
drop table AppStandardInputOutputSetting;

--// table: AppStandardInputOutputSetting
create table [AppStandardInputOutputSetting] (
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
	[FontId] text not null /* フォント  */,
	[OutputForeground] text not null /* 標準出力前景色 #AARRGGBB */,
	[OutputBackground] text not null /* 標準出力背景色 #AARRGGBB */,
	[ErrorForeground] text not null /* エラー前景色 #AARRGGBB */,
	[ErrorBackground] text not null /* エラー背景色 #AARRGGBB */,
	[IsTopmost] boolean not null /* 最前面  */,
	primary key(
		[Generation]
	),
	foreign key([FontId]) references [Fonts]([FontId])
)
;

--// 退避用テーブル AppStandardInputOutputSetting2 から AppStandardInputOutputSetting へデータ移送
insert into
	AppStandardInputOutputSetting
select
	*
from
	AppStandardInputOutputSetting2
;

--// 退避用テーブル AppStandardInputOutputSetting2 の破棄
drop table AppStandardInputOutputSetting2;




