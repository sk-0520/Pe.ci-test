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
	values
	(
/* ToolbarId                */ @LauncherToolbarId,
/* ScreenName               */ @ScreenName,
/* LauncherGroupId          */ @LauncherGroupId,
/* PositionKind             */ @PositionKind,
/* Direction                */ @Direction,
/* IconBox                  */ @IconBox,
/* FontId                   */ @FontId,
/* AutoHideTime             */ @AutoHideTime,
/* TextWidth                */ @TextWidth,
/* IsVisible                */ @IsVisible,
/* IsTopmost                */ @IsTopmost,
/* IsAutoHide               */ @IsAutoHide,
/* IsIconOnly               */ @IsIconOnly,

/* CreatedTimestamp         */ @CreatedTimestamp,
/* CreatedAccount           */ @CreatedAccount,
/* CreatedProgramName       */ @CreatedProgramName,
/* CreatedProgramVersion    */ @CreatedProgramVersion,
/* UpdatedTimestamp         */ @UpdatedTimestamp,
/* UpdatedAccount           */ @UpdatedAccount,
/* UpdatedProgramName       */ @UpdatedProgramName,
/* UpdatedProgramVersion    */ @UpdatedProgramVersion,
/* UpdatedCount             */ 0
	)
