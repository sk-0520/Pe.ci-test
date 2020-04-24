select
	LauncherRedoItems.LauncherItemId,
	LauncherRedoItems.RedoMode,
	LauncherRedoItems.WaitTime,
	LauncherRedoItems.RetryCount
from
	LauncherRedoItems
where
	LauncherRedoItems.LauncherItemId = @LauncherItemId

