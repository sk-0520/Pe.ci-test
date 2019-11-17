update
	LauncherItems
set
	ExecuteCount = ExecuteCount + 1,

	UpdatedTimestamp = @UpdatedTimestamp,
	UpdatedAccount = @UpdatedAccount,
	UpdatedProgramName = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount = UpdatedCount + 1
where
	LauncherItemId = @LauncherItemId

