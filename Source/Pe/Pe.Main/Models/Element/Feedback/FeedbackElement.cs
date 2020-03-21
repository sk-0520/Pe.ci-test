using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.WebView;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Feedback
{
    public class FeedbackElement : WebViewElementBase
    {
        public FeedbackElement(EnvironmentParameters environmentParameters, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, CultureService cultureService, IOrderManager orderManager, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(userAgentManager, loggerFactory)
        {
            EnvironmentParameters = environmentParameters;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
            OrderManager = orderManager;
            CultureService = cultureService;

            SendStatus = new RunningStatus(LoggerFactory);
        }

        #region property

        EnvironmentParameters EnvironmentParameters { get; }
        ApiConfiguration ApiConfiguration => EnvironmentParameters.Configuration.Api;
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IOrderManager OrderManager { get; }
        CultureService CultureService { get; }

        TimeSpan RetryWaitTime { get; } = TimeSpan.FromSeconds(5);
        public RunningStatus SendStatus { get; }
        public string ErrorMessage { get; private set; } = string.Empty;
        #endregion

        #region function

        public async Task SendAync(FeedbackInputData inputData)
        {
            ErrorMessage = string.Empty;
            SendStatus.State = RunningState.Running;

            var settingData = await Task.Run(() => {
                return MainDatabaseBarrier.ReadData(c => {
                    var appExecuteSettingEntityDao = new AppExecuteSettingEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                    //var appGeneralSettingEntityDao = new AppGeneralSettingEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);

                    var userIdManager = new UserIdManager(LoggerFactory);
                    var userId = userIdManager.SafeGetOrCreateUserId(appExecuteSettingEntityDao);

                    var firstData = appExecuteSettingEntityDao.SelectFirstData();

                    return (userId, firstVersion: firstData.FirstExecuteVersion, firstTimestamp: firstData.FirstExecuteTimestamp);
                });
            });

            var versionConverter = new VersionConverter();

            var data = new FeedbackSendData() {
                Kind = inputData.Kind.ToString().ToLowerInvariant(),
                Subject = inputData.Subject,
                Content = inputData.Content,

                Timestamp = DateTime.UtcNow.ToString("u"),
                Version = versionConverter.ConvertNormalVersion(BuildStatus.Version),
                Revision = BuildStatus.Revision,
                Build = BuildStatus.BuildType.ToString().ToLowerInvariant(),

                UserId = settingData.userId,
                FirstExecuteVersion = versionConverter.ConvertNormalVersion(settingData.firstVersion),
                FirstExecuteTimestamp = settingData.firstTimestamp.ToString("u"),

                Process = ProcessArchitecture.ApplicationArchitecture,
                Platform = ProcessArchitecture.PlatformArchitecture,
                Os = Environment.OSVersion.ToString(),
                Clr = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
            };

            var json = JsonSerializer.Serialize(data);
            Logger.LogDebug("json: {0}", json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            foreach(var counter in new Counter(5)) {
                try {
                    using var userAgent = UserAgentManager.CreateUserAgent();
                    var result = await userAgent.PostAsync(ApiConfiguration.FeedbackUri, content);
                    if(result.IsSuccessStatusCode) {
                        Logger.LogInformation("送信完了");
                        var rawResponse = await result.Content.ReadAsStringAsync();
                        var response = JsonSerializer.Deserialize<FeedbackResponse>(rawResponse);

                        if(response.Success) {
                            SendStatus.State = RunningState.End;
                            Logger.LogInformation("BODY: {0}", rawResponse);
                            return;
                        } else {
                            Logger.LogError(response.Message);
                            ErrorMessage = response.Message;
                            SendStatus.State = RunningState.Error;
                        }

                        return;
                    }
                    Logger.LogWarning("HTTP: {0}", result.StatusCode);
                    Logger.LogWarning("{0}", await result.Content.ReadAsStringAsync());
                } catch(Exception ex) {
                    Logger.LogWarning(ex, ex.Message);
                    if(!counter.IsLast) {
                        Logger.LogDebug("待機中: {0}", RetryWaitTime);
                        await Task.Delay(RetryWaitTime);
                    } else {
                        Logger.LogError(ex, ex.Message);
                        ErrorMessage = ex.Message;
                        SendStatus.State = RunningState.Error;
                    }
                }
            }

            return;
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
