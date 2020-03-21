using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.WebView;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element
{

    public abstract class WebViewElementBase : ElementBase
    {
        #region define

        protected class WebViewTemplateDictionary : Dictionary<string, WebViewTemplateBase>
        {
            public WebViewTemplateDictionary()
            { }

            public WebViewTemplateDictionary(IDictionary<string, WebViewTemplateBase> dictionary) : base(dictionary)
            { }
        }

        #endregion


        public WebViewElementBase(IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            UserAgentManager = userAgentManager;
        }

        #region property

        public string HtmlTemplateLang => "HTML-TEMPLATE-LANG";
        public string HtmlTemplateJqury => "HTML-TEMPLATE-JQUERY";
        public string HtmlTemplateMarked => "HTML-TEMPLATE-MARKED";
        public string HtmlTemplateBasicStyle => "HTML-TEMPLATE-BASIC-STYLE";

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

        #endregion
    }
}
