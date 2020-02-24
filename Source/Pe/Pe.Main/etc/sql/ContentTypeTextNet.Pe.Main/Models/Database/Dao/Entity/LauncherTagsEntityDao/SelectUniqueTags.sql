select
	distinct TRIM(LauncherTags.TagName)
from
	LauncherTags
where
	LauncherTags.LauncherItemId = @LauncherItemId
	and
	0 < LENGTH(TRIM(LauncherTags.TagName))
order by
	LauncherTags.TagName

