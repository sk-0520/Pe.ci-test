select
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
	and
	AppNoteHiddenSetting.HiddenMode = @HiddenMode
