select
	COUNT(1) = 1
from
	PluginLauncherItemSettings
where
	PluginLauncherItemSettings.PluginId = @PluginId
	and
	PluginLauncherItemSettings.LauncherItemId = @LauncherItemId
	and
	PluginLauncherItemSettings.PluginSettingKey = @PluginSettingKey
