--// AppExecuteSetting
update
	AppExecuteSetting
set
	ExecuteCount  = ExecuteCount + 1,
	LastVersion   = @ExecuteVersion,
	LastTimestamp = @UpdatedTimestamp,
	Accepted      = true, -- いつかの未来に丸投げする

	UpdatedTimestamp = @UpdatedTimestamp,
	UpdatedAccount = @UpdatedAccount,
	UpdatedProgramName = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount = UpdatedCount + 1
;








