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
		[AutoHideTimeout],
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
/* LauncherGroupId          */ '00000000-0000-0000-0000-000000000000',
/* PositionKind             */ 'right',
/* Direction                */ 'left-top',
/* IconBox                  */ 'normal',
/* FontId                   */ '00000000-0000-0000-0000-000000000000',
/* AutoHideTimeout          */ '00:00:03',
/* TextWidth                */ 80,
/* IsVisible                */ true,
/* IsTopmost                */ true,
/* IsAutoHide               */ false,
/* IsIconOnly               */ true,

/*                          */
/* CreatedTimestamp         */ CURRENT_TIMESTAMP,
/* CreatedAccount           */ @CreatedAccount,
/* CreatedProgramName       */ @CreatedProgramName,
/* CreatedProgramVersion    */ @CreatedProgramVersion,
/* UpdatedTimestamp         */ CURRENT_TIMESTAMP,
/* UpdatedAccount           */ @UpdatedAccount,
/* UpdatedProgramName       */ @UpdatedProgramName,
/* UpdatedProgramVersion    */ @UpdatedProgramVersion,
/* UpdatedCount             */ 0
	)
