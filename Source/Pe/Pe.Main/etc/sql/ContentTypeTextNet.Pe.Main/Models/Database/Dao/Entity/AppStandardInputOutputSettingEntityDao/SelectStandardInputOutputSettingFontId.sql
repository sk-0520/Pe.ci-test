select
	AppStandardInputOutputSetting.FontId
from
	AppStandardInputOutputSetting
where
	AppStandardInputOutputSetting.Generation = (
		select
			MAX(AppStandardInputOutputSetting.Generation)
		from
			AppStandardInputOutputSetting
	)
