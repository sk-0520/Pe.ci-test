update
	Plugins
set
	Name  = @Name,
	State = @State,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	PluginId = @PluginId
	and
	(
		Name <> @Name
		or
		State <> @State
	)
