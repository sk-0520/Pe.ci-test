select
	AppLauncherToolbarSetting.ContentDropMode,
	AppLauncherToolbarSetting.GroupMenuPosition
from
	AppLauncherToolbarSetting
where
	AppLauncherToolbarSetting.Generation = (
		select
			MAX(AppLauncherToolbarSetting.Generation)
		from
			AppLauncherToolbarSetting
	)
