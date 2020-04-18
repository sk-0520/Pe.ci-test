select
	AppNotifyLogSetting.IsVisible,
	AppNotifyLogSetting.Position
from
	AppNotifyLogSetting
where
	AppNotifyLogSetting.Generation = (
		select
			MAX(AppNotifyLogSetting.Generation)
		from
			AppNotifyLogSetting
	)
