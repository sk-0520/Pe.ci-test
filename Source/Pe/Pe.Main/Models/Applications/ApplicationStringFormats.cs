using System.Collections.Generic;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Web.WebView2.Wpf;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    internal static class ApplicationStringFormats
    {
        #region function

        private static void WriteHttpUserAgentValue(IDictionary<string, string> target)
        {
            var versionConverter = new VersionConverter();

            target.Add("APPLICATION-NAME", BuildStatus.Name);
            target.Add("APPLICATION-BUILD", BuildStatus.BuildType.ToString());
            target.Add("APPLICATION-VERSION", versionConverter.ConvertNormalVersion(BuildStatus.Version));
            target.Add("APPLICATION-REVISION", BuildStatus.Revision);
        }

        private static void WriteHttpUserAgentWebViewValue(IDictionary<string, string> target, WebView2 webView)
        {
            target.Add("BROWSER-LIBRARY-NAME", typeof(WebView2).FullName!);
            target.Add("BROWSER-CORE-VERSION", webView.CoreWebView2.Environment.BrowserVersionString);
            target.Add("BROWSER-LIBRARY-VERSION", typeof(WebView2).Assembly.GetName().Version!.ToString());
        }

        /// <summary>
        /// 通常HTTP処理のUA文字列を取得。
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetHttpUserAgentValue(string format)
        {
            var map = new Dictionary<string, string>();

            WriteHttpUserAgentValue(map);

            return TextUtility.ReplaceFromDictionary(format, map);
        }

        /// <summary>
        /// WebViewのUA文字列を取得。
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetHttpUserAgentWebViewValue(string format, WebView2 webView)
        {
            var map = new Dictionary<string, string>();

            WriteHttpUserAgentValue(map);
            WriteHttpUserAgentWebViewValue(map, webView);

            return TextUtility.ReplaceFromDictionary(format, map);
        }

        #endregion
    }
}
