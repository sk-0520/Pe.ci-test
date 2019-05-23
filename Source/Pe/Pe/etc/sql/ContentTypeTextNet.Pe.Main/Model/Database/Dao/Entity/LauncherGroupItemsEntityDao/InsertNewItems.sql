insert into
	LauncherGroupItems
	(
		[LauncherGroupId],
		[LauncherItemId],
		[Sort],

		[CreatedTimestamp],
		[CreatedAccount],
		[CreatedProgramName],
		[CreatedProgramVersion]
	)
	values
	(
/* LauncherGroupId          */ @LauncherGroupId,
/* LauncherItemId           */ @LauncherItemId,
/* Sort                     */ @Sort,
/*                          */
/* CreatedTimestamp         */ @CreatedTimestamp,
/* CreatedAccount           */ @CreatedAccount,
/* CreatedProgramName       */ @CreatedProgramName,
/* CreatedProgramVersion    */ @CreatedProgramVersion
	)
