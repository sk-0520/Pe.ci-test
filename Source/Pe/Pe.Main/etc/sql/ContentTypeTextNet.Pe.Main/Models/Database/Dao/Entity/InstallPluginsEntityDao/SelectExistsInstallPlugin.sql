
select
	COUNT(1) = 1
from
	InstallPlugins
where
	InstallPlugins.PluginId = @PluginId

