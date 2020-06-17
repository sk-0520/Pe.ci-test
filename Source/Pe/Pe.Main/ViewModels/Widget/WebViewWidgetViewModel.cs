using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Text;
using System.Windows;
using CefSharp;
using CefSharp.Wpf;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Views.Widget;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Widget
{
    /// <summary>
    /// ViewModelだけどあっちゃこっちゃ引き回されて <see cref="IWebViewGrass"/> にもなる忙しい人。
    /// </summary>
    public class WebViewWidgetViewModel: ViewModelBase, IWebViewGrass
    {
        public WebViewWidgetViewModel(WebViewWidgetWindow window, IHtmlSource htmlSource, Action<IWebViewGrass>? widgetCallback, EnvironmentParameters environmentParameters, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            WidgetWindow = window;
            WebView = WidgetWindow.webView;
            DispatcherWrapper = dispatcherWrapper;
            HtmlSource = htmlSource;
            WidgetCallback = widgetCallback;
            EnvironmentParameters = environmentParameters;

            if(WebView.IsBrowserInitialized) {
                LoadHtmlSource(HtmlSource);
            } else {
                WebView.IsBrowserInitializedChanged += WebView_IsBrowserInitializedChanged;
            }
        }

        #region proeprty

        WebViewWidgetWindow WidgetWindow { get; }
        IHtmlSource HtmlSource { get; }
        Action<IWebViewGrass>? WidgetCallback { get; }
        EnvironmentParameters EnvironmentParameters { get; }
        IDispatcherWrapper DispatcherWrapper { get; }
        #endregion

        #region command

        #endregion

        #region function

        void InjectWidget()
        {
            string injectionScript;
            using(var reader = EnvironmentParameters.WebViewWidgetInjectionFile.OpenText()) {
                injectionScript = reader.ReadToEnd();
            }

            WebView.ExecuteScriptAsync(injectionScript);
            //WebView.JavascriptObjectRepository.Register("Pe_Callback", this, true);
        }

        void LoadHtmlSource(IHtmlSource htmlSource)
        {
            Debug.Assert(WebView.IsLoaded);

            WebView.LoadingStateChanged += WebView_LoadingStateChanged;

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

        private void WebView_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            WebView.IsBrowserInitializedChanged -= WebView_IsBrowserInitializedChanged;
            LoadHtmlSource(HtmlSource);
        }

        private void WebView_LoadingStateChanged(object? sender, LoadingStateChangedEventArgs e)
        {
            if(!e.IsLoading) {
                DispatcherWrapper.Begin(() => {
                    InjectWidget();
                });
            }
        }

    }
}
