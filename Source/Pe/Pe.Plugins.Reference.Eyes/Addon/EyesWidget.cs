using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Reference.Eyes.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Reference.Eyes.Views;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Eyes.Addon
{
    internal class EyesWidget: IWidget
    {
        public EyesWidget(IAddonParameter parameter, IPluginInformation pluginInformations)
        {
            LoggerFactory = parameter.LoggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            AddonExecutor = parameter.AddonExecutor;
            DispatcherWrapper = parameter.DispatcherWrapper;
            SkeletonImplements = parameter.SkeletonImplements;
            PluginInformations = pluginInformations;
        }

        #region property

        private ILoggerFactory LoggerFactory { get; }
        private ILogger Logger { get; }
        private IAddonExecutor AddonExecutor { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }
        private ISkeletonImplements SkeletonImplements { get; }
        private IPluginInformation PluginInformations { get; }

        private EyesWidgetWindow? WidgetView { get; set; }
        private EyesWidgetViewModel? ViewModel { get; set; }

        private EyesBackground? EyesBackground { get; set; }

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

        public IWebViewSeed CreateWebViewWidget(IWidgetAddonCreateContext widgetAddonCreateContext)
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
