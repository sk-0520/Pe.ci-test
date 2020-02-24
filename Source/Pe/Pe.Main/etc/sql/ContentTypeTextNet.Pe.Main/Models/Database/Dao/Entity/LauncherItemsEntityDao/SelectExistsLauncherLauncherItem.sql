
select
	COUNT(1) = 1
from
	LauncherItems
where
	LauncherItems.LauncherItemId = @LauncherItemId

