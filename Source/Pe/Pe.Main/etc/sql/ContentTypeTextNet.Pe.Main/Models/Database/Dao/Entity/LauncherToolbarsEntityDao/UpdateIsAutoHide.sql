update
	LauncherToolbars
set
	IsAutoHide = @IsAutoHide,

	UpdatedTimestamp = @UpdatedTimestamp,
	UpdatedAccount = @UpdatedAccount,
	UpdatedProgramName = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount = UpdatedCount + 1
where
	LauncherToolbarId = @LauncherToolbarId
	and
	IsAutoHide != @IsAutoHide
