insert into
	LauncherRedoItems
	(
		LauncherItemId,
		RedoMode,
		WaitTime,
		RetryCount,

		CreatedTimestamp,
		CreatedAccount,
		CreatedProgramName,
		CreatedProgramVersion,
		UpdatedTimestamp,
		UpdatedAccount,
		UpdatedProgramName,
		UpdatedProgramVersion,
		UpdatedCount
	)
	values
	(
/* LauncherItemId           */ @LauncherItemId,
/* RedoMode                 */ @RedoMode,
/* WaitTime                 */ @WaitTime,
/* RetryCount               */ @RetryCount,
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

