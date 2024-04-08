using System.Collections.Generic;
using CefSharp;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Standard.Base;

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

        private static void WriteHttpUserAgentWebViewValue(IDictionary<string, string> target)
        {
            target.Add("BROWSER-CORE-VERSION", Cef.CefVersion);
            target.Add("BROWSER-LIBRARY-VERSION", Cef.CefSharpVersion);
            target.Add("BROWSER-REVISION", Cef.CefCommitHash);
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
        public static string GetHttpUserAgentWebViewValue(string format)
        {
            var map = new Dictionary<string, string>();

            WriteHttpUserAgentValue(map);
            WriteHttpUserAgentWebViewValue(map);

            return TextUtility.ReplaceFromDictionary(format, map);
        }

        #endregion
    }
}
