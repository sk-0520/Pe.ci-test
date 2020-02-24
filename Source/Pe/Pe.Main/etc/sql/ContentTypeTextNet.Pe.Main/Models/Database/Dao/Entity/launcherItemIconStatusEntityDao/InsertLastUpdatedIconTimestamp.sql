insert into
	LauncherItemIconStatus
	(
		LauncherItemId,
		IconBox,
		LastUpdatedTimestamp,

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
/* LauncherItemId           */ @LauncherItemId,
/* IconBox                  */ @IconBox,
/* LastUpdatedTimestamp     */ @LastUpdatedTimestamp,
/*                          */
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

