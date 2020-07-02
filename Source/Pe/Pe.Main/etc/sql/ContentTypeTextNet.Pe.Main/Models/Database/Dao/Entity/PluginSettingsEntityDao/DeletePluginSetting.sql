delete
from
	PluginSettings
where
	PluginSettings.PluginId = @PluginId
	and
	PluginSettings.PluginSettingKey = @PluginSettingKey
