using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CefSharp;
using CefSharp.Handler;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.WebView
{
    public class PlatformRequestHandler : RequestHandler
    {
        public PlatformRequestHandler(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected ILogger Logger { get; }

        #endregion

        #region RequestHandler

        protected override bool OnOpenUrlFromTab(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            if(userGesture) {
                try {
                    var systemExecutor = new SystemExecutor();
                    systemExecutor.OpenUri(new Uri(targetUrl));
                } catch(Exception ex) {
                    Logger.LogWarning(ex, ex.Message);
                }
                return true;
            }

            return base.OnOpenUrlFromTab(chromiumWebBrowser, browser, frame, targetUrl, targetDisposition, userGesture);
        }

        #endregion

    }
}
