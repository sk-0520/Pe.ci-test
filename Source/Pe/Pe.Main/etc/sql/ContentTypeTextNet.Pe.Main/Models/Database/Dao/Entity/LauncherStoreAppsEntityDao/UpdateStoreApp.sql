update
	LauncherStoreApps
set
	ProtocolAlias = @ProtocolAlias,
	Option = @Option,

	UpdatedTimestamp = @UpdatedTimestamp,
	UpdatedAccount = @UpdatedAccount,
	UpdatedProgramName = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount = UpdatedCount + 1
where
	LauncherItemId = @LauncherItemId




