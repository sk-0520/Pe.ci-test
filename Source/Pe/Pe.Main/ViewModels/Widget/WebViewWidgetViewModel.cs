using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using CefSharp;
using CefSharp.Wpf;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Views.Widget;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Widget
{
    /// <summary>
    /// ViewModelだけどあっちゃこっちゃ引き回されて <see cref="IWebViewGrass"/> にもなる忙しい人。
    /// </summary>
    public class WebViewWidgetViewModel: ViewModelBase, IWebViewGrass
    {
        public WebViewWidgetViewModel(IPluginIdentifiers pluginIdentifiers, WebViewWidgetWindow window, IHtmlSource htmlSource, Action<IWebViewGrass>? widgetCallback, object? pluginExtensions, EnvironmentParameters environmentParameters, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            PluginIdentifiers = pluginIdentifiers;
            WidgetWindow = window;
            WebView = WidgetWindow.webView;
            DispatcherWrapper = dispatcherWrapper;
            HtmlSource = htmlSource;
            WidgetCallback = widgetCallback;
            EnvironmentParameters = environmentParameters;
            PluginExtensions = pluginExtensions;

            Callbacks = new WebViewWidgetCallbacks(PluginIdentifiers, LoggerFactory);

            Callbacks.MoveStarted += Callbacks_MoveStarted;
            Callbacks.ResizeStarted += Callbacks_ResizeStarted;

            if(WebView.IsBrowserInitialized) {
                LoadHtmlSource(HtmlSource);
            } else {
                WebView.IsBrowserInitializedChanged += WebView_IsBrowserInitializedChanged;
            }
        }

        #region proeprty

        public IPluginIdentifiers PluginIdentifiers { get; }
        WebViewWidgetWindow WidgetWindow { get; }
        IHtmlSource HtmlSource { get; }
        Action<IWebViewGrass>? WidgetCallback { get; }
        EnvironmentParameters EnvironmentParameters { get; }
        IDispatcherWrapper DispatcherWrapper { get; }
        TimeSpan ScriptTimeout { get; } = TimeSpan.FromMinutes(1);
        WebViewWidgetCallbacks Callbacks { get; }
        object? PluginExtensions { get; }
        #endregion

        #region command

        #endregion

        #region function

        void InjectWidget()
        {
            string injectionScript;
            using(var reader = EnvironmentParameters.WebViewWidgetInjectionScriptFile.OpenText()) {
                injectionScript = reader.ReadToEnd();
            }

            string injectionStyle;
            using(var reader = EnvironmentParameters.WebViewWidgetInjectionStyleSheetFile.OpenText()) {
                injectionStyle = reader.ReadToEnd();
            }

            WebView.JavascriptObjectRepository.Register("pe_callbacks", Callbacks, true);
            if(PluginExtensions != null) {
                WebView.JavascriptObjectRepository.Register("pe_extensions", PluginExtensions, true);
            }

            Callbacks.Injected += Callbacks_Injected;

            //WebView.ExecuteScriptAsync(injectionScript, injectionStyle);
            WebView.ExecuteScriptAsync(injectionScript, injectionStyle);

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
        }

        IWebViewScriptResult EvaluateScriptAsyncCore(Task<JavascriptResponse> javascriptResponse)
        {
            if(!javascriptResponse.IsCompletedSuccessfully) {
                Logger.LogError(javascriptResponse.Exception, "{0} スクリプト実行失敗: {2}, {1}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId, javascriptResponse.Exception?.Message);
                return WebViewScriptResult.Failure();
            }
            var result = javascriptResponse.Result;
            if(!result.Success) {
                Logger.LogError("{0} スクリプト実行失敗: {2}, {1}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId, result.Message);
            }
            return new WebViewScriptResult(result);
        }

        #endregion

        #region IWebViewGrass

        /// <inheritdoc cref="IWebViewGrass.WebView"/>
        public ChromiumWebBrowser WebView { get; }
        object IWebViewGrass.WebView => WebView;

        /// <inheritdoc cref="IWebViewGrass.ExecuteScriptAsync(string)"/>
        public void ExecuteScriptAsync(string script)
        {
            if(WebView.CanExecuteJavascriptInMainFrame) {
                WebView.ExecuteScriptAsync(script);
            } else {
                Logger.LogError("{0} のスクリプト実行不可状態, {1}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId);
            }
        }
        /// <inheritdoc cref="IWebViewGrass.ExecuteScriptAsync(string, object[])"/>
        public void ExecuteScriptAsync(string methodName, params object[] parameters)
        {
            if(WebView.CanExecuteJavascriptInMainFrame) {
                WebView.ExecuteScriptAsync(methodName, parameters);
            } else {
                Logger.LogError("{0} スクリプト実行不可状態, {1}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId);
            }
        }

        /// <inheritdoc cref="IWebViewGrass.EvaluateScriptAsync(string)"/>
        public Task<IWebViewScriptResult> EvaluateScriptAsync(string script)
        {
            if(WebView.CanExecuteJavascriptInMainFrame) {
                return WebView.EvaluateScriptAsync(script, ScriptTimeout).ContinueWith(EvaluateScriptAsyncCore);
            } else {
                Logger.LogError("{0} スクリプト実行不可状態, {1}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId);
                return Task.FromResult<IWebViewScriptResult>(WebViewScriptResult.Failure());
            }
        }

        /// <inheritdoc cref="IWebViewGrass.EvaluateScriptAsync(string, object[])"/>
        public Task<IWebViewScriptResult> EvaluateScriptAsync(string methodName, params object[] parameters)
        {
            if(WebView.CanExecuteJavascriptInMainFrame) {
                return WebView.EvaluateScriptAsync(ScriptTimeout, methodName, parameters).ContinueWith(EvaluateScriptAsyncCore);
            } else {
                Logger.LogError("{0} スクリプト実行不可状態, {1}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId);
                return Task.FromResult<IWebViewScriptResult>(WebViewScriptResult.Failure());
            }
        }

        #endregion

        #region WebViewWidgetViewModel

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                WebView.IsBrowserInitializedChanged -= WebView_IsBrowserInitializedChanged;

                Callbacks.MoveStarted -= Callbacks_MoveStarted;
                Callbacks.ResizeStarted -= Callbacks_ResizeStarted;
                Callbacks.Injected -= Callbacks_Injected;
            }
            base.Dispose(disposing);
        }

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

        private void Callbacks_MoveStarted(object? sender, EventArgs e)
        {
            DispatcherWrapper.Begin(() => {
                WebView.ReleaseMouseCapture();
                NativeMethods.SendMessage(HandleUtility.GetWindowHandle(WidgetWindow), PInvoke.Windows.WM.WM_NCLBUTTONDOWN, new IntPtr((int)HT.HTCAPTION), IntPtr.Zero);
            }, System.Windows.Threading.DispatcherPriority.Normal);
        }
        private void Callbacks_ResizeStarted(object? sender, WebViewWidgetResizeEventArgs e)
        {
            DispatcherWrapper.Begin(() => {
                if(WidgetWindow.ResizeMode == ResizeMode.NoResize) {
                    Logger.LogWarning("{0} はリサイズが許可されていない, {1}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId);
                    return;
                }

                WebView.ReleaseMouseCapture();
                var scSizeEx = (int)SC.SC_SIZE + e.Direction switch
                {
                    WebViewWidgetResizeDirection.North => 3,
                    WebViewWidgetResizeDirection.South => 6,
                    WebViewWidgetResizeDirection.East => 2,
                    WebViewWidgetResizeDirection.West => 1,
                    WebViewWidgetResizeDirection.NorthEast => 5,
                    WebViewWidgetResizeDirection.NorthWest => 4,
                    WebViewWidgetResizeDirection.SouthWest => 7,
                    WebViewWidgetResizeDirection.SouthEast => 8,
                    _ => throw new NotImplementedException(),
                };

                NativeMethods.SendMessage(HandleUtility.GetWindowHandle(WidgetWindow), PInvoke.Windows.WM.WM_SYSCOMMAND, new IntPtr(scSizeEx), IntPtr.Zero);
            }, System.Windows.Threading.DispatcherPriority.Normal);
        }

        private void Callbacks_Injected(object? sender, EventArgs e)
        {
            Callbacks.Injected -= Callbacks_Injected;

            if(WidgetCallback != null) {
                WidgetCallback(this);
            }
        }

    }
}
