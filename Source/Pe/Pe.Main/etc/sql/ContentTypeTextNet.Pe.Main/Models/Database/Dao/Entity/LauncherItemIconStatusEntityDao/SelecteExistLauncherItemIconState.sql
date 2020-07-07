select
	COUNT(1) == 1
from
	LauncherItemIconStatus
where
	LauncherItemIconStatus.LauncherItemId = @LauncherItemId
	and
	LauncherItemIconStatus.IconBox = @IconBox
	and
	LauncherItemIconStatus.IconScale = @IconScale
