update
	AppPlatformSetting
set
	SuppressSystemIdle    = @SuppressSystemIdle,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	AppPlatformSetting.Generation = (
		select
			MAX(AppPlatformSetting.Generation)
		from
			AppPlatformSetting
	)
