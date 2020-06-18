using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CefSharp;
using CefSharp.Handler;
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

        #region proeprty

        ILogger Logger { get; }
        DirectoryInfo WidgetDirectory { get; }

        #endregion

        #region ResourceRequestHandler

        protected override IResourceHandler GetResourceHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
        {
            //ResourceHandler has many static methods for dealing with Streams,
            // byte[], files on disk, strings
            // Alternatively ou can inheir from IResourceHandler and implement
            // a custom behaviour that suites your requirements.
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
            return ResourceHandler.FromFilePath(path, mime, true);
        }

        #endregion
    }

    public class WebViewWidgetRequestHandler: RequestHandler
    {
        public WebViewWidgetRequestHandler(DirectoryInfo widgetDirectory, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            WidgetDirectory = widgetDirectory;
        }

        #region proeprty

        ILogger Logger { get; }
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
