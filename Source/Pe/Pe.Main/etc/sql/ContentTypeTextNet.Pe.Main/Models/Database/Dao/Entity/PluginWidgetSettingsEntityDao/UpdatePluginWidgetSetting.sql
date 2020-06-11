update
	PluginWidgetSettings
set
	X          = @X,
	Y          = @Y,
	IsVisible  = @IsVisible,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	PluginId = @PluginId
