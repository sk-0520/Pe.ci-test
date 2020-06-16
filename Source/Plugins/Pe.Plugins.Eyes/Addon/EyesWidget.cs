using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Eyes.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Eyes.Views;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Eyes.Addon
{
    internal class EyesWidget: IWidget
    {
        public EyesWidget(IAddonParameter parameter, IPluginInformations pluginInformations)
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

        EyesWidgetWindow? WidgetView { get; set; }
        EyesWidgetViewModel? ViewModel { get; set; }

        EyesBackground? EyesBackground { get; set; }

        #endregion

        #region function

        internal void Attach(EyesBackground eyesBackground)
        {
            EyesBackground = eyesBackground;
            if(ViewModel != null) {
                ViewModel.Attach(EyesBackground);
            }
        }

        #endregion

        #region IWidget

        public WidgetViewType ViewType => WidgetViewType.Window;

        public DependencyObject? GetMenuIcon(IPluginContext pluginContext)
        {
            return null;
        }

        public string GetMenuHeader(IPluginContext pluginContext)
        {
            return "👀";
        }

        public Window CreateWindowWidget(IWidgetAddonCreateContext widgetAddonCreateContext)
        {
            if(ViewModel != null) {
                throw new InvalidOperationException(nameof(ViewModel));
            }

            WidgetView = new EyesWidgetWindow();
            ViewModel = new EyesWidgetViewModel(WidgetView, SkeletonImplements, DispatcherWrapper, LoggerFactory);
            if(EyesBackground != null) {
                Attach(EyesBackground);
            }
            WidgetView.DataContext = ViewModel;

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
            //ViewModel.StartClock();
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

            //ViewModel.StopClock();
            ViewModel.Dispose();
            ViewModel = null;
            WidgetView = null;
        }

        #endregion
    }
}
