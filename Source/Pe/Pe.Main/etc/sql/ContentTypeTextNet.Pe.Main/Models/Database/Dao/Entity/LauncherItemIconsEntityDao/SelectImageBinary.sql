select
	LauncherItemIcons.Image
from
	LauncherItemIcons
where
	LauncherItemIcons.LauncherItemId = @LauncherItemId
	and
	LauncherItemIcons.IconBox = @IconBox
	and
	LauncherItemIcons.IconScale = @IconScale
order by
	LauncherItemIcons.Sequence
