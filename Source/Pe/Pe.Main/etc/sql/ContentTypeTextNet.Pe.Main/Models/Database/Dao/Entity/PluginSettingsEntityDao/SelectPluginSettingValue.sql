select
	PluginSettings.DataType,
	PluginSettings.DataValue
from
	PluginSettings
where
	PluginSettings.PluginId = @PluginId
	and
	PluginSettings.PluginSettingKey = @PluginSettingKey
