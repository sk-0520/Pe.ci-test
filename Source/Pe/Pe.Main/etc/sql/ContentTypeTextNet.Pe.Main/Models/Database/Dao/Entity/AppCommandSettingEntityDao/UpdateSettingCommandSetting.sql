update
	AppCommandSetting
set
	FontId                = @FontId,
	IconBox               = @IconBox,
	Width                 = @Width,
	HideWaitTime          = @HideWaitTime,
	FindTag               = @FindTag,

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
