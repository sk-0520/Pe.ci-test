select
	AppNoteHiddenSetting.HiddenMode,
	AppNoteHiddenSetting.WaitTime
from
	AppNoteHiddenSetting
where
	AppNoteHiddenSetting.Generation = (
		select
			MAX(AppNoteHiddenSetting.Generation)
		from
			AppNoteHiddenSetting
	)
