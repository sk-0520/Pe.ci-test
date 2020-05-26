select
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
