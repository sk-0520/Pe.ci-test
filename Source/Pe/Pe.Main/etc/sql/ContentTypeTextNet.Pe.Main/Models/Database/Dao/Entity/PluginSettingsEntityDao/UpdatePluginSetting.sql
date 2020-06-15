update
	PluginSettings
set
	DataType  = @DataType,
	DataValue = @DataValue,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	PluginId = @PluginId
	and
	PluginSettingKey = @PluginSettingKey
