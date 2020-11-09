select
	Plugins.PluginId,
	Plugins.Name,
	Plugins.State
from
	Plugins
where
	Plugins.PluginId = @PluginId
