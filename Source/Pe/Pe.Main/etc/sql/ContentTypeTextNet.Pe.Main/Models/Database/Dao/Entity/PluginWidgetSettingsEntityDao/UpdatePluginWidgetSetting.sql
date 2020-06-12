update
	PluginWidgetSettings
set
	X          = @X,
	Y          = @Y,
	Width      = @Width,
	Height     = @Height,
	IsVisible  = @IsVisible,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	PluginId = @PluginId
