using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Clock.Models.Data;
using ContentTypeTextNet.Pe.Plugins.Clock.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Clock.Views;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Clock.Addon
{
    internal class ClockWidget: IWidget
    {
        public ClockWidget(IAddonParameter parameter, IPluginInformations pluginInformations)
        {
            LoggerFactory = parameter.LoggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            AddonExecutor = parameter.AddonExecutor;
            DispatcherWrapper = parameter.DispatcherWrapper;
            SkeletonImplements = parameter.SkeletonImplements;
            PluginInformations = pluginInformations;
        }

        #region property

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }
        IAddonExecutor AddonExecutor { get; }
        IDispatcherWrapper DispatcherWrapper { get; }
        ISkeletonImplements SkeletonImplements { get; }
        IPluginInformations PluginInformations { get; }

        ClockWidgetWindow? WidgetView { get; set; }
        ClockWidgetViewModel? ViewModel { get; set; }

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
            if(ViewModel != null) {
                throw new InvalidOperationException(nameof(ViewModel));
            }

            ClockWidgetSetting? clockWidgetSetting;
            if(!widgetAddonCreateContext.Storage.Persistent.Normal.TryGet<ClockWidgetSetting>(ClockConstants.WidgetSettengKey, out clockWidgetSetting)) {
                clockWidgetSetting = new ClockWidgetSetting();
            }
            ViewModel = new ClockWidgetViewModel(clockWidgetSetting, SkeletonImplements, LoggerFactory);
            WidgetView = new ClockWidgetWindow() {
                DataContext = ViewModel,
            };
            return WidgetView;
        }

        public IHtmlSource CreateWebViewWidget(IWidgetAddonCreateContext widgetAddonCreateContext)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc cref="IWidget.OpeningWidget(IPluginContext)"/>
        public void OpeningWidget(IPluginContext pluginContext)
        {
            if(ViewModel == null) {
                throw new InvalidOperationException(nameof(ViewModel));
            }
            ViewModel.StartClock();
        }

        /// <inheritdoc cref="IWidget.OpenedWidget(IPluginContext)"/>
        public void OpenedWidget(IPluginContext pluginContext)
        {
            if(ViewModel == null) {
                throw new InvalidOperationException(nameof(ViewModel));
            }
        }

        /// <inheritdoc cref="IWidget.ClosedWidget(IWidgetAddonClosedContext)"/>
        public void ClosedWidget(IWidgetAddonClosedContext widgetAddonClosedContext)
        {
            if(ViewModel == null) {
                throw new InvalidOperationException(nameof(ViewModel));
            }
            if(WidgetView == null) {
                throw new InvalidOperationException(nameof(WidgetView));
            }

            ViewModel.StopClock();
            ViewModel.Dispose();
            ViewModel = null;
            WidgetView = null;
        }

        #endregion

    }
}
