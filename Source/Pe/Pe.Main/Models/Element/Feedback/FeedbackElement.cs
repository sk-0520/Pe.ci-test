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

            var results = await Task.WhenAll(new[] {
                htmlReader.ReadToEndAsync(),
                jqueryReader.ReadToEndAsync(),
                scriptReader.ReadToEndAsync(),
                styleReader.ReadToEndAsync(),
            });

            var htmlSource = results[0];
            var jquerySource = results[1];
            var scriptSource = results[2];
            var styleSource = results[3];

            var map = new Dictionary<string, string>() {
                ["HTML-JQUERY"] = jquerySource,
                ["FEEDBACK-SCRIPT"] = scriptSource,
                ["FEEDBACK-STYLE"] = styleSource,
                ["FEEDBACK-TITLE"] = "title",
                ["FEEDBACK-DESCRIPTION"] = "description",
            };
            var embeddedSource = TextUtility.ReplaceFromDictionary(htmlSource, map);
            return embeddedSource;
        }

        #endregion

        #region WebViewElementBase

        protected override void InitializeImpl()
        { }

        #endregion
    }
}
