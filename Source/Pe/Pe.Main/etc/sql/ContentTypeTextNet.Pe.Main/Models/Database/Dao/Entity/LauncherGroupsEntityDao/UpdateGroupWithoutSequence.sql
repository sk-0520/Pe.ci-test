update
	LauncherGroups
set
	Name       = @Name,
	ImageName  = @ImageName,
	ImageColor = @ImageColor,
	UpdatedTimestamp       = @UpdatedTimestamp,
	UpdatedAccount         = @UpdatedAccount,
	UpdatedProgramName     = @UpdatedProgramName,
	UpdatedProgramVersion  = @UpdatedProgramVersion,
	UpdatedCount           = UpdatedCount + 1
where
	LauncherGroupId = @LauncherGroupId
