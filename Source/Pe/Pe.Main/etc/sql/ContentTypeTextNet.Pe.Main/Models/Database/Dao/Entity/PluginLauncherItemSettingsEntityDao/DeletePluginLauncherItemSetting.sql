delete
from
	PluginLauncherItemSettings
where
	PluginLauncherItemSettings.PluginId = @PluginId
	and
	PluginLauncherItemSettings.LauncherItemId = @LauncherItemId
	and
	PluginLauncherItemSettings.PluginSettingKey = @PluginSettingKey
