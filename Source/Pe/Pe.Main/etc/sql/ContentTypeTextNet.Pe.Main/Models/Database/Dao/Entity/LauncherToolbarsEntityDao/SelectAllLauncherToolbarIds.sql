select
	LauncherToolbars.LauncherToolbarId
from
	LauncherToolbars
order by
	LauncherToolbars.UpdatedTimestamp desc,
	LauncherToolbars.UpdatedCount desc
