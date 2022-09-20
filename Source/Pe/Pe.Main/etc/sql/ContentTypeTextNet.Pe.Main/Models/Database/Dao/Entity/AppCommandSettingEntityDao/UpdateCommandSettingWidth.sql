update
	AppCommandSetting
set
	Width                 = @Width,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	AppCommandSetting.Generation = (
		select
			MAX(AppCommandSetting.Generation)
		from
			AppCommandSetting
	)
