update
	AppUpdateSetting
set
	CheckReleaseVersion   = @CheckReleaseVersion,
	CheckRcVersion        = @CheckRcVersion,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
