update
	AppStandardInputOutputSetting
set
	FontId                = @FontId,
	OutputForeground      = @OutputForegroundColor,
	OutputBackground      = @OutputBackgroundColor,
	ErrorForeground       = @ErrorForegroundColor,
	ErrorBackground       = @ErrorBackgroundColor,
	IsTopmost             = @IsTopmost,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	AppStandardInputOutputSetting.Generation = (
		select
			MAX(AppStandardInputOutputSetting.Generation)
		from
			AppStandardInputOutputSetting
	)
