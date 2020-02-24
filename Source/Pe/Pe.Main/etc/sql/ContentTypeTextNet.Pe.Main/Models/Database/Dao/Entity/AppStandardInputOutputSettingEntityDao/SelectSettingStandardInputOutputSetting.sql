select
	AppStandardInputOutputSetting.FontId,
	AppStandardInputOutputSetting.OutputForeground as OutputForegroundColor,
	AppStandardInputOutputSetting.OutputBackground as OutputBackgroundColor,
	AppStandardInputOutputSetting.ErrorForeground  as ErrorForegroundColor,
	AppStandardInputOutputSetting.ErrorBackground  as ErrorBackgroundColor,
	AppStandardInputOutputSetting.IsTopmost
from
	AppStandardInputOutputSetting
where
	AppStandardInputOutputSetting.Generation = (
		select
			MAX(AppStandardInputOutputSetting.Generation)
		from
			AppStandardInputOutputSetting
	)
