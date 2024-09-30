select
	LauncherGroups.LauncherGroupId,
	LauncherGroups.Name as LauncherGroupName,
	LauncherItems.LauncherItemId,
	LauncherItems.Name as LauncherItemName,
	LauncherItems.Kind
from
	LauncherGroups
	inner join
		LauncherGroupItems
		on
		(
			LauncherGroups.LauncherGroupId = LauncherGroupItems.LauncherGroupId
		)
	inner join
		LauncherItems
		on
		(
			LauncherGroupItems.LauncherItemId = LauncherItems.LauncherItemId
		)
order by
	LauncherGroups.Sequence,
	LauncherGroupItems.Sequence
	

--LauncherItems
