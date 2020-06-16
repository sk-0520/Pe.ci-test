using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using CefSharp;
using CefSharp.Wpf;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Views.Widget;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Widget
{
    /// <summary>
    /// ViewModelだけどあっちゃこっちゃ引き回されて <see cref="IWebViewGrass"/> にもなる忙しい人。
    /// </summary>
    public class WebViewWidgetViewModel: ViewModelBase, IWebViewGrass
    {
        public WebViewWidgetViewModel(WebViewWidgetWindow window, IHtmlSource htmlSource, Action<IWebViewGrass>? widgetCallback, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            WidgetWindow = window;
            WebView = WidgetWindow.webView;
            HtmlSource = htmlSource;
            WidgetCallback = widgetCallback;

            if(WebView.IsLoaded) {
                LoadHtmlSource(HtmlSource);
            } else {
                WebView.Loaded += WebView_Loaded;
            }
        }

        #region proeprty

        WebViewWidgetWindow WidgetWindow { get; }
        IHtmlSource HtmlSource { get; }
        Action<IWebViewGrass>? WidgetCallback { get; }

        #endregion

        #region command

        #endregion

        #region function

        void LoadHtmlSource(IHtmlSource htmlSource)
        {
            Debug.Assert(WebView.IsLoaded);

            switch(htmlSource.HtmlSourceKind) {
                case HtmlSourceKind.Address: {
                        var address = (IHtmlAddress)htmlSource;
                        WebView.Load(address.Address.ToString());
                    }
                    break;

                case HtmlSourceKind.SourceCode: {
                        var sourceCode = (IHtmlSourceCode)htmlSource;
                        if(sourceCode.BaseAddress == null) {
                            WebView.LoadHtml(sourceCode.SourceCode);
                        } else {
                            WebView.LoadHtml(sourceCode.SourceCode, sourceCode.BaseAddress.ToString());
                        }
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            if(WidgetCallback != null) {
                WidgetCallback(this);
            }
        }

        #endregion

        #region IWebViewGrass

        /// <inheritdoc cref="IWebViewGrass.WebView"/>
        public ChromiumWebBrowser WebView { get; }
        object IWebViewGrass.WebView => WebView;

        #endregion

        private void WebView_Loaded(object sender, RoutedEventArgs e)
        {
            WebView.Loaded -= WebView_Loaded;

            LoadHtmlSource(HtmlSource);
        }

    }
}
