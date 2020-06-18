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
        #region define

        public class Extensions
        {
            #region property

            #endregion

            #region function

            public int Func(int a, int b)
            {
                return a + b;
            }

            #endregion
        }

        #endregion
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
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name!;
        }

        public Window CreateWindowWidget(IWidgetAddonCreateContext widgetAddonCreateContext)
        {
            throw new NotSupportedException();
        }

        public IWebViewSeed CreateWebViewWidget(IWidgetAddonCreateContext widgetAddonCreateContext)
        {
            //var webViewSeed = new WebViewSeed(new HtmlAddress(new Uri("https://google.co.jp")));
            var webViewSeed = new WebViewSeed(new HtmlSourceCode(@"<!DOCTYPE html>
<html lang='ja'>
<head>
<meta charset='utf-8'>
    <title></title>
</head>
<body style='background-color: transparent'>
    <div style='margin: 20px; color: red'>
        <div class='pe_move-area' style='background: #b6ff00'>View Move</div>
        <h1>HTML!</h1>
        <span class='pe_resize-area' style='background: #b6ff00'>resize</span>
<ul>
        <li class='pe_resize-area' data-pe_resize='n' style='background: #b6ff00'>n　↑</li>
        <li class='pe_resize-area' data-pe_resize='s' style='background: #b6ff00'>s　↓</li>
        <li class='pe_resize-area' data-pe_resize='e' style='background: #b6ff00'>e　→</li>
        <li class='pe_resize-area' data-pe_resize='w' style='background: #b6ff00'>w　←</li>
        <li class='pe_resize-area' data-pe_resize='ne' style='background: #b6ff00'>ne　↗</li>
        <li class='pe_resize-area' data-pe_resize='nw' style='background: #b6ff00'>nw　↖</li>
        <li class='pe_resize-area' data-pe_resize='se' style='background: #b6ff00'>se　↘</li>
        <li class='pe_resize-area' data-pe_resize='sw' style='background: #b6ff00'>sw　↙</li>
</ul>

    </div>
</body>
</html>
")) {
                Background = Brushes.Transparent,
                WindowStyle = WindowStyle.None,
                Extensions = new Extensions(),
            };

            webViewSeed.SoilCallback = g => {
                Logger.LogInformation("{0}", g);

                //g.ExecuteScriptAsync("alert(window.pe_callbacks)");
                //g.EvaluateScriptAsync("1+1").ContinueWith(t => {
                //    g.ExecuteScriptAsync("alert('" + t.Result.Result + "')");
                //});

                g.EvaluateScriptAsync("pe_extensions.toString()").ContinueWith(tt => {
                    g.ExecuteScriptAsync("alert('" + tt.Result.Result + "')");
                });
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
