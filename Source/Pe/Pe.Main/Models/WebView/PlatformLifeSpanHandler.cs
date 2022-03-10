using System;
using CefSharp;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.WebView
{
    public class PlatformLifeSpanHandler: ILifeSpanHandler
    {
        public PlatformLifeSpanHandler(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        private ILogger Logger { get; }

        #endregion

        #region ILifeSpanHandler

        public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            return true;
        }

        public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
        { }

        public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            throw new NotSupportedException();
        }

        public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            try {
                var systemExecutor = new SystemExecutor();
                systemExecutor.OpenUri(new Uri(targetUrl));
            } catch(Exception ex) {
                Logger.LogWarning(ex, ex.Message);
            }
            newBrowser = default!;

            return true;
        }

        #endregion
    }
}
