select
	distinct
	LauncherTags.LauncherItemId,
	TRIM(LauncherTags.TagName)
from
	LauncherTags
where
	0 < LENGTH(TRIM(LauncherTags.TagName))
order by
	LauncherTags.TagName

