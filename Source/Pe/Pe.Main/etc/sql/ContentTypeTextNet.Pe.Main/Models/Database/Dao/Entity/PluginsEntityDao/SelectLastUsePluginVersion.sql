select
	Plugins.LastUsePluginVersion
from
	Plugins
where
	Plugins.PluginId = @PluginId

