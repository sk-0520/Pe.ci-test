select
	COUNT(1) = 1
from
	Plugins
where
	Plugins.PluginId = @PluginId
