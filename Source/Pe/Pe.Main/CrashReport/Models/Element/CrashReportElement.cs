using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.CrashReport.Models.Data;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Element;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Models.Element
{
    internal class CrashReportElement : ElementBase
    {
        public CrashReportElement(CrashReportOptions options, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Options = options;
            SendStatus = new RunningStatus(LoggerFactory);
        }

        #region property

        TimeSpan RetryWaitTime { get; } = TimeSpan.FromSeconds(5);
        CrashReportOptions Options { get; }
        public CrashReportSaveData Data { get; set; } = new CrashReportSaveData();

        public bool AutoSend => Options.AutoSend;
        public string CrashReportRawFilePath => Environment.ExpandEnvironmentVariables(Options.CrashReportRawFilePath);


        public IReadOnlyList<ObjectDumpItem> RawProperties { get; private set; } = new List<ObjectDumpItem>();

        public RunningStatus SendStatus { get; }

        #endregion

        #region function

        public async Task SendAsync()
        {
            SendStatus.State = RunningState.Running;

            // 入力内容をファイルに保存及び、該当ファイルを転送する
            var path = Environment.ExpandEnvironmentVariables(Options.CrashReportSaveFilePath);
            FileUtility.MakeFileParentDirectory(path);
            var jsonValue = JsonSerializer.Serialize(Data, Data.GetType());
            using(var writer = new StreamWriter(new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read), Encoding.UTF8)) {
                await writer.WriteAsync(jsonValue);
            }

            var map = new Dictionary<string, string>() {
                ["report"] = jsonValue,
            };
            var encoding = Encoding.UTF8;
            var items = map
                .Select(p => new { Key = HttpUtility.UrlEncode(p.Key, encoding), Value = HttpUtility.UrlEncode(p.Value, encoding) })
                .Select(i => $"{i.Key}={i.Value}");
            ;
            var param = string.Join("&", items);
            var content = new StringContent(param, encoding, "application/x-www-form-urlencoded");


            foreach(var counter in new Counter(5)) {
                Logger.LogInformation("post {0}/{1}", counter.CurrentCount, counter.MaxCount);
                // 失敗時にリフレッシュしたいので毎回生成する
                try {
                    using(var httpClient = new HttpClient()) {
                        var result = await httpClient.PostAsync(Options.PostUri, content);
                        if(result.IsSuccessStatusCode) {
                            Logger.LogInformation("送信完了");
                            SendStatus.State = RunningState.End;
                            return;
                        }
                        Logger.LogWarning("HTTP: {0}", result.StatusCode);
                        Logger.LogWarning("{0}", await result.Content.ReadAsStringAsync());
                    }
                } catch(Exception ex) {
                    Logger.LogWarning(ex, ex.Message);
                    if(!counter.IsLast) {
                        Logger.LogDebug("待機中: {0}", RetryWaitTime);
                        await Task.Delay(RetryWaitTime);
                    }
                }
            }
            SendStatus.State = RunningState.Error;
        }

        public void Reboot()
        {
            var systemExecutor = new SystemExecutor();
            Logger.LogInformation("App path: {0}", Options.ExecuteCommand);
            Logger.LogInformation("App args: {0}", Options.ExecuteArgument);
            systemExecutor.ExecuteFile(Options.ExecuteCommand, Options.ExecuteArgument);
        }

        public void Cancel()
        {
            SendStatus.State = RunningState.None;
        }

        CrashReportRawData LoadRawData()
        {
            using var stream = new FileStream(Options.CrashReportRawFilePath, FileMode.Open);
            var serializer = new BinaryDataContractSerializer();
            return serializer.Load<CrashReportRawData>(stream);
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            var rawData = LoadRawData();

            var dumper = new ObjectDumper();
            RawProperties = dumper.Dump(rawData);

            Data = new CrashReportSaveData() {
                UserId = rawData.UserId,
                Version = rawData.Version,
                Revision = rawData.Revision,
                Timestamp = rawData.Timestamp,
                Exception = rawData.Exception,
                Informations = rawData.Informations,
                LogItems = rawData.LogItems,
            };
        }

        #endregion
    }
}
