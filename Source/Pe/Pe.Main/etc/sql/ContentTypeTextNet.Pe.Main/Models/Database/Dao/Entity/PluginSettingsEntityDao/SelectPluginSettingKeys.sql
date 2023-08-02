select
	PluginSettings.PluginSettingKey
from
	PluginSettings
where
	PluginSettings.PluginId = @PluginId
order by
	PluginSettings.PluginSettingKey
