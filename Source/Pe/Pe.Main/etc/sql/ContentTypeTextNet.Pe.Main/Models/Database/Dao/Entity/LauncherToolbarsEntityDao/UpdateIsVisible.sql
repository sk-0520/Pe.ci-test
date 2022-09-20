update
	LauncherToolbars
set
	IsVisible = @IsVisible,

	UpdatedTimestamp = @UpdatedTimestamp,
	UpdatedAccount = @UpdatedAccount,
	UpdatedProgramName = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount = UpdatedCount + 1
where
	LauncherToolbarId = @LauncherToolbarId
	and
	IsVisible != @IsVisible
