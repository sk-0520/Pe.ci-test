update
	LauncherItems
set
	[Code] = @Code,
	[Name] = @Name,
	[IconPath] = @IconPath,
	[IconIndex] = @IconIndex,
	[IsEnabledCommandLauncher] = @IsEnabledCommandLauncher,
	[Comment] = @Comment,

	UpdatedTimestamp = @UpdatedTimestamp,
	UpdatedAccount = @UpdatedAccount,
	UpdatedProgramName = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount = UpdatedCount + 1
where
	LauncherItemId = @LauncherItemId


