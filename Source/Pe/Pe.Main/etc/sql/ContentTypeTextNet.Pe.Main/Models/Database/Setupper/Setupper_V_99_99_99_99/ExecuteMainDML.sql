--// AppExecuteSetting
update
	AppExecuteSetting
set
	LastVersion   = @ExecuteVersion,
	LastTimestamp = CURRENT_TIMESTAMP,

	UpdatedTimestamp = @UpdatedTimestamp,
	UpdatedAccount = @UpdatedAccount,
	UpdatedProgramName = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount = UpdatedCount + 1
where
	AppExecuteSetting.LastVersion != @ExecuteVersion
;








