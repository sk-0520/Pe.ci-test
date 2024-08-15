insert into
	LauncherItems
	(
		LauncherItemId,
		Name,
		Kind,
		IconPath,
		IconIndex,
		IsEnabledCommandLauncher,
		ExecuteCount,
		LastExecuteTimestamp,
		Comment,

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
/* Name                     */ @Name,
/* Kind                     */ @Kind,
/* IconPath                 */ @IconPath,
/* IconIndex                */ @IconIndex,
/* IsEnabledCommandLauncher */ @IsEnabledCommandLauncher,
/* ExecuteCount             */ 0,
/* LastExecuteTimestamp     */ '0001-01-01T00:00:00',
/* Comment                  */ @Comment,
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
