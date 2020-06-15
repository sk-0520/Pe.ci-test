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


--// table: PluginWidgetSettings
create table [PluginWidgetSettings] (
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
	[X] real /* X座標 原点: プライマリウィンドウ左上 null時は中央 */,
	[Y] real /* Y座標 原点: プライマリウィンドウ左上 null時は中央 */,
	[Width] real /* 横幅 null時はウィジェットの初期サイズ */,
	[Height] real /* 高さ null時はウィジェットの初期サイズ */,
	[IsVisible] boolean not null /* 表示  */,
	[IsTopmost] boolean not null /* 最前面  */,
	primary key(
		[PluginId]
	),
	foreign key([PluginId]) references [Plugins]([PluginId])
)
;


--// [#509] 退避用テーブル AppGeneralSetting2 の作成
create table
	AppGeneralSetting2
as
	select
		*
	from
		AppGeneralSetting
;

--// [#509] 現行テーブル AppGeneralSetting 破棄
drop table AppGeneralSetting;

--// [#509] table: AppGeneralSetting
create table [AppGeneralSetting] (
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
	[Language] text not null /* 使用言語  */,
	[UserBackupDirectoryPath] text not null /* 明示的バックアップディレクトリ  */,
	[ThemePluginId] text not null /* テーマプラグインID  */,
	primary key(
		[Generation]
	)
)
;

--// [#509] 退避用テーブル AppGeneralSetting2 から AppGeneralSetting へデータ移送
insert into
	AppGeneralSetting
select
	AppGeneralSetting2.Generation,
	AppGeneralSetting2.CreatedTimestamp,
	AppGeneralSetting2.CreatedAccount,
	AppGeneralSetting2.CreatedProgramName,
	AppGeneralSetting2.CreatedProgramVersion,
	AppGeneralSetting2.UpdatedTimestamp,
	AppGeneralSetting2.UpdatedAccount,
	AppGeneralSetting2.UpdatedProgramName,
	AppGeneralSetting2.UpdatedProgramVersion,
	AppGeneralSetting2.UpdatedCount,
	AppGeneralSetting2.Language,
	AppGeneralSetting2.UserBackupDirectoryPath,
	'4524fc23-ebb9-4c79-a26b-8f472c05095e' -- Pe.Plugins.DefaultTheme
from
	AppGeneralSetting2
;

--// [#509] 退避用テーブル AppGeneralSetting2 の破棄
drop table AppGeneralSetting2
;


