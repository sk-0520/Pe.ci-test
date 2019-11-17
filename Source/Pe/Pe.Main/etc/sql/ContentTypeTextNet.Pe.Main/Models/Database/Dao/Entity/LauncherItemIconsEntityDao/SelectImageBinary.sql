select
	LauncherItemIcons.Image
from
	LauncherItemIcons
where
	LauncherItemIcons.LauncherItemId = @LauncherItemId
	and
	LauncherItemIcons.IconBox = @IconBox
order by
	LauncherItemIcons.Sequence
