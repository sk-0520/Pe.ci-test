
select
	LauncherItems.LauncherItemId,
	LauncherItems.Name,
	LauncherItems.Code,
	LauncherItems.Kind,
	LauncherItems.IsEnabledCommandLauncher,
	LauncherItems.Comment
from
	LauncherItems
where
	LauncherItems.LauncherItemId = @LauncherItemId

