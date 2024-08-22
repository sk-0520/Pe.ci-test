select
	LauncherItemIconStatus.IconBox,
	LauncherItemIconStatus.LastUpdatedTimestamp
from
	LauncherItemIconStatus
where
	LauncherItemIconStatus.LauncherItemId = @LauncherItemId
	and
	LauncherItemIconStatus.IconBox = @IconBox
	and
	LauncherItemIconStatus.IconScale = @IconScale
