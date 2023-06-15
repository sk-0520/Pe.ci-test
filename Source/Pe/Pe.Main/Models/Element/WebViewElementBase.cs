using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.WebView;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element
{
    public abstract class WebViewElementBase: ElementBase
    {
        #region define

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly")]
        protected class WebViewTemplateDictionary: Dictionary<string, WebViewTemplateBase>
        {
            public WebViewTemplateDictionary()
            { }

            public WebViewTemplateDictionary(IDictionary<string, WebViewTemplateBase> dictionary) : base(dictionary)
            { }
        }

        #endregion

        protected WebViewElementBase(IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            UserAgentManager = userAgentManager;
        }

        #region property

        public string HtmlTemplateLang => "HTML-TEMPLATE-LANG";

        protected IUserAgentManager UserAgentManager { get; }

        protected static Regex TemplateRegex { get; } = new Regex(
            @"
            (
                (
                    (<!--)
                    |
                    (/\*)
                )
                \s*
            )?
            \${
            (?<KEY>.+?)
            (
                :
                (?<OPTION>.+?)
            )?
            }
            (
                \s*
                (
                    (-->)
                    |
                    (\*/)
                )
            )?
            ",
            RegexOptions.Multiline | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace
        );

        #endregion

        #region function

        private async Task<KeyValuePair<string, string>> LoadSourceFileAsync(KeyValuePair<string, FileInfo> pair)
        {
            var file = pair.Value;
            using var htmlReader = new StreamReader(file.OpenRead());
            var content = await htmlReader.ReadToEndAsync();
            return KeyValuePair.Create(pair.Key, content);
        }

        protected async Task<IReadOnlyDictionary<string, string>> LoadSourceFilesAsync(IReadOnlyDictionary<string, FileInfo> loadFiles)
        {
            var result = new Dictionary<string, string>(loadFiles.Count);
            foreach(var pair in loadFiles) {
                var loadedFile = await LoadSourceFileAsync(pair);
                result.Add(loadedFile.Key, loadedFile.Value);
            }
            return result;
        }

        protected string BuildTemplate(string source, IReadOnlyDictionary<string, WebViewTemplateBase> map)
        {
            return TemplateRegex.Replace(source, (Match m) => {
                var key = m.Groups["KEY"].Value;
                var option = m.Groups["OPTION"].Value;
                if(map.TryGetValue(key, out var template)) {
                    return template.Build(option);
                }
                return m.Value;
            });
        }

        public string ToJavaScriptString(string text)
        {
            var s = text
                .Replace("\\", @"\\")
                .Replace("\r", @"\r")
                .Replace("\n", @"\n")
                .Replace("\t", @"\t")
                .Replace("\"", @"\""")
                .Replace("\'", @"\'")
            ;
            return "\"" + s + "\"";
        }

        #endregion
    }
}
