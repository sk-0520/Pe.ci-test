
select
	LauncherItems.LauncherItemId,
	LauncherItems.Name,
	LauncherItems.Code,
	LauncherItems.Kind,
	LauncherItems.IsEnabledCommandLauncher,
	LauncherItems.Note
from
	LauncherItems
where
	LauncherItems.LauncherItemId = @LauncherItemId

