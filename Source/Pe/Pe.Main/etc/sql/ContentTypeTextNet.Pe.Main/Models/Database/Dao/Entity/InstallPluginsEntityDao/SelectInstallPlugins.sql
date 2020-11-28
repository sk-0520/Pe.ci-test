select
	InstallPlugins.PluginId,
	InstallPlugins.PluginName,
	InstallPlugins.PluginVersion,
	InstallPlugins.PluginInstallMode
from
	InstallPlugins
order by
	InstallPlugins.CreatedTimestamp,
	InstallPlugins.PluginName,
	InstallPlugins.PluginId
