select
	AppNoteSetting.FontId,
	AppNoteSetting.TitleKind,
	AppNoteSetting.LayoutKind,
	AppNoteSetting.ForegroundColor,
	AppNoteSetting.BackgroundColor,
	AppNoteSetting.IsTopmost,
	AppNoteSetting.CaptionPosition
from
	AppNoteSetting
where
	AppNoteSetting.Generation = (
		select
			MAX(AppNoteSetting.Generation)
		from
			AppNoteSetting
	)
