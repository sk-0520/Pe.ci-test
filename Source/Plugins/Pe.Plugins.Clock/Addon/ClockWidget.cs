using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Plugins.Clock.Views;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Clock.Addon
{
    internal class ClockWidget: IWidget
    {
        public ClockWidget(IAddonParameter parameter, IPluginInformations pluginInformations)
        {
            Logger = parameter.LoggerFactory.CreateLogger(GetType());
            AddonExecutor = parameter.AddonExecutor;
            DispatcherWrapper = parameter.DispatcherWrapper;
            PluginInformations = pluginInformations;
        }

        #region property

        ILogger Logger { get; }
        IAddonExecutor AddonExecutor { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        IPluginInformations PluginInformations { get; }

        ClockWidgetWindow? WidgetView { get; set; }

        #endregion

        #region IWidget

        public WidgetViewType ViewType => WidgetViewType.Window;

        public DependencyObject? GetMenuIcon(IPluginContext pluginContext)
        {
            return new TextBlock() {
                Text = DateTime.Now.Hour.ToString()
            };
        }
        public string GetMenuHeader(IPluginContext pluginContext)
        {
            return $"とけい {DateTime.Now.Hour}:{DateTime.Now.Minute}";
        }

        public Window CreateWindowWidget(IWidgetAddonCreateContext widgetAddonCreateContext)
        {
            WidgetView = new ClockWidgetWindow();
            return WidgetView;
        }

        public IHtmlSource CreateWebViewWidget(IWidgetAddonCreateContext widgetAddonCreateContext)
        {
            throw new NotSupportedException();
        }

        public void ClosedWidget(IWidgetAddonClosedContext widgetAddonClosedContext)
        {
            WidgetView = null;
        }

        #endregion

    }
}
