using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Main.Models.WebView;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Bridge.Models;
using System.Threading;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Feedback
{
    public class FeedbackElement: ElementBase
    {
        public FeedbackElement(ApiConfiguration apiConfiguration, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ICultureService cultureService, IOrderManager orderManager, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            ApiConfiguration = apiConfiguration;
            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            OrderManager = orderManager;
            CultureService = cultureService;
            UserAgentManager = userAgentManager;

            SendStatus = new RunningStatus(LoggerFactory);
        }

        #region property

        private ApiConfiguration ApiConfiguration { get; }
        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }
        private IOrderManager OrderManager { get; }
        private ICultureService CultureService { get; }
        IUserAgentManager UserAgentManager { get; }

        private TimeSpan RetryWaitTime { get; } = TimeSpan.FromSeconds(5);
        public RunningStatus SendStatus { get; }
        public string ErrorMessage { get; private set; } = string.Empty;

        #endregion

        #region function

        public void ShowSourceUri()
        {
            var systemExecutor = new SystemExecutor();
            try {
                systemExecutor.OpenUri(ApiConfiguration.FeedbackSourceUri);
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
        }

        public void Cancel()
        {
            SendStatus.State = RunningState.None;
        }

        public async Task SendAsync(FeedbackInputData inputData, CancellationToken cancellationToken)
        {
            ErrorMessage = string.Empty;
            SendStatus.State = RunningState.Running;

            var settingData = await Task.Run(() => {
                return MainDatabaseBarrier.ReadData(c => {
                    var appExecuteSettingEntityDao = new AppExecuteSettingEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                    //var appGeneralSettingEntityDao = new AppGeneralSettingEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);

                    var userIdManager = new UserIdManager(LoggerFactory);
                    var userId = userIdManager.SafeGetOrCreateUserId(appExecuteSettingEntityDao);

                    var firstData = appExecuteSettingEntityDao.SelectFirstData();

                    return (userId, firstVersion: firstData.FirstExecuteVersion, firstTimestamp: firstData.FirstExecuteTimestamp);
                });
            });

            var versionConverter = new VersionConverter();

            var data = new FeedbackSendData() {
                Kind = EnumUtility.ToString(inputData.Kind),
                Subject = inputData.Subject,
                Content = inputData.Content,

                Version = versionConverter.ConvertNormalVersion(BuildStatus.Version),
                Revision = BuildStatus.Revision,
                Build = EnumUtility.ToString(BuildStatus.BuildType),

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
                    var result = await userAgent.PostAsync(ApiConfiguration.FeedbackUri, content, cancellationToken);
                    if(result.IsSuccessStatusCode) {
                        Logger.LogInformation("送信完了");
                        var rawResponse = await result.Content.ReadAsStringAsync(cancellationToken);
                        var response = JsonSerializer.Deserialize<FeedbackResponse>(rawResponse);

                        if(response != null && response.Success) {
                            SendStatus.State = RunningState.End;
                            Logger.LogInformation("BODY: {0}", rawResponse);
                            return;
                        } else {
                            var msg = response?.Message ?? Properties.Resources.String_Common_Network_UnknownResponse;
                            Logger.LogError("{0}", msg);
                            ErrorMessage = msg;
                            SendStatus.State = RunningState.Error;
                        }

                        return;
                    }
                    Logger.LogWarning("HTTP: {0}", result.StatusCode);
                    Logger.LogWarning("{0}", await result.Content.ReadAsStringAsync(cancellationToken));
                    if(!result.IsSuccessStatusCode && counter.IsLast) {
                        ErrorMessage = result.StatusCode.ToString();
                        SendStatus.State = RunningState.Error;
                    }
                } catch(Exception ex) {
                    Logger.LogWarning(ex, ex.Message);
                    if(!counter.IsLast) {
                        Logger.LogDebug("待機中: {0}", RetryWaitTime);
                        await Task.Delay(RetryWaitTime, cancellationToken);
                    } else {
                        Logger.LogError(ex, ex.Message);
                        ErrorMessage = ex.Message;
                        SendStatus.State = RunningState.Error;
                    }
                }
            }

            return;
        }

        #endregion

        #region WebViewElementBase

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
