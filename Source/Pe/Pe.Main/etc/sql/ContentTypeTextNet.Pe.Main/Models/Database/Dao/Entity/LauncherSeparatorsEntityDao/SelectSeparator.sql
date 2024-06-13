select
	LauncherSeparators.Kind,
	LauncherSeparators.Width
from
	LauncherSeparators
where
	LauncherSeparators.LauncherItemId = @LauncherItemId
