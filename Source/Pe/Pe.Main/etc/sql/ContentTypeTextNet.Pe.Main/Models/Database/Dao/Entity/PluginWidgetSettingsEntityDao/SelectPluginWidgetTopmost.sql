select
	PluginWidgetSettings.IsTopmost
from
	PluginWidgetSettings
where
	PluginWidgetSettings.PluginId = @PluginId
