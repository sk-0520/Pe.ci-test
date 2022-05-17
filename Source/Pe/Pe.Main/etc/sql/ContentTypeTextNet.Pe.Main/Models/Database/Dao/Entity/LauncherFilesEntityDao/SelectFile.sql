select
	LauncherFiles.File,
	LauncherFiles.Option,
	LauncherFiles.WorkDirectory,
	LauncherFiles.ShowMode,
	LauncherFiles.IsEnabledCustomEnvVar,
	LauncherFiles.IsEnabledStandardIo,
	LauncherFiles.StandardIoEncoding,
	LauncherFiles.RunAdministrator
from
	LauncherFiles
where
	LauncherFiles.LauncherItemId = @LauncherItemId

