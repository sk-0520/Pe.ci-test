select distinct
	LauncherTags.LauncherItemId,
	TRIM(LauncherTags.TagName) as TagName
from
	LauncherTags
where
	0 < LENGTH(TRIM(LauncherTags.TagName))
order by
	TRIM(LauncherTags.TagName)
