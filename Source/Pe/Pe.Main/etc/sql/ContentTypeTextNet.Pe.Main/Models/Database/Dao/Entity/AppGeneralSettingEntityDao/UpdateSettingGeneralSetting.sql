update
	AppGeneralSetting
set
	Language                = @Language,
	UserBackupDirectoryPath = @UserBackupDirectoryPath,
	ThemePluginId           = @ThemePluginId,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	AppGeneralSetting.Generation = (
		select
			MAX(AppGeneralSetting.Generation)
		from
			AppGeneralSetting
	)
