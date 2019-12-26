select
	AppNoteSetting.FontId,
	AppNoteSetting.TitleKind,
	AppNoteSetting.LayoutKind,
	-- TODO: カラム名変更
	AppNoteSetting.Foreground as ForegroundColor,
	AppNoteSetting.Background as BackgroundColor,
	AppNoteSetting.IsTopmost
from
	AppNoteSetting
