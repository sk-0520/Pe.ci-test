select
	PluginVersionChecks.CheckUrl
from
	PluginVersionChecks
where
	PluginVersionChecks.PluginId = @PluginId
order by
	PluginVersionChecks.Sequence
