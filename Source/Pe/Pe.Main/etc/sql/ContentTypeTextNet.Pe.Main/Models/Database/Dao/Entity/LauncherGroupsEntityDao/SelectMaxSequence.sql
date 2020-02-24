select
	IFNULL(MAX(LauncherGroups.Sequence), 0)
from
	LauncherGroups
