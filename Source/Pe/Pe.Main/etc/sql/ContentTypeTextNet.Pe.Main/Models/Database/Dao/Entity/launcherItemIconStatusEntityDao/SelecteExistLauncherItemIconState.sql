select
	COUNT(1) != 0
from
	LauncherItemIconStatus
where
	LauncherItemIconStatus.LauncherItemId = @LauncherItemId
	and
	LauncherItemIconStatus.IconBox = @IconBox
