update
	AppProxySetting
set
	ProxyIsEnabled       = case
		when ProxyIsEnabled then false
		else true
	end,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	AppProxySetting.Generation = (
		select
			MAX(AppProxySetting.Generation)
		from
			AppProxySetting
	)
