select
	LauncherGroupItems.LauncherItemId
from
	LauncherGroupItems
where
	LauncherGroupItems.LauncherGroupId = @LauncherGroupId
order by
	LauncherGroupItems.Sequence
