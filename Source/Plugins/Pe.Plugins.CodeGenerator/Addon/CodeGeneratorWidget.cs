using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.CodeGenerator.Addon
{
    internal class CodeGeneratorWidget: IWidget
    {
        public CodeGeneratorWidget(IAddonParameter parameter, IPluginInformations pluginInformations)
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

        #endregion

        #region function

        #endregion

        #region IWidget

        public WidgetViewType ViewType => WidgetViewType.WebView;

        public DependencyObject? GetMenuIcon(IPluginContext pluginContext)
        {
            return null;
        }

        public string GetMenuHeader(IPluginContext pluginContext)
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().FullName;
        }

        public Window CreateWindowWidget(IWidgetAddonCreateContext widgetAddonCreateContext)
        {
            throw new NotSupportedException();
        }

        public IWebViewSeed CreateWebViewWidget(IWidgetAddonCreateContext widgetAddonCreateContext)
        {
            //var webViewSeed = new WebViewSeed(new HtmlAddress(new Uri("https://google.co.jp")));
            var webViewSeed = new WebViewSeed(new HtmlSourceCode(@"
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title></title>
</head>
<body style='background-color: transparent'>
    <div style='margin: 20px; color: red'>
        <div class='PE:MOVE-AREA' style='background: #b6ff00'>View Move</div>
        <h1>HTML!</h1>
        <span class='PE:RESIZE-AREA' style='background: #b6ff00'>resize</span>
    </div>
</body>
</html>
")) {
                Background = Brushes.Transparent,
                WindowStyle = WindowStyle.None,
            };

            webViewSeed.SoilCallback = g => {
                Logger.LogInformation("{0}", g);
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
        }

        #endregion
    }
}
