select
	AppLauncherToolbarSetting.FontId
from
	AppLauncherToolbarSetting
where
	AppLauncherToolbarSetting.Generation = (
		select
			MAX(AppLauncherToolbarSetting.Generation)
		from
			AppLauncherToolbarSetting
	)
