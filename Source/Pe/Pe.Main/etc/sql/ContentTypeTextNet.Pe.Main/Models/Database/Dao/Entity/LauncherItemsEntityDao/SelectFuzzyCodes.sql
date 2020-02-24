select
	LauncherItems.Code
from
	LauncherItems
where
	LauncherItems.Code like @BaseCode || '%'

