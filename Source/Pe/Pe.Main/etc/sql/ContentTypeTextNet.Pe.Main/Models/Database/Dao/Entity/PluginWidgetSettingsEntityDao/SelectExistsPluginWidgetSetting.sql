select
	COUNT(1) = 1
from
	PluginWidgetSettings
where
	PluginWidgetSettings.PluginId = @PluginId
