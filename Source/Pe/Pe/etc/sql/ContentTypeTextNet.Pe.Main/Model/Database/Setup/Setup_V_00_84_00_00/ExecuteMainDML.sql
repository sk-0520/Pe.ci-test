--// AppSystems
insert into
	AppSystems
	(
		[Key],
		[Value],
		[Note],
		/*CREATE*/ CreatedTimestamp, CreatedAccount, CreatedProgramName, CreatedProgramVersion, /*UPDATE*/ UpdatedTimestamp, UpdatedAccount, UpdatedProgramName, UpdatedProgramVersion, UpdatedCount
	)
	values
	(
		'info.last.version',
		'0.84.0.0',
		'',
		/*CREATE*/ CURRENT_TIMESTAMP, @CreatedAccount, @CreatedProgramName, @CreatedProgramVersion, /*UPDATE*/ CURRENT_TIMESTAMP, @UpdatedAccount, @UpdatedProgramName, @UpdatedProgramVersion, 0
	)

;










