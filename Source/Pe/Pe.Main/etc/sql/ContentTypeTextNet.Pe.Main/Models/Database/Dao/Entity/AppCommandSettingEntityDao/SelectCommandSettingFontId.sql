select
	AppCommandSetting.FontId
from
	AppCommandSetting
where
	AppCommandSetting.Generation = (
		select
			MAX(AppCommandSetting.Generation)
		from
			AppCommandSetting
	)
