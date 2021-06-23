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
		where
			AppNoteHiddenSetting.HiddenMode = @HiddenMode
	)
