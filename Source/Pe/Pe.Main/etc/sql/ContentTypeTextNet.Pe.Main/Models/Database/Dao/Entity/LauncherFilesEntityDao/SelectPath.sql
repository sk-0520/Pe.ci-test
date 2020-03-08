select
	LauncherFiles.File,
	LauncherFiles.Option,
	LauncherFiles.WorkDirectory
from
	LauncherFiles
where
	LauncherFiles.LauncherItemId = @LauncherItemId

