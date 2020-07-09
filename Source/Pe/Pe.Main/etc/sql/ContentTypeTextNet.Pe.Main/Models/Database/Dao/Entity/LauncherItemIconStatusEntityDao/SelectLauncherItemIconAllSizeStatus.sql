select
	LauncherItemIconStatus.IconBox,
	LauncherItemIconStatus.IconScale,
	LauncherItemIconStatus.LastUpdatedTimestamp
from
	LauncherItemIconStatus
where
	LauncherItemIconStatus.LauncherItemId = @LauncherItemId
