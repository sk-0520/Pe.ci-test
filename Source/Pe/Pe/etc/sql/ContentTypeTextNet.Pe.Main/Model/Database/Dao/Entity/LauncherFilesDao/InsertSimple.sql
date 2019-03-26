insert into
	LauncherFiles
	(
		[LauncherItemId],
		[Command],
		[Option],
		[WorkDirectory],
		[IsEnabledCustomEnvVar],
		[IsEnabledStandardOutput],
		[IsEnabledStandardInput],
		[Permission],
		[CredentId],

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
/* Command                  */ @Command,
/* Option                   */ @Option,
/* WorkDirectory            */ @WorkDirectory,
/* IsEnabledCustomEnvVar    */ false,
/* IsEnabledStandardOutput  */ false,
/* IsEnabledStandardInput   */ false,
/* Permission               */ '',
/* CredentId                */ '00000000-0000-0000-0000-000000000000',

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
