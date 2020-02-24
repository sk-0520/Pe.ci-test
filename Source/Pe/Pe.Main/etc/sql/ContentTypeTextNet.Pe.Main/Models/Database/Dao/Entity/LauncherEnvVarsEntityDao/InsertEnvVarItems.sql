insert into
	LauncherEnvVars
	(
		[LauncherItemId],
		[EnvName],
		[EnvValue],

		[CreatedTimestamp],
		[CreatedAccount],
		[CreatedProgramName],
		[CreatedProgramVersion]
	)
	values
	(
/* LauncherItemId           */ @LauncherItemId,
/* EnvName                  */ @EnvName,
/* EnvValue                 */ @EnvValue,
/*                          */
/* CreatedTimestamp         */ @CreatedTimestamp,
/* CreatedAccount           */ @CreatedAccount,
/* CreatedProgramName       */ @CreatedProgramName,
/* CreatedProgramVersion    */ @CreatedProgramVersion
	)
