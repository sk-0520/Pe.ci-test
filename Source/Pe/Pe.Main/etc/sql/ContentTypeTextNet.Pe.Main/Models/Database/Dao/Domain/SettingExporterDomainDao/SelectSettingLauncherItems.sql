select
	LauncherItems.LauncherItemId,
	LauncherItems.Name as LauncherItemName,
	LauncherItems.Kind as LauncherItemKind,
	LauncherItems.IconPath,
	LauncherItems.IconIndex,
	case
		when LauncherFiles.File is null then ''
		else LauncherFiles.File
	end as FilePath,
	case
		when LauncherFiles.Option is null then ''
		else LauncherFiles.Option
	end as FileOption,
	case
		when LauncherFiles.WorkDirectory is null then ''
		else LauncherFiles.WorkDirectory
	end as FileWorkDirectory
from
	LauncherItems
	left join
		LauncherFiles
		on
		(
			LauncherItems.LauncherItemId = LauncherFiles.LauncherItemId
		)
order by
	case LauncherItems.Kind
		when 'file' then 10
		when 'store-app' then 20
		when 'addon' then 30
		when 'separator' then 40
		else -1
	end,
	LauncherItems.Name,
	LauncherItems.LauncherItemId

