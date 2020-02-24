select
	LauncherStoreApps.ProtocolAlias,
	LauncherStoreApps.Option
from
	LauncherStoreApps
order by
	LauncherStoreApps.LauncherItemId = @LauncherItemId
