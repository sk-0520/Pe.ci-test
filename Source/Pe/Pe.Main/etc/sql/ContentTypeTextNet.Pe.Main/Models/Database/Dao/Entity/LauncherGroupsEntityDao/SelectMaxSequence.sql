select
	COALESCE(MAX(LauncherGroups.Sequence), 0)
from
	LauncherGroups
