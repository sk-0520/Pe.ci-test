update
	AppExecuteSetting
set
	FirstVersion         = @FirstVersion,
	FirstTimestamp       = @FirstTimestamp,
	ExecuteCount         = @ExecuteCount,

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
