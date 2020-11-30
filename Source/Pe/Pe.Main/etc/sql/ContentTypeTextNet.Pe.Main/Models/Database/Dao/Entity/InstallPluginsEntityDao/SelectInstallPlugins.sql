select
	InstallPlugins.PluginId,
	InstallPlugins.PluginName,
	InstallPlugins.PluginVersion,
	InstallPlugins.PluginInstallMode,
	InstallPlugins.ExtractedDirectoryPath,
	InstallPlugins.PluginDirectoryPath
from
	InstallPlugins
order by
	InstallPlugins.CreatedTimestamp,
	InstallPlugins.PluginName,
	InstallPlugins.PluginId
