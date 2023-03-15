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
	[DisplayTime] text not null /* 表示するまでの抑制時間  */,
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
	'0.00:00:00.250',
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
