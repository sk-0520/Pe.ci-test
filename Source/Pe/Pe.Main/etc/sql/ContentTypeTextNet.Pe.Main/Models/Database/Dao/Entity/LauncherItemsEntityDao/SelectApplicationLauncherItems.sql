
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
-- こいつの存在が良く分からん（元の where 動いてないし、名前が分からんし、なんもわからん）
--where
--	LauncherItems.Kind != 'Addon'
