select
	LauncherItemIcons.IconBox,
	LauncherItemIcons.IconScale,
	LauncherItemIcons.LastUpdatedTimestamp
from
	LauncherItemIcons
where
	LauncherItemIcons.LauncherItemId = @LauncherItemId
