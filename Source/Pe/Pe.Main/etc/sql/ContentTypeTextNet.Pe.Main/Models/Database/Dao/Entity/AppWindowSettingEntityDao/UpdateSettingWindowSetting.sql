update
	AppWindowSetting
set
	IsEnabled             = @IsEnabled,
	Count                 = @Count,
	Interval              = @Interval,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
