select
	PluginLauncherItemSettings.PluginSettingKey
from
	PluginLauncherItemSettings
where
	PluginLauncherItemSettings.PluginId = @PluginId
	and
	PluginLauncherItemSettings.LauncherItemId = @LauncherItemId
order by
	PluginLauncherItemSettings.PluginSettingKey
