using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CefSharp;
using CefSharp.Handler;
using ContentTypeTextNet.Pe.Main.Models.WebView;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public class WebViewWidgetResourceRequestHandler: ResourceRequestHandler
    {
        public WebViewWidgetResourceRequestHandler(DirectoryInfo widgetDirectory, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            WidgetDirectory = widgetDirectory;
        }

        #region property

        ILogger Logger { get; }
        DirectoryInfo WidgetDirectory { get; }

        #endregion

        #region ResourceRequestHandler

        protected override IResourceHandler GetResourceHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
        {
            var uri = new Uri(request.Url);

            var path = 1 < uri.LocalPath.Length
                ? Path.Combine(WidgetDirectory.FullName, uri.LocalPath.Substring(1))
                : Path.Combine(WidgetDirectory.FullName, "index.html")
            ;
            string? mime = null;
            var ext = Path.GetExtension(path);
            if(ext != null && 1 < ext.Length) {
                mime = Cef.GetMimeType(ext.Substring(1));
            }

            try {
                if(File.Exists(path)) {
                    return ResourceHandler.FromFilePath(path, mime, true);
                }
            } catch(IOException ex) {
                Logger.LogError(ex, ex.Message);
            }

            return null!;
        }

        #endregion
    }

    public class WebViewWidgetRequestHandler: PlatformRequestHandler
    {
        public WebViewWidgetRequestHandler(DirectoryInfo widgetDirectory, ILoggerFactory loggerFactory)
            :base(loggerFactory)
        {
            LoggerFactory = loggerFactory;
            WidgetDirectory = widgetDirectory;
        }

        #region property

        ILoggerFactory LoggerFactory { get; }
        DirectoryInfo WidgetDirectory { get; }

        #endregion

        #region RequestHandler

        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            if(request.Url.StartsWith("pe://plugin")) {
                return new WebViewWidgetResourceRequestHandler(WidgetDirectory, LoggerFactory);
            }

            return null!;
        }
        #endregion
    }
}
