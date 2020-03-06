select
	LauncherEnvVars.EnvName,
	LauncherEnvVars.EnvValue
from
	LauncherEnvVars
where
	LauncherEnvVars.LauncherItemId = @LauncherItemId

