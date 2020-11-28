select
	InstallPlugins.ExtractedDirectoryPath
from
	InstallPlugins
where
	InstallPlugins.PluginId = @PluginId
