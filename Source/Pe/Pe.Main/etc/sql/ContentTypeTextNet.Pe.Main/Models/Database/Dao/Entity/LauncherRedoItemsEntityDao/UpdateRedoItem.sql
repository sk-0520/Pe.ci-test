update
	LauncherRedoItems
set
	RedoWait              = @RedoWait,
	WaitTime              = @WaitTime,
	RetryCount            = @RetryCount,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	LauncherItemId = @LauncherItemId

