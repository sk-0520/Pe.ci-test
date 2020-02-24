update
	KeyActions
set
	KeyActionContent      = @KeyActionContent,
	Comment               = @Comment,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	KeyActionId           = @KeyActionId
