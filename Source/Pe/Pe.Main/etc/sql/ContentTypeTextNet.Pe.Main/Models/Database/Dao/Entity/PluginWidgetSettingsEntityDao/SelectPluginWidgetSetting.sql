select
	PluginWidgetSettings.X,
	PluginWidgetSettings.Y,
	PluginWidgetSettings.IsVisible
from
	PluginWidgetSettings
where
	PluginId = @PluginId
