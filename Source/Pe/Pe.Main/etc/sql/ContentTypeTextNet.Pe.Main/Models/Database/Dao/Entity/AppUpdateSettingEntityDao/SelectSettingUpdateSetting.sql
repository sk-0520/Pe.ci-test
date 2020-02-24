select
	AppUpdateSetting.UpdateKind
from
	AppUpdateSetting
where
	AppUpdateSetting.Generation = (
		select
			MAX(AppUpdateSetting.Generation)
		from
			AppUpdateSetting
	)
