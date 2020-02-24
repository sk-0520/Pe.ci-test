select
	AppGeneralSetting.UserBackupDirectoryPath
from
	AppGeneralSetting
where
	AppGeneralSetting.Generation = (
		select
			MAX(AppGeneralSetting.Generation)
		from
			AppGeneralSetting
	)
