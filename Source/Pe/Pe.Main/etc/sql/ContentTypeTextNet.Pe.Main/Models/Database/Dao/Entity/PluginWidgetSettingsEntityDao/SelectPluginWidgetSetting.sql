select
	PluginWidgetSettings.X,
	PluginWidgetSettings.Y,
	PluginWidgetSettings.Width,
	PluginWidgetSettings.Height,
	PluginWidgetSettings.IsVisible
from
	PluginWidgetSettings
where
	PluginWidgetSettings.PluginId = @PluginId
