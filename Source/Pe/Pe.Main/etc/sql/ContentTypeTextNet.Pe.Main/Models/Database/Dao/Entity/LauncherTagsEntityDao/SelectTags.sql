select
	LauncherTags.TagName
from
	LauncherTags
where
	LauncherTags.LauncherItemId = @LauncherItemId
order by
	LauncherTags.TagName

