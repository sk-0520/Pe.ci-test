update
	AppProxySetting
set
	ProxyIsEnabled = not ProxyIsEnabled,

	UpdatedTimestamp = @UpdatedTimestamp,
	UpdatedAccount = @UpdatedAccount,
	UpdatedProgramName = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount = UpdatedCount + 1
where
	AppProxySetting.Generation = (
		select
			MAX(AppProxySetting.Generation)
		from
			AppProxySetting
	)
