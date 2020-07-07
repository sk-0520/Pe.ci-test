update
	LauncherItemIconStatus
set
	LastUpdatedTimestamp = @LastUpdatedTimestamp,

	UpdatedTimestamp = @UpdatedTimestamp,
	UpdatedAccount = @UpdatedAccount,
	UpdatedProgramName = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount = UpdatedCount + 1
where
	LauncherItemId = @LauncherItemId
	and
	IconBox = @IconBox
	and
	IconScale = @IconScale


