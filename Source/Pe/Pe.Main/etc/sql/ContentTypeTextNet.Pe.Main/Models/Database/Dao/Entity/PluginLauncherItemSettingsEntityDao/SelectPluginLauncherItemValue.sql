select
	PluginLauncherItemSettings.DataType,
	PluginLauncherItemSettings.DataValue
from
	PluginLauncherItemSettings
where
	PluginLauncherItemSettings.PluginId = @PluginId
	and
	PluginLauncherItemSettings.LauncherItemId = @LauncherItemId
	and
	PluginLauncherItemSettings.PluginSettingKey = @PluginSettingKey
