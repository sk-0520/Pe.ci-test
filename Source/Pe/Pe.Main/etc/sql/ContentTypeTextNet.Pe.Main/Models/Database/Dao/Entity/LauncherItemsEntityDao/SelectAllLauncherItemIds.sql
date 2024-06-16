select
	LauncherItems.LauncherItemId
from
	LauncherItems
order by
	case LauncherItems.Kind
		when 'file' then 10
		when 'store-app' then 20
		when 'addon' then 30
		when 'separator' then 40
		else -1
	end,
	LauncherItems.Name,
	LauncherItems.Code,
	LauncherItems.LauncherItemId
