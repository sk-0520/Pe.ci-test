update
	LauncherToolbars
set
	IsTopmost = @IsTopmost,

	UpdatedTimestamp = @UpdatedTimestamp,
	UpdatedAccount = @UpdatedAccount,
	UpdatedProgramName = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount = UpdatedCount + 1
where
	LauncherToolbarId = @LauncherToolbarId
	and
	IsTopmost != @IsTopmost
