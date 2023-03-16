--// [#845] 退避用テーブル AppLauncherToolbarSetting2 の作成
create table
	AppLauncherToolbarSetting2
as
	select
		*
	from
		AppLauncherToolbarSetting
;

--// [#845] 現行テーブル AppLauncherToolbarSetting 破棄
drop table [AppLauncherToolbarSetting]

--// [#845] table: AppLauncherToolbarSetting
create table [AppLauncherToolbarSetting] (
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
	[PositionKind] text not null /* 表示位置 上下左右 */,
	[Direction] text not null /* 方向 アイコンの並びの基点 */,
	[IconBox] text not null /* アイコンサイズ  */,
	[FontId] text not null /* フォント  */,
	[DisplayDelayTime] text not null /* 表示するまでの抑制時間  */,
	[AutoHideTime] text not null /* 自動的に隠す時間  */,
	[TextWidth] integer not null /* 文字幅  */,
	[IsVisible] boolean not null /* 表示  */,
	[IsTopmost] boolean not null /* 最前面  */,
	[IsAutoHide] boolean not null /* 自動的に隠す  */,
	[IsIconOnly] boolean not null /* アイコンのみ  */,
	[ContentDropMode] text not null /* ツールバーへのD&D処理  */,
	[GroupMenuPosition] text not null /* グループメニュー表示位置  */,
	primary key(
		[Generation]
	),
	foreign key([FontId]) references [Fonts]([FontId])
)
;

--// [#845] 退避用テーブル AppLauncherToolbarSetting2 から AppLauncherToolbarSetting へデータ移送
insert into
	AppLauncherToolbarSetting
select
	AppLauncherToolbarSetting2.Generation,
	AppLauncherToolbarSetting2.CreatedTimestamp,
	AppLauncherToolbarSetting2.CreatedAccount,
	AppLauncherToolbarSetting2.CreatedProgramName,
	AppLauncherToolbarSetting2.CreatedProgramVersion,
	AppLauncherToolbarSetting2.UpdatedTimestamp,
	AppLauncherToolbarSetting2.UpdatedAccount,
	AppLauncherToolbarSetting2.UpdatedProgramName,
	AppLauncherToolbarSetting2.UpdatedProgramVersion,
	AppLauncherToolbarSetting2.UpdatedCount,
	AppLauncherToolbarSetting2.PositionKind,
	AppLauncherToolbarSetting2.Direction,
	AppLauncherToolbarSetting2.IconBox,
	AppLauncherToolbarSetting2.FontId,
	'0.00:00:00.250',
	AppLauncherToolbarSetting2.AutoHideTime,
	AppLauncherToolbarSetting2.TextWidth,
	AppLauncherToolbarSetting2.IsVisible,
	AppLauncherToolbarSetting2.IsTopmost,
	AppLauncherToolbarSetting2.IsAutoHide,
	AppLauncherToolbarSetting2.IsIconOnly,
	AppLauncherToolbarSetting2.ContentDropMode,
	AppLauncherToolbarSetting2.GroupMenuPosition
from
	AppLauncherToolbarSetting2
;


--// [#845] 退避用テーブル AppLauncherToolbarSetting2 の破棄
drop table [AppLauncherToolbarSetting2]
;


--// [#845] 退避用テーブル LauncherToolbars2 の作成
create table
	LauncherToolbars2
as
	select
		*
	from
		LauncherToolbars
;

--// [#845] 現行テーブル LauncherToolbars 破棄
drop table [LauncherToolbars]
;

--// [#845] table: LauncherToolbars
create table [LauncherToolbars] (
	[LauncherToolbarId] text not null /* ランチャーツールバーID  */,
	[CreatedTimestamp] datetime not null /* 作成タイムスタンプ UTC */,
	[CreatedAccount] text not null /* 作成ユーザー名  */,
	[CreatedProgramName] text not null /* 作成プログラム名  */,
	[CreatedProgramVersion] text not null /* 作成プログラムバージョン  */,
	[UpdatedTimestamp] datetime not null /* 更新タイムスタンプ UTC */,
	[UpdatedAccount] text not null /* 更新ユーザー名  */,
	[UpdatedProgramName] text not null /* 更新プログラム名  */,
	[UpdatedProgramVersion] text not null /* 更新プログラムバージョン  */,
	[UpdatedCount] integer not null /* 更新回数 0始まり */,
	[ScreenName] text not null /* スクリーン名 ドライバアップデートとかもろもろでよく変わる */,
	[LauncherGroupId] text not null /* ランチャーグループID  */,
	[PositionKind] text not null /* 表示位置 上下左右 */,
	[Direction] text not null /* 方向 アイコンの並びの基点 */,
	[IconBox] text not null /* アイコンサイズ  */,
	[FontId] text not null /* フォント  */,
	[DisplayDelayTime] text not null /* 表示するまでの抑制時間  */,
	[AutoHideTime] text not null /* 自動的に隠す時間  */,
	[TextWidth] integer not null /* 文字幅  */,
	[IsVisible] boolean not null /* 表示  */,
	[IsTopmost] boolean not null /* 最前面  */,
	[IsAutoHide] boolean not null /* 自動的に隠す  */,
	[IsIconOnly] boolean not null /* アイコンのみ  */,
	primary key(
		[LauncherToolbarId]
	),
	foreign key([FontId]) references [Fonts]([FontId])
)
;

--// [#845] 退避用テーブル LauncherToolbars2 から LauncherToolbars へデータ移送
insert into
	LauncherToolbars
select
	[LauncherToolbars2].[LauncherToolbarId],
	[LauncherToolbars2].[CreatedTimestamp],
	[LauncherToolbars2].[CreatedAccount],
	[LauncherToolbars2].[CreatedProgramName],
	[LauncherToolbars2].[CreatedProgramVersion],
	[LauncherToolbars2].[UpdatedTimestamp],
	[LauncherToolbars2].[UpdatedAccount],
	[LauncherToolbars2].[UpdatedProgramName],
	[LauncherToolbars2].[UpdatedProgramVersion],
	[LauncherToolbars2].[UpdatedCount],
	[LauncherToolbars2].[ScreenName],
	[LauncherToolbars2].[LauncherGroupId],
	[LauncherToolbars2].[PositionKind],
	[LauncherToolbars2].[Direction],
	[LauncherToolbars2].[IconBox],
	[LauncherToolbars2].[FontId],
	(
		select
			AppLauncherToolbarSetting.DisplayDelayTime
		from
			AppLauncherToolbarSetting
		where
			AppLauncherToolbarSetting.Generation = (
				select
					MAX(AppLauncherToolbarSetting.Generation)
				from
					AppLauncherToolbarSetting
			)
	),
	[LauncherToolbars2].[AutoHideTime],
	[LauncherToolbars2].[TextWidth],
	[LauncherToolbars2].[IsVisible],
	[LauncherToolbars2].[IsTopmost],
	[LauncherToolbars2].[IsAutoHide],
	[LauncherToolbars2].[IsIconOnly]
from
	[LauncherToolbars2]
;

--// [#845] 退避用テーブル LauncherToolbars2 の破棄
drop table [LauncherToolbars2]
;
