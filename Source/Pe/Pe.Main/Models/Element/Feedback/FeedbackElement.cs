using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.WebView;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Feedback
{
    public class FeedbackElement : WebViewElementBase
    {
        public FeedbackElement(EnvironmentParameters environmentParameters, CultureService cultureService, IOrderManager orderManager, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(userAgentManager, loggerFactory)
        {
            EnvironmentParameters = environmentParameters;
            OrderManager = orderManager;
            CultureService = cultureService;
        }

        #region property

        EnvironmentParameters EnvironmentParameters { get; }
        IOrderManager OrderManager { get; }
        CultureService CultureService { get; }
        #endregion

        #region function

        public Task SendAync()
        {
            return Task.CompletedTask;
        }

        public async Task<string> LoadHtmlSourceAsync()
        {
            using var htmlReader = new StreamReader(EnvironmentParameters.WebViewFeedbackTemplateFile.OpenRead());
            using var jqueryReader = new StreamReader(EnvironmentParameters.WebViewJQueryScriptFile.OpenRead());
            using var scriptReader = new StreamReader(EnvironmentParameters.WebViewFeedbackScriptFile.OpenRead());
            using var styleReader = new StreamReader(EnvironmentParameters.WebViewFeedbackStyleFile.OpenRead());

            // これは非同期で一気にやる必要ないと思うなぁ
            var htmlTask = htmlReader.ReadToEndAsync();
            var jqueryTask = jqueryReader.ReadToEndAsync();
            var scriptTask = scriptReader.ReadToEndAsync();
            var styleTask = styleReader.ReadToEndAsync();

            var results = await Task.WhenAll(new[] {
                htmlTask,
                jqueryTask,
                scriptTask,
                styleTask,
            });

            var htmlSource = htmlTask.Result;
            var jquerySource = jqueryTask.Result;
            var scriptSource = scriptTask.Result;
            var styleSource = styleTask.Result;

            var map = new WebViewTemplateDictionary() {
                [HtmlTemplateLang] = new CultureWebViewTemplate(CultureService.Current.Culture),
                [HtmlTemplateJqury] = new RawTextWebViewTemplate(jquerySource),
                ["FEEDBACK-TEMPLATE-SCRIPT"] = new RawTextWebViewTemplate(scriptSource),
                ["FEEDBACK-TEMPLATE-STYLE"] = new RawTextWebViewTemplate(styleSource),
                ["FEEDBACK-TITLE"] = new HtmlTextWebViewTemplate("title"),
                ["FEEDBACK-DESCRIPTION"] = new HtmlTextWebViewTemplate("description"),
                ["FEEDBACK-SUBJECT"] = new HtmlTextWebViewTemplate("subject"),
                ["FEEDBACK-KIND"] = new HtmlTextWebViewTemplate("target"),
                ["FEEDBACK-KIND-OPTIONS"] = new CustomWebViewTemplate(s => {
                    var builder = new StringBuilder();
                    foreach(var kind in EnumUtility.GetMembers<FeedbackKind>().OrderBy(i => i)) {
                        builder.Append("<option value=");
                        builder.Append(HttpUtility.HtmlEncode(kind.ToString()));
                        builder.Append("\">");
                        builder.Append(HttpUtility.HtmlEncode(CultureService.GetString(kind, ResourceNameKind.Normal)));
                        builder.Append("</option>");
                    }
                    return builder.ToString();
                }),
            };
            var embeddedSource = BuildTemplate(htmlSource, map);
            return embeddedSource;
        }

        #endregion

        #region WebViewElementBase

        protected override void InitializeImpl()
        { }

        #endregion
    }
}
