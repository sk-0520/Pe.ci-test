update
	AppUpdateSetting
set
	UpdateKind   = @UpdateKind,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	AppUpdateSetting.Generation = (
		select
			MAX(AppUpdateSetting.Generation)
		from
			AppUpdateSetting
	)
