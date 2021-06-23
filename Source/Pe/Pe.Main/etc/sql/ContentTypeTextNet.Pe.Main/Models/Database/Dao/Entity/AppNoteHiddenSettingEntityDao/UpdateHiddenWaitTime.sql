update
	AppNoteHiddenSetting
set
	WaitTime              = @WaitTime,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	AppNoteHiddenSetting.Generation = (
		select
			MAX(AppNoteHiddenSetting.Generation)
		from
			AppNoteHiddenSetting
		where
			AppNoteHiddenSetting.HiddenMode = @HiddenMode
	)
