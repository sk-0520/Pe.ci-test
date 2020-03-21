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

        public Task SendAync(FeedbackInputData data)
        {
            return Task.CompletedTask;
        }

        public async Task<string> LoadHtmlSourceAsync()
        {
            const string feedbackHtml = "feedback-html";
            //const string feedbackScript = "FEEDBACK-TEMPLATE-SCRIPT";
            const string feedbackStyle = "FEEDBACK-TEMPLATE-STYLE";
            var fileMap = new Dictionary<string, FileInfo>() {
                [feedbackHtml] = EnvironmentParameters.WebViewFeedbackTemplateFile,
                [HtmlTemplateJqury] = EnvironmentParameters.WebViewJqueryScriptFile,
                [HtmlTemplateMarked] = EnvironmentParameters.WebViewMarkedScriptFile,
                [HtmlTemplateBasicStyle] = EnvironmentParameters.WebViewBasicStyleFile,
                //[feedbackScript] = EnvironmentParameters.WebViewFeedbackScriptFile,
                [feedbackStyle] = EnvironmentParameters.WebViewFeedbackStyleFile,
            };
            var sourceMap = await LoadSourceFilesAsync(fileMap);

            var map = new WebViewTemplateDictionary() {
                [HtmlTemplateLang] = new CultureWebViewTemplate(CultureService.Current.Culture),
                [HtmlTemplateJqury] = new RawTextWebViewTemplate(sourceMap[HtmlTemplateJqury]),
                [HtmlTemplateMarked] = new RawTextWebViewTemplate(sourceMap[HtmlTemplateMarked]),
                [HtmlTemplateBasicStyle] = new RawTextWebViewTemplate(sourceMap[HtmlTemplateBasicStyle]),
                //[feedbackScript] = new RawTextWebViewTemplate(sourceMap[feedbackScript]),
                [feedbackStyle] = new RawTextWebViewTemplate(sourceMap[feedbackStyle]),
                ["FEEDBACK-TITLE"] = new HtmlTextWebViewTemplate(Properties.Resources.String_Feedback_Title),
                ["FEEDBACK-DESCRIPTION"] = new HtmlTextWebViewTemplate(Properties.Resources.String_Feedback_Description),
                ["FEEDBACK-WARNING"] = new HtmlTextWebViewTemplate(Properties.Resources.String_Feedback_Warning),
                ["FEEDBACK-SUBJECT"] = new HtmlTextWebViewTemplate(Properties.Resources.String_Feedback_Subject),
                ["FEEDBACK-KIND"] = new HtmlTextWebViewTemplate(Properties.Resources.String_Feedback_Kind),
                ["FEEDBACK-KIND-OPTIONS"] = new CustomWebViewTemplate(s => {
                    var builder = new StringBuilder();
                    foreach(var kind in EnumUtility.GetMembers<FeedbackKind>().OrderBy(i => i)) {
                        builder.Append("<option value=\"");
                        builder.Append(HttpUtility.HtmlEncode(kind.ToString()));
                        builder.Append("\">");
                        builder.Append(HttpUtility.HtmlEncode(CultureService.GetString(kind, ResourceNameKind.Normal)));
                        builder.Append("</option>");
                    }
                    return builder.ToString();
                }),
                ["FEEDBACK-KIND-BUG"] = new HtmlTextWebViewTemplate(FeedbackKind.Bug.ToString()),
                ["FEEDBACK-KIND-PROPOSAL"] = new HtmlTextWebViewTemplate(FeedbackKind.Proposal.ToString()),
                ["FEEDBACK-KIND-OTHERS"] = new HtmlTextWebViewTemplate(FeedbackKind.Others.ToString()),
                ["FEEDBACK-KIND-SET"] = new HtmlTextWebViewTemplate(Properties.Resources.String_Feedback_KindSet),
                ["FEEDBACK-COMMENT"] = new HtmlTextWebViewTemplate(Properties.Resources.String_Feedback_Comment),
                ["FEEDBACK-CONTENT"] = new HtmlTextWebViewTemplate(Properties.Resources.String_Feedback_CommentContent),
                ["FEEDBACK-PREVIEW"] = new HtmlTextWebViewTemplate(Properties.Resources.String_Feedback_CommentPreview),
                ["FEEDBACK-FORMAT"] = new HtmlTextWebViewTemplate(Properties.Resources.String_Feedback_Comment_Format),
                ["FEEDBACK-SET-KIND-BUG"] = new RawTextWebViewTemplate(Properties.Resources.String_Feedback_Comment_Kind_Bug),
                ["FEEDBACK-SET-KIND-PROPOSAL"] = new RawTextWebViewTemplate(Properties.Resources.String_Feedback_Comment_Kind_Proposal),
                ["FEEDBACK-SET-KIND-OTHERS"] = new RawTextWebViewTemplate(Properties.Resources.String_Feedback_Comment_Kind_Others),
            };
            var embeddedSource = BuildTemplate(sourceMap[feedbackHtml], map);
            return embeddedSource;
        }

        #endregion

        #region WebViewElementBase

        protected override void InitializeImpl()
        { }

        #endregion
    }
}
