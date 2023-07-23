using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Html.Addon
{
    internal class HtmlWidget: IWidget
    {
        #region define

        private class Extensions
        {
            #region property

            #endregion

            #region function

            public string SampleCallback(string value)
            {
                return value switch {
                    "1" => "おはよう！",
                    "2" => "こんちは！",
                    "3" => "おつかれ！",
                    _ => "💩",
                };
            }

            #endregion
        }

        #endregion
        public HtmlWidget(IAddonParameter parameter, IPluginInformation pluginInformation)
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

        private IWebViewGrass? WebViewGrass { get; set; }

        private Timer? SendTimer { get; set; }

        #endregion

        #region function

        private void OnInitialized(IWebViewGrass webViewGrass)
        {
            WebViewGrass = webViewGrass;

            SendTimer = new Timer(TimeSpan.FromSeconds(10).TotalMilliseconds);
            SendTimer.Elapsed += SendTimer_Elapsed;
            SendTimer.Start();
        }

        #endregion

        #region IWidget

        public WidgetViewType ViewType => WidgetViewType.WebView;

        public DependencyObject? GetMenuIcon(IPluginContext pluginContext)
        {
            return null;
        }

        public string GetMenuHeader(IPluginContext pluginContext)
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name!;
        }

        public Window CreateWindowWidget(IWidgetAddonCreateContext widgetAddonCreateContext)
        {
            throw new NotSupportedException();
        }

        public IWebViewSeed CreateWebViewWidget(IWidgetAddonCreateContext widgetAddonCreateContext)
        {
            var webViewSeed = new WebViewSeed(new HtmlAddress(new Uri("pe://plugin"))) {
                Background = Brushes.Transparent,
                WindowStyle = WindowStyle.None,
                Extensions = new Extensions(),
                SoilCallback = OnInitialized,
            };

            return webViewSeed;
        }

        /// <inheritdoc cref="IWidget.OpeningWidget(IPluginContext)"/>
        public void OpeningWidget(IPluginContext pluginContext)
        {
        }

        /// <inheritdoc cref="IWidget.OpenedWidget(IPluginContext)"/>
        public void OpenedWidget(IPluginContext pluginContext)
        {
        }

        /// <inheritdoc cref="IWidget.ClosedWidget(IWidgetAddonClosedContext)"/>
        public void ClosedWidget(IWidgetAddonClosedContext widgetAddonClosedContext)
        {
            if(SendTimer != null) {
                SendTimer.Stop();
                SendTimer.Dispose();
                SendTimer = null;
            }
        }

        #endregion

        private void SendTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Debug.Assert(SendTimer != null);

            SendTimer.Stop();

            var timestamp = DateTime.Now.ToString("u");
            var memory = GC.GetTotalMemory(false);

            WebViewGrass!.ExecuteScriptAsync("receiveSample", timestamp, memory);

            SendTimer.Start();
        }
    }
}
