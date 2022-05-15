update
	LauncherFiles
set
	File                  = @File,
	Option                = @Option,
	WorkDirectory         = @WorkDirectory,
	IsEnabledCustomEnvVar = @IsEnabledCustomEnvVar,
	ShowMode              = @ShowMode,
	IsEnabledStandardIo   = @IsEnabledStandardIo,
	StandardIoEncoding    = @StandardIoEncoding,
	RunAdministrator      = @RunAdministrator,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	LauncherItemId = @LauncherItemId
