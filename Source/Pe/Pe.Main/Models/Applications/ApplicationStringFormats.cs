using System;
using System.Collections.Generic;
using System.Text;
using CefSharp;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Logic;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    internal static class ApplicationStringFormats
    {
        #region function

        public static string GetHttpUserAgentValue(string format)
        {
            var versionConverter = new VersionConverter();

            var map = new Dictionary<string, string>() {
                ["APPLICATION-NAME"] = BuildStatus.Name,
                ["APPLICATION-BUILD"] = BuildStatus.BuildType.ToString(),
                ["APPLICATION-VERSION"] = versionConverter.ConvertNormalVersion(BuildStatus.Version),
                ["APPLICATION-REVISION"] = BuildStatus.Revision,
                ["BROWSER-CORE-VERSION"] = Cef.CefVersion,
                ["BROWSER-LIBRARY-VERSION"] = Cef.CefSharpVersion,
                ["BROWSER-REVISION"] = Cef.CefCommitHash,
            };

            return TextUtility.ReplaceFromDictionary(format, map);
        }

        #endregion
    }
}
