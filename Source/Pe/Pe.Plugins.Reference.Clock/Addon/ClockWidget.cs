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
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Models;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Models.Data;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Views;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.Addon
{
    internal class ClockWidget: IWidget
    {
        public ClockWidget(IAddonParameter parameter, IPluginInformation pluginInformation)
        {
            LoggerFactory = parameter.LoggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            AddonExecutor = parameter.AddonExecutor;
            DispatcherWrapper = parameter.DispatcherWrapper;
            SkeletonImplements = parameter.SkeletonImplements;
            PluginInformation = pluginInformation;
        }

        #region property

        private ILoggerFactory LoggerFactory { get; }
        private ILogger Logger { get; }
        private IAddonExecutor AddonExecutor { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }
        private ISkeletonImplements SkeletonImplements { get; }
        private IPluginInformation PluginInformation { get; }
        private ClockWidgetWindow? WidgetView { get; set; }
        private ClockWidgetViewModel? ViewModel { get; set; }

        #endregion

        #region IWidget

        public WidgetViewType ViewType => WidgetViewType.Window;

        public DependencyObject? GetMenuIcon(IPluginContext pluginContext)
        {
            return new Viewbox() {
                Stretch = System.Windows.Media.Stretch.Fill,
                StretchDirection = StretchDirection.Both,
                Child = new TextBlock() {
                    Text = ClockUtility.GetClockEmoji(DateTime.Now)
                }
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
            if(!widgetAddonCreateContext.Storage.Persistence.Normal.TryGet<ClockWidgetSetting>(ClockConstants.WidgetSettengKey, out clockWidgetSetting)) {
                clockWidgetSetting = new ClockWidgetSetting();
            }
            ViewModel = new ClockWidgetViewModel(clockWidgetSetting, SkeletonImplements, DispatcherWrapper, LoggerFactory);
            WidgetView = new ClockWidgetWindow() {
                DataContext = ViewModel,
            };
            return WidgetView;
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
