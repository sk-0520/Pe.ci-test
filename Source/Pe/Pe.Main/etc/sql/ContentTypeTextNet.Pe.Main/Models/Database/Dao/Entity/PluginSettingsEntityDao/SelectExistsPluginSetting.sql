select
	COUNT(1) = 1
from
	PluginSettings
where
	PluginSettings.PluginId = @PluginId
	and
	PluginSettings.PluginSettingKey = @PluginSettingKey
