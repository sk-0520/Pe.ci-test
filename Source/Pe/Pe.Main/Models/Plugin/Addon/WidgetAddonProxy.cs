using System;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public sealed class WidgetAddonProxy: AddonProxyBase<IWidget>, IWidget
    {
        public WidgetAddonProxy(IAddon addon, PluginContextFactory pluginContextFactory, IHttpUserAgentFactory userAgentFactory, IViewManager viewManager, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(addon, pluginContextFactory, userAgentFactory, viewManager, platformTheme, imageLoader, mediaConverter, policy, dispatcherWrapper, loggerFactory)
        { }

        #region AddonProxyBase

        protected override AddonKind AddonKind => AddonKind.Widget;

        protected override IWidget BuildFunctionUnit(IAddon loadedAddon)
        {
            return loadedAddon.BuildWidget(CreateParameter(Addon));
        }

        #endregion

        #region IWidget

        public WidgetViewType ViewType => FunctionUnit.ViewType;


        /// <inheritdoc cref="IWidget.GetMenuIcon(IPluginContext)"/>
        public DependencyObject? GetMenuIcon(IPluginContext pluginContext)
        {
            return FunctionUnit.GetMenuIcon(pluginContext);
        }

        /// <inheritdoc cref="IWidget.GetMenuHeader(IPluginContext)"/>
        public string GetMenuHeader(IPluginContext pluginContext)
        {
            return FunctionUnit.GetMenuHeader(pluginContext);
        }

        /// <inheritdoc cref="IWidget.CreateWindowWidget(IWidgetAddonCreateContext)"/>
        public Window CreateWindowWidget(IWidgetAddonCreateContext widgetAddonCreateContext)
        {
            if(ViewType == WidgetViewType.Window) {
                return FunctionUnit.CreateWindowWidget(widgetAddonCreateContext);
            }

            throw new NotSupportedException();
        }

        /// <inheritdoc cref="IWidget.OpeningWidget(IPluginContext)"/>
        public void OpeningWidget(IPluginContext pluginContext)
        {
            FunctionUnit.OpeningWidget(pluginContext);
        }

        /// <inheritdoc cref="IWidget.OpenedWidget(IPluginContext)"/>
        public void OpenedWidget(IPluginContext pluginContext)
        {
            FunctionUnit.OpenedWidget(pluginContext);
        }

        /// <inheritdoc cref="IWidget.ClosedWidget(IWidgetAddonClosedContext)"/>
        public void ClosedWidget(IWidgetAddonClosedContext widgetAddonClosedContext)
        {
            FunctionUnit.ClosedWidget(widgetAddonClosedContext);
        }

        #endregion
    }
}
