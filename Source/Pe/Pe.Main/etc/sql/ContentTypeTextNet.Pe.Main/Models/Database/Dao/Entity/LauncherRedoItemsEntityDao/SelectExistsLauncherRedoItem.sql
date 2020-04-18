
select
	COUNT(1) = 1
from
	LauncherRedoItems
where
	LauncherRedoItems.LauncherItemId = @LauncherItemId

