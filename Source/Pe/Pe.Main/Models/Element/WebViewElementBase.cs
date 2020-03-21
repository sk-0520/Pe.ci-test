using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element
{
    public enum TemplateTarget
    {
        Raw,
        //StyleSheet,
        //Script,
        Text,
    }

    public class WebViewTemplate
    {
        public WebViewTemplate(TemplateTarget target, string value)
        {
            Target = target;
            Value = value;
        }

        #region property


        public TemplateTarget Target { get; }
        public string Value { get; }

        #endregion
    }

    public abstract class WebViewElementBase : ElementBase
    {
        public WebViewElementBase(IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            UserAgentManager = userAgentManager;
        }

        #region property
        public string HtmlTemplateJqury => "HTML-TEMPLATE-JQUERY";

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

        private string ConvertTemplate(WebViewTemplate template)
        {
            switch(template.Target) {
                case TemplateTarget.Raw:
                    return template.Value;

                //case TemplateTarget.StyleSheet:
                //    return template.Value;

                //case TemplateTarget.Script:
                //    return template.Value;

                case TemplateTarget.Text:
                    return HttpUtility.HtmlEncode(template.Value);

                default:
                    throw new NotImplementedException();
            }
        }

        protected string BuildTemplate(string source, IReadOnlyDictionary<string, WebViewTemplate> map)
        {
            return TemplateRegex.Replace(source, (Match m) => {
                var key = m.Groups["KEY"].Value;
                if(map.TryGetValue(key, out var template)) {
                    return ConvertTemplate(template);
                }
                return m.Value;
            });
        }

        #endregion
    }
}
