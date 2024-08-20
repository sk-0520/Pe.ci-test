select
	AppLauncherToolbarSetting.ContentDropMode,
	AppLauncherToolbarSetting.ShortcutDropMode,
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
