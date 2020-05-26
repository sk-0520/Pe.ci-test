update
	Plugins
set
	LastUseTimestamp     = @LastUseTimestamp,
	LastUsePluginVersion = @LastUsePluginVersion,
	LastUseAppVersion    = @LastUseAppVersion,
	ExecuteCount         = ExecuteCount + 1,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	PluginId = @PluginId
