select
	AppNoteSetting.FontId
from
	AppNoteSetting
where
	AppNoteSetting.Generation = (
		select
			MAX(AppNoteSetting.Generation)
		from
			AppNoteSetting
	)
