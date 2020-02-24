update
	AppExecuteSetting
set
	UserId                = @UserId,
	IsEnabledTelemetry   = @IsEnabledTelemetry,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	AppExecuteSetting.Generation = (
		select
			MAX(AppExecuteSetting.Generation)
		from
			AppExecuteSetting
	)
