using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Feedback
{
    public class FeedbackElement : WebViewElementBase
    {
        public FeedbackElement(EnvironmentParameters environmentParameters, IOrderManager orderManager, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(userAgentManager, loggerFactory)
        {
            EnvironmentParameters = environmentParameters;
            OrderManager = orderManager;
        }

        #region property

        EnvironmentParameters EnvironmentParameters { get; }
        IOrderManager OrderManager { get; }

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

            var map = new Dictionary<string, WebViewTemplate>() {
                [HtmlTemplateJqury] = new WebViewTemplate(TemplateTarget.Raw, jquerySource),
                ["FEEDBACK-TEMPLATE-SCRIPT"] = new WebViewTemplate(TemplateTarget.Raw, scriptSource),
                ["FEEDBACK-TEMPLATE-STYLE"] = new WebViewTemplate(TemplateTarget.Raw, styleSource),
                ["FEEDBACK-TITLE"] = new WebViewTemplate(TemplateTarget.Text, "title"),
                ["FEEDBACK-DESCRIPTION"] = new WebViewTemplate(TemplateTarget.Text, "description"),
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
