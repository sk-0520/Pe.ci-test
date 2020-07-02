using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CefSharp;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.WebView
{
    public class ApplicationStorageSchemeHandlerFactory: ISchemeHandlerFactory
    {
        public ApplicationStorageSchemeHandlerFactory(EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            EnvironmentParameters = environmentParameters;
        }

        #region property

        ILogger Logger { get; }
        EnvironmentParameters EnvironmentParameters { get; }

        public static string SchemeName => "pe";
        public static string DomainName => "storage";

        #endregion

        #region ISchemeHandlerFactory

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            var uri = new Uri(request.Url);

            var rootDir =
#if PRODUCT
            EnvironmentParameters.RootDirectory
#else
            EnvironmentParameters.ApplicationBaseDirectory
#endif
            ;

            var path = Path.Combine(rootDir.FullName, string.Join(Path.DirectorySeparatorChar, uri.Segments.Skip(1).Select(i => i.Trim('/')).Where(i => !string.IsNullOrEmpty(i))));
            if(File.Exists(path)) {
                string? mime = null;
                var ext = Path.GetExtension(uri.Segments.Last());
                if(ext != null && 1 < ext.Length) {
                    mime = Cef.GetMimeType(ext.Substring(1));
                }
                return ResourceHandler.FromFilePath(path, mime, true);
            }

            return null!;
        }

        #endregion
    }
}
