using CefSharp;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    /// <inheritdoc cref="IWebViewScriptResult"/>
    internal class WebViewScriptResult: IWebViewScriptResult
    {
        public WebViewScriptResult(JavascriptResponse javaScriptResponse)
        {
            Success = javaScriptResponse.Success;
            Result = javaScriptResponse.Result;
        }

        public WebViewScriptResult(bool success, object? result)
        {
            Success = success;
            Result = result;
        }

        #region function

        public static WebViewScriptResult Failure() => new WebViewScriptResult(false, null);

        #endregion

        #region IWebViewScriptResult

        /// <inheritdoc cref="IWebViewScriptResult.Success"/>
        public bool Success { get; }

        /// <inheritdoc cref="IWebViewScriptResult.Result"/>
        public object? Result { get; }

        #endregion
    }
}
