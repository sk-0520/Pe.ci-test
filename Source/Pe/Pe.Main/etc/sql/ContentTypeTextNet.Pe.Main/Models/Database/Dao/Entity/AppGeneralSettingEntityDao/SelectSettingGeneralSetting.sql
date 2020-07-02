select
	AppGeneralSetting.Language,
	AppGeneralSetting.UserBackupDirectoryPath,
	AppGeneralSetting.ThemePluginId
from
	AppGeneralSetting
where
	AppGeneralSetting.Generation = (
		select
			MAX(AppGeneralSetting.Generation)
		from
			AppGeneralSetting
	)
