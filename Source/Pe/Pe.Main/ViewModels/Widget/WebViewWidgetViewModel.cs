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
using ContentTypeTextNet.Pe.Main.Views.Widget;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Widget
{
    /// <summary>
    /// ViewModelだけどあっちゃこっちゃ引き回されて <see cref="IWebViewGrass"/> にもなる忙しい人。
    /// </summary>
    public class WebViewWidgetViewModel: ViewModelBase, IWebViewGrass
    {
        public WebViewWidgetViewModel(WebViewWidgetWindow window, IHtmlSource htmlSource, Action<IWebViewGrass>? widgetCallback, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            WidgetWindow = window;
            WebView = WidgetWindow.webView;
            DispatcherWrapper = dispatcherWrapper;
            HtmlSource = htmlSource;
            WidgetCallback = widgetCallback;

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
        IDispatcherWrapper DispatcherWrapper { get; }
        #endregion

        #region command

        #endregion

        #region function

        void InjectWidget()
        {
            var js = @"
alert(1);
function() {

alert(2);

/*
    const styleElement = document.createElement('style');
    styleElement.textContent(`
    *.PE:MOVE-AREA {
        cursor: move;
    }
    *.PE:RESIZE-AREA {
        cursor: nw-resize;
    }
    `);
    document.body.appendChild(styleElement);

    const moveAreaElements = document.querySelectorAll('PE:MOVE-AREA');

    for(const moveAreaElement of moveAreaElements) {
    }

    const moveAreaElements = document.querySelectorAll('PE:RESIZE-AREA');

*/
}();";
            WebView.ExecuteScriptAsync(js);
            //WebView.JavascriptObjectRepository.Register("Pe_Callback", this, true);
        }

        void LoadHtmlSource(IHtmlSource htmlSource)
        {
            Debug.Assert(WebView.IsLoaded);

            WebView.LoadingStateChanged += WebView_LoadingStateChanged;
            WebView.FrameLoadEnd += WebView_FrameLoadEnd;

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

        private void WebView_FrameLoadEnd(object? sender, FrameLoadEndEventArgs e)
        {
            if(e.Frame.IsMain) {
                DispatcherWrapper.Begin(() => {
                    InjectWidget();
                });
            }
        }

        private void WebView_LoadingStateChanged(object? sender, LoadingStateChangedEventArgs e)
        {
            if(!e.IsLoading) {
                DispatcherWrapper.Begin(() => {
                    InjectWidget();
                });
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
            WebView.IsBrowserInitializedChanged += WebView_IsBrowserInitializedChanged;
            LoadHtmlSource(HtmlSource);
        }

    }
}
