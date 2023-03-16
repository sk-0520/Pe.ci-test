insert into
	LauncherToolbars
	(
		[LauncherToolbarId],
		[ScreenName],
		[LauncherGroupId],
		[PositionKind],
		[Direction],
		[IconBox],
		[FontId],
		[DisplayDelayTime],
		[AutoHideTime],
		[TextWidth],
		[IsVisible],
		[IsTopmost],
		[IsAutoHide],
		[IsIconOnly],

		[CreatedTimestamp],
		[CreatedAccount],
		[CreatedProgramName],
		[CreatedProgramVersion],
		[UpdatedTimestamp],
		[UpdatedAccount],
		[UpdatedProgramName],
		[UpdatedProgramVersion],
		[UpdatedCount]
	)
	select
/* ToolbarId                */ @LauncherToolbarId,
/* ScreenName               */ @ScreenName,
/* LauncherGroupId          */ '00000000-0000-0000-0000-000000000000',
/* PositionKind             */ AppLauncherToolbarSetting.PositionKind,
/* Direction                */ AppLauncherToolbarSetting.Direction,
/* IconBox                  */ AppLauncherToolbarSetting.IconBox,
/* FontId                   */ @FontId,
/* DisplayDelayTime         */ AppLauncherToolbarSetting.DisplayDelayTime,
/* AutoHideTime             */ AppLauncherToolbarSetting.AutoHideTime,
/* TextWidth                */ AppLauncherToolbarSetting.TextWidth,
/* IsVisible                */ AppLauncherToolbarSetting.IsVisible,
/* IsTopmost                */ AppLauncherToolbarSetting.IsTopmost,
/* IsAutoHide               */ AppLauncherToolbarSetting.IsAutoHide,
/* IsIconOnly               */ AppLauncherToolbarSetting.IsIconOnly,

/* CreatedTimestamp         */ @CreatedTimestamp,
/* CreatedAccount           */ @CreatedAccount,
/* CreatedProgramName       */ @CreatedProgramName,
/* CreatedProgramVersion    */ @CreatedProgramVersion,
/* UpdatedTimestamp         */ @UpdatedTimestamp,
/* UpdatedAccount           */ @UpdatedAccount,
/* UpdatedProgramName       */ @UpdatedProgramName,
/* UpdatedProgramVersion    */ @UpdatedProgramVersion,
/* UpdatedCount             */ 0
	from
		AppLauncherToolbarSetting
	where
		AppLauncherToolbarSetting.Generation = (
			select
				MAX(AppLauncherToolbarSetting.Generation)
			from
				AppLauncherToolbarSetting
		)
