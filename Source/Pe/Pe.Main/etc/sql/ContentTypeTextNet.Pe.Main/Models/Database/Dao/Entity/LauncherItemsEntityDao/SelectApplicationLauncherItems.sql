
select
	LauncherItems.LauncherItemId,
	LauncherItems.Name,
	LauncherItems.Kind,
	LauncherItems.IsEnabledCommandLauncher,
	LauncherItems.IconPath,
	LauncherItems.IconIndex,
	LauncherItems.Comment
from
	LauncherItems
where
	LauncherItems.Kind != 'Addon'

