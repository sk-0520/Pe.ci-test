select
	LauncherGroups.LauncherGroupId,
	LauncherGroups.Name,
	LauncherGroups.ImageName,
	LauncherGroups.ImageColor,
	LauncherGroups.Sequence
from
	LauncherGroups
where
	LauncherGroups.LauncherGroupId = @LauncherGroupId
