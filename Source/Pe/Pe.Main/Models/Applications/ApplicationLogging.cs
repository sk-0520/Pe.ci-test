using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;
using NLog;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public class ApplicationLogging
    {
        public ApplicationLogging(int logLimit, string logginConfigFilePath, string outputPath, string withLog, bool createDirectory, bool isFullTrace)
        {
            LogItems = new ConcurrentFixedQueue<LogEventInfo>(logLimit);

            Factory = new LoggerFactory();
            LogManager.LoadConfiguration(logginConfigFilePath);

            var op = new NLog.Extensions.Logging.NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true };
            var prov = new NLog.Extensions.Logging.NLogLoggerProvider(op, LogManager.LogFactory);
            Factory.AddProvider(prov);

            var appTarget = new NLog.Targets.MethodCallTarget("APPLOG", ReceiveLog);
            LogManager.Configuration.AddTarget(appTarget);

            var logger = Factory.CreateLogger(GetType());
            logger.LogInformation("ログ出力開始");

            var enabledLog = new HashSet<string>();

            // ログ出力(ファイル・ディレクトリが存在しなければ終了で構わない)
            if(!string.IsNullOrWhiteSpace(outputPath)) {
                var expandedOutputPath = Environment.ExpandEnvironmentVariables(outputPath);
                if(createDirectory) {
                    var fileName = Path.GetFileName(expandedOutputPath);
                    if(!string.IsNullOrEmpty(fileName) && fileName.IndexOf('.') == -1) {
                        // 拡張子がなければディレクトリ指定と決めつけ
                        Directory.CreateDirectory(expandedOutputPath);
                    } else {
                        var parentDir = Path.GetDirectoryName(expandedOutputPath);
                        if(!string.IsNullOrEmpty(parentDir)) {
                            Directory.CreateDirectory(parentDir);
                        }
                    }
                }

                // ディレクトリ指定であればタイムスタンプ付きでファイル生成(プレーンログ)
                var filePath = expandedOutputPath;
                if(Directory.Exists(expandedOutputPath)) {
                    var fileName = PathUtility.AppendExtension(DateTime.Now.ToString("yyyy-MM-dd_HHmmss"), "log");
                    filePath = Path.Combine(expandedOutputPath, fileName);
                }

                //TODO: なんかうまいことする
                switch(Path.GetExtension(filePath)?.ToLowerInvariant() ?? string.Empty) {
                    case ".log":
                        LogManager.LogFactory.Configuration.Variables.Add("logPath", filePath);
                        enabledLog.Add("log");
                        switch(withLog) {
                            case "xml":
                                LogManager.LogFactory.Configuration.Variables.Add("xmlPath", Path.ChangeExtension(filePath, "xml"));
                                enabledLog.Add("xml");
                                break;
                        }
                        break;

                    case ".xml":
                        LogManager.LogFactory.Configuration.Variables.Add("xmlPath", filePath);
                        enabledLog.Add("xml");
                        switch(withLog) {
                            case "log":
                                LogManager.LogFactory.Configuration.Variables.Add("logPath", Path.ChangeExtension(filePath, "log"));
                                enabledLog.Add("log");
                                break;
                        }
                        break;
                }
                LogManager.LogFactory.Configuration.Variables.Add("dirPath", Path.GetDirectoryName(filePath));
            }

            var traceTargets = enabledLog
                .Select(i => LogManager.Configuration.FindTargetByName(i))
                .ToList()
            ;


            foreach(var loggingRule in LogManager.Configuration.LoggingRules) {
                if(isFullTrace) {
                    if(loggingRule.RuleName == "fulltrace") {
                        foreach(var traceTarget in traceTargets) {
                            loggingRule.Targets.Add(traceTarget);
                        }
                    }
                } else {
                    if(loggingRule.RuleName != "fulltrace") {
                        foreach(var traceTarget in traceTargets) {
                            loggingRule.Targets.Add(traceTarget);
                        }
                    }
                }
            }
            foreach(var loggingRule in LogManager.Configuration.LoggingRules.Where(i => i.RuleName == "fulltrace")) {
                loggingRule.Targets.Insert(0, appTarget);
            }

            if(traceTargets.Any()) {
                var stopwatch = Stopwatch.StartNew();
                LogManager.ReconfigExistingLoggers();
                LogManager.Flush();
                //LogManager.GetCurrentClassLogger();
                logger = Factory.CreateLogger(GetType());
                if(isFullTrace) {
                    logger.LogInformation("全データ出力: {0}", stopwatch.Elapsed);
                } else {
                    logger.LogInformation("データ出力: {0}", stopwatch.Elapsed);
                }
                foreach(var traceTarget in traceTargets) {
                    logger.LogInformation("{0}", traceTarget);
                }
            }
        }

        #region property

        private IFixedQueue<LogEventInfo> LogItems { get; }

        public LoggerFactory Factory { get; }

        #endregion

        #region function

        public void ReceiveLog(LogEventInfo logEventInfo, object[] parameters)
        {
            LogItems.Enqueue(logEventInfo);
        }

        public IReadOnlyList<LogEventInfo> GetLogItems() => LogItems.ToArray();

        #endregion
    }
}
