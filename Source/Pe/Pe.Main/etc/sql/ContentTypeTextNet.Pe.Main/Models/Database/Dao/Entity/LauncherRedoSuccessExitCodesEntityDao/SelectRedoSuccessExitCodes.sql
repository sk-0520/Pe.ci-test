select distinct
	LauncherRedoSuccessExitCodes.SuccessExitCode
from
	LauncherRedoSuccessExitCodes
where
	LauncherRedoSuccessExitCodes.LauncherItemId = @LauncherItemId
order by
	LauncherRedoSuccessExitCodes.SuccessExitCode
