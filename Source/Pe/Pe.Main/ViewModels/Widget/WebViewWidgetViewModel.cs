using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using CefSharp;
using CefSharp.Wpf;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
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
        public WebViewWidgetViewModel(IPluginIdentifiers pluginIdentifiers, WebViewWidgetWindow window, HtmlSourceBase htmlSource, Action<IWebViewGrass>? widgetCallback, object? pluginExtensions, EnvironmentParameters environmentParameters, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
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

        #region property

        public IPluginIdentifiers PluginIdentifiers { get; }
        private WebViewWidgetWindow WidgetWindow { get; }
        private HtmlSourceBase HtmlSource { get; }
        private Action<IWebViewGrass>? WidgetCallback { get; }
        private EnvironmentParameters EnvironmentParameters { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }
        private TimeSpan ScriptTimeout { get; } = TimeSpan.FromMinutes(1);
        private WebViewWidgetCallbacks Callbacks { get; }
        private object? PluginExtensions { get; }

        #endregion

        #region command

        #endregion

        #region function

        private void InjectWidget()
        {
            string injectionScript;
            using(var reader = EnvironmentParameters.WebViewWidgetInjectionScriptFile.OpenText()) {
                injectionScript = reader.ReadToEnd();
            }

            string injectionStyle;
            using(var reader = EnvironmentParameters.WebViewWidgetInjectionStyleSheetFile.OpenText()) {
                injectionStyle = reader.ReadToEnd();
            }


            if(!WebView.JavascriptObjectRepository.IsBound("pe_callbacks")) {
                WebView.JavascriptObjectRepository.Register("pe_callbacks", Callbacks, BindingOptions.DefaultBinder);
            }
            if(!WebView.JavascriptObjectRepository.IsBound("pe_extensions")) {
                if(PluginExtensions != null) {
                    WebView.JavascriptObjectRepository.Register("pe_extensions", PluginExtensions, BindingOptions.DefaultBinder);
                } else {
                    // 一応ダミーで作っておく
                    WebView.JavascriptObjectRepository.Register("pe_extensions", new object(), BindingOptions.DefaultBinder);
                }
            }

            //WebView.JavascriptObjectRepository.ObjectBoundInJavascript += JavascriptObjectRepository_ObjectBoundInJavascript;

            Callbacks.Injected += Callbacks_Injected;

            //WebView.ExecuteScriptAsync(injectionScript, injectionStyle);
            WebView.ExecuteScriptAsync(injectionScript, injectionStyle);

        }

        private void LoadHtmlSource(HtmlSourceBase htmlSource)
        {
            Debug.Assert(WebView.IsLoaded);

            WebView.LoadingStateChanged += WebView_LoadingStateChanged;

            switch(htmlSource.HtmlSourceKind) {
                case HtmlSourceKind.Address: {
                        var address = (HtmlAddress)htmlSource;
                        WebView.Load(address.Address.ToString());
                    }
                    break;

                case HtmlSourceKind.SourceCode: {
                        var sourceCode = (HtmlSourceCode)htmlSource;
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

        private IWebViewScriptResult EvaluateScriptAsyncCore(Task<JavascriptResponse> javascriptResponse)
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
        public Task ExecuteScriptAsync(string script)
        {
            if(WebView.CanExecuteJavascriptInMainFrame) {
                WebView.ExecuteScriptAsync(script);
            } else {
                Logger.LogError("{0} のスクリプト実行不可状態, {1}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId);
            }
            return Task.CompletedTask;
        }
        /// <inheritdoc cref="IWebViewGrass.ExecuteScriptAsync(string, object[])"/>
        public Task ExecuteScriptAsync(string methodName, params object[] parameters)
        {
            if(WebView.CanExecuteJavascriptInMainFrame) {
                WebView.ExecuteScriptAsync(methodName, parameters);
            } else {
                Logger.LogError("{0} スクリプト実行不可状態, {1}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId);
            }
            return Task.CompletedTask;
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
                DispatcherWrapper.BeginAsync(() => {
                    InjectWidget();
                });
            }
        }

        private void Callbacks_MoveStarted(object? sender, EventArgs e)
        {
            DispatcherWrapper.BeginAsync(() => {
                WebView.ReleaseMouseCapture();
                NativeMethods.SendMessage(HandleUtility.GetWindowHandle(WidgetWindow), PInvoke.Windows.WM.WM_NCLBUTTONDOWN, new IntPtr((int)HT.HTCAPTION), IntPtr.Zero);
            }, System.Windows.Threading.DispatcherPriority.Normal);
        }
        private void Callbacks_ResizeStarted(object? sender, WebViewWidgetResizeEventArgs e)
        {
            DispatcherWrapper.BeginAsync(() => {
                if(WidgetWindow.ResizeMode == ResizeMode.NoResize) {
                    Logger.LogWarning("{0} はリサイズが許可されていない, {1}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId);
                    return;
                }

                WebView.ReleaseMouseCapture();
                var scSizeEx = (int)SC.SC_SIZE + e.Direction switch {
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

        private void JavascriptObjectRepository_ObjectBoundInJavascript(object? sender, CefSharp.Event.JavascriptBindingCompleteEventArgs e)
        {
            if(e.ObjectName == "pe_callbacks") {
                if(!e.ObjectRepository.IsBound("pe_callbacks")) {
                    e.ObjectRepository.Register("pe_callbacks", Callbacks, BindingOptions.DefaultBinder);
                    Logger.LogDebug("register: pe_callbacks");
                }
            }
            if(e.ObjectName == "pe_extensions") {
                if(!e.ObjectRepository.IsBound("pe_extensions")) {
                    e.ObjectRepository.Register("pe_extensions", PluginExtensions, BindingOptions.DefaultBinder);
                    Logger.LogDebug("register: pe_extensions");
                }
            }
        }
    }
}
