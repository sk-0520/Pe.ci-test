--// AppExecuteSetting
update
	AppExecuteSetting
set
	LastVersion   = @ExecuteVersion,
	LastTimestamp = CURRENT_TIMESTAMP,
	Accepted      = 1, -- いつかの未来に丸投げする

	UpdatedTimestamp = @UpdatedTimestamp,
	UpdatedAccount = @UpdatedAccount,
	UpdatedProgramName = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount = UpdatedCount + 1
where
	AppExecuteSetting.LastVersion != @ExecuteVersion
;








