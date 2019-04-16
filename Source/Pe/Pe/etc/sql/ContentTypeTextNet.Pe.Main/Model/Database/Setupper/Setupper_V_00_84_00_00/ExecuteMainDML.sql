--// AppSystems
insert into
	AppSystems
	(
		[Key],
		[Value],
		[Comment],
		/*CREATE*/ CreatedTimestamp, CreatedAccount, CreatedProgramName, CreatedProgramVersion, /*UPDATE*/ UpdatedTimestamp, UpdatedAccount, UpdatedProgramName, UpdatedProgramVersion, UpdatedCount
	)
	values
	(
		'info.last.version',
		'0.84.0.0',
		'',
		/*CREATE*/ @CreatedTimestamp, @CreatedAccount, @CreatedProgramName, @CreatedProgramVersion, /*UPDATE*/ @UpdatedTimestamp, @UpdatedAccount, @UpdatedProgramName, @UpdatedProgramVersion, 0
	)

;










