update
	LauncherToolbars
set
	PositionKind = @PositionKind,

	UpdatedTimestamp = @UpdatedTimestamp,
	UpdatedAccount = @UpdatedAccount,
	UpdatedProgramName = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount = UpdatedCount + 1
where
	LauncherToolbarId = @LauncherToolbarId
	and
	PositionKind <> @PositionKind
