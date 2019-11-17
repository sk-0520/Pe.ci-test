select
	IFNULL(MAX(LauncherGroupItems.Sequence), 0)
from
	LauncherGroupItems
where
	LauncherGroupItems.LauncherGroupId = @LauncherGroupId
