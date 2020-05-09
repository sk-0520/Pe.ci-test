
select
	LauncherItems.Kind,
	IFNULL(LauncherFiles.[File], '') as FilePath,
	0 as CommandIndex,
	LauncherItems.IconPath,
	LauncherItems.IconIndex
from
	LauncherItems
	left join
		LauncherFiles
		on
		(
			LauncherItems.LauncherItemId = LauncherFiles.LauncherItemId
		)
where
	LauncherItems.LauncherItemId = @LauncherItemId

