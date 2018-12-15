--// AppSystems
update
	AppSystems
set
	Value = @ExecuteVersion,
	UpdatedTimestamp = CURRENT_TIMESTAMP,
	UpdatedAccount = @UpdatedAccount,
	UpdatedProgramName = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount = UpdatedCount + 1
where
	AppSystems.Key = 'info.last.version'
	and
	AppSystems.Value != @ExecuteVersion
;








