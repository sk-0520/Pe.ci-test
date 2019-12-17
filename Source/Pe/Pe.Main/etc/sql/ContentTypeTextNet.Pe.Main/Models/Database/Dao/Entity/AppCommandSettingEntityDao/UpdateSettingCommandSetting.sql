update
	AppCommandSetting
set
	FontId                = @FontId,
	IconBox               = @IconBox,
	HideWaitTime          = @HideWaitTime,
	FindTag               = @FindTag,
	FindFile              = @FindFile,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
