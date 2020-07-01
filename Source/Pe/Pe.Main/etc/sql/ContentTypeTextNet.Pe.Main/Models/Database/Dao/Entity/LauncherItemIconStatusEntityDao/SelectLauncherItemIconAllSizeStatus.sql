select
	LauncherItemIconStatus.IconBox,
	LauncherItemIconStatus.LastUpdatedTimestamp
from
	LauncherItemIconStatus
where
	LauncherItemIconStatus.LauncherItemId = @LauncherItemId
