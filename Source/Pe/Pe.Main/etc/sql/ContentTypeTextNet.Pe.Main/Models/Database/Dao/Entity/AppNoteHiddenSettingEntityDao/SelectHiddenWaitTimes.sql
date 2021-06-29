select
	AppNoteHiddenSetting.HiddenMode,
	AppNoteHiddenSetting.WaitTime
from
	AppNoteHiddenSetting
where
	AppNoteHiddenSetting.Generation in (
		select
			MAX(AppNoteHiddenSetting.Generation)
		from
			AppNoteHiddenSetting
		group by
			AppNoteHiddenSetting.HiddenMode
	)
