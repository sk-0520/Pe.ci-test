update
	AppProxySetting
set
	ProxyIsEnabled       = @ProxyIsEnabled,
	ProxyUrl             = @ProxyUrl,
	CredentialIsEnabled  = @CredentialIsEnabled,
	CredentialUser       = @CredentialUser,
	CredentialPassword   = @CredentialPassword,

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
