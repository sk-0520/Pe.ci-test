select
	LauncherItemHistories.Kind,
	LauncherItemHistories.Value,
	LauncherItemHistories.LastExecuteTimestamp
from
	LauncherItemHistories
where
	LauncherItemHistories.LauncherItemId = @LauncherItemId
order by
	LauncherItemHistories.LastExecuteTimestamp desc
