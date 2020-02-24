select
	AppCommandSetting.FontId,
	AppCommandSetting.IconBox,
	AppCommandSetting.Width,
	AppCommandSetting.HideWaitTime,
	AppCommandSetting.FindTag
from
	AppCommandSetting
where
	AppCommandSetting.Generation = (
		select
			MAX(AppCommandSetting.Generation)
		from
			AppCommandSetting
	)

