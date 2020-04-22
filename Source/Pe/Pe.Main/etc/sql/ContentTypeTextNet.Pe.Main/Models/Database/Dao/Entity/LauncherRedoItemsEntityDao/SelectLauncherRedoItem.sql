select
	LauncherRedoItems.LauncherItemId,
	LauncherRedoItems.RedoWait,
	LauncherRedoItems.WaitTime,
	LauncherRedoItems.RetryCount
from
	LauncherRedoItems
where
	LauncherRedoItems.LauncherItemId = @LauncherItemId

