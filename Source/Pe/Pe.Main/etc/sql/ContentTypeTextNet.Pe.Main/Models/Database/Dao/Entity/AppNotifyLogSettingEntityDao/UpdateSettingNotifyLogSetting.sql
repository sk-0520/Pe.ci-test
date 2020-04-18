update
	AppNotifyLogSetting
set
	IsVisible             = @IsVisible,
	Position              = @Position,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	AppNotifyLogSetting.Generation = (
		select
			MAX(AppNotifyLogSetting.Generation)
		from
			AppNotifyLogSetting
	)
