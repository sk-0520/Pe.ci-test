insert into
	LauncherGroupItems
	(
		[LauncherGroupId],
		[LauncherItemId],
		[Sequence],

		[CreatedTimestamp],
		[CreatedAccount],
		[CreatedProgramName],
		[CreatedProgramVersion]
	)
	values
	(
/* LauncherGroupId          */ @LauncherGroupId,
/* LauncherItemId           */ @LauncherItemId,
/* Sequence                 */ @Sequence,
/*                          */
/* CreatedTimestamp         */ @CreatedTimestamp,
/* CreatedAccount           */ @CreatedAccount,
/* CreatedProgramName       */ @CreatedProgramName,
/* CreatedProgramVersion    */ @CreatedProgramVersion
	)
