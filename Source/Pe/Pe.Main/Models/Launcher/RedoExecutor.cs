using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Timers;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
{
    public class RedoParameter
    {
        public RedoParameter(ILauncherExecutePathParameter path, ILauncherExecuteCustomParameter custom, IReadOnlyCollection<LauncherEnvironmentVariableData> environmentVariableItems, IReadOnlyLauncherRedoData redoData, IScreen screen)
        {
            Path = path;
            Custom = custom;
            EnvironmentVariableItems = environmentVariableItems;
            RedoData = redoData;
            Screen = screen;
        }

        #region property

        public ILauncherExecutePathParameter Path { get; }
        public ILauncherExecuteCustomParameter Custom { get; }
        public IReadOnlyCollection<LauncherEnvironmentVariableData> EnvironmentVariableItems { get; }
        public IReadOnlyLauncherRedoData RedoData { get; }
        public IScreen Screen { get; }

        #endregion
    }

    public class RedoExecutor: DisposerBase
    {
        #region event

        public event EventHandler? Exited;

        #endregion

        #region variable

        string? _notifyLogHeader;

        #endregion

        public RedoExecutor(LauncherExecutor executor, ILauncherExecuteResult firstResult, RedoParameter parameter, INotifyManager notifyManager, ILoggerFactory loggerFactory)
        {
            if(firstResult.Process == null) {
                throw new ArgumentException($"{nameof(firstResult)}.{nameof(firstResult.Process)}");
            }
            if(parameter.RedoData.RedoMode == RedoMode.None) {
                throw new ArgumentException($"{nameof(parameter)}.{nameof(parameter.RedoData)}.{nameof(parameter.RedoData.RedoMode)}");
            }

            Logger = loggerFactory.CreateLogger(GetType());

            Executor = executor;
            FirstResult = firstResult;
            Parameter = parameter;
            NotifyManager = notifyManager;

            if(Parameter.RedoData.RedoMode == RedoMode.Timeout || Parameter.RedoData.RedoMode == RedoMode.TimeoutOrCount) {
                Stopwatch = Stopwatch.StartNew();
                WaitEndTimer = new Timer() {
                    Interval = (int)(Parameter.RedoData.WaitTime + Stopwatch.Elapsed).TotalMilliseconds,
                    AutoReset = false,
                };
                WaitEndTimer.Elapsed += WaitEndTimer_Elapsed;
                WaitEndTimer.Start();
            }

            Watching(FirstResult.Process, false);
        }


        #region property

        ILogger Logger { get; }

        Guid NotifyLogId { get; set; }

        ILauncherExecuteResult FirstResult { get; }
        LauncherExecutor Executor { get; }
        RedoParameter Parameter { get; }

        INotifyManager NotifyManager { get; }

        Stopwatch? Stopwatch { get; }

        Process? CurrentProcess { get; set; }
        Timer? WaitEndTimer { get; set; }

        int RetryCount { get; set; }

        public bool IsExited { get; private set; }

        string NotifyLogHeader
        {
            get
            {
                return this._notifyLogHeader ??= TextUtility.ReplaceFromDictionary(
                    Properties.Resources.String_RedoExecutor_Caption_Format,
                    new Dictionary<string, string>() {
                        ["CAPTION"] = Parameter.Custom.Caption
                    }
                );
            }
        }

        #endregion

        #region function

        void PutNotifyLog(bool isComplete, string message)
        {
            if(isComplete) {
                if(NotifyLogId != Guid.Empty) {
                    NotifyManager.ClearLog(NotifyLogId);
                }
                NotifyLogId = NotifyManager.AppendLog(new NotifyMessage(NotifyLogKind.Normal, NotifyLogHeader, new NotifyLogContent(message)));
            } else {
                if(NotifyLogId == Guid.Empty) {
                    NotifyLogId = NotifyManager.AppendLog(new NotifyMessage(NotifyLogKind.Command, NotifyLogHeader, new NotifyLogContent(message), StopWatch));
                } else {
                    if(NotifyManager.ExistsLog(NotifyLogId)) {
                        NotifyManager.ReplaceLog(NotifyLogId, message);
                    } else {
                        NotifyLogId = NotifyManager.AppendLog(new NotifyMessage(NotifyLogKind.Command, NotifyLogHeader, new NotifyLogContent(message), StopWatch));
                    }
                }
            }
        }


        bool IsTimeout() => Stopwatch != null && Parameter.RedoData.WaitTime < Stopwatch.Elapsed;
        bool IsMaxRetry() => Parameter.RedoData.RetryCount <= RetryCount;

        void OnExited()
        {
            IsExited = true;
            Exited?.Invoke(this, EventArgs.Empty);

            //NotifyManager.FadeoutLog(NotifyLogId);

            Dispose();
        }

        void StopWatch()
        {
            Logger.LogInformation("{0}: 再試行中断", Parameter.Custom.Caption);
            Dispose();
        }


        void Watching(Process process, bool isContinue)
        {
            CurrentProcess = process;
            CurrentProcess.EnableRaisingEvents = true;

            if(isContinue && CurrentProcess.HasExited) {
                if(Check(CurrentProcess)) {
                    Execute();
                } else {
                    OnExited();
                }
            } else {
                CurrentProcess.Exited += Process_Exited;
            }
        }

        /// <summary>
        /// 再試行が可能か。
        /// </summary>
        /// <param name="process"></param>
        /// <returns>真: 再試行が可能。</returns>
        bool Check(Process process)
        {
            if(!process.HasExited) {
                Logger.LogWarning("到達不可");
                return false;
            }

            if(Parameter.RedoData.SuccessExitCodes.Any(i => i == process.ExitCode)) {
                Logger.LogInformation("正常終了コードのため再試行不要: {0}", process.ExitCode);
                if(NotifyLogId != Guid.Empty) {
                    PutNotifyLog(true, Properties.Resources.String_RedoExecutor_SuccessExit);
                }

                return false;
            }

            switch(Parameter.RedoData.RedoMode) {
                case RedoMode.Timeout:
                    if(IsTimeout()) {
                        PutNotifyLog(true, Properties.Resources.String_RedoExecutor_Timeout);
                        Logger.LogInformation("タイムアウト");
                        return false;
                    }
                    break;

                case RedoMode.Count:
                    if(IsMaxRetry()) {
                        PutNotifyLog(true, Properties.Resources.String_RedoExecutor_CountMax);
                        Logger.LogInformation("試行回数超過");
                        return false;
                    }
                    break;

                case RedoMode.TimeoutOrCount:
                    if(IsTimeout() || IsMaxRetry()) {
                        PutNotifyLog(true, Properties.Resources.String_RedoExecutor_TimeoutOrCountMax);
                        Logger.LogInformation("タイムアウト/試行回数超過");
                        return false;
                    }
                    break;

                case RedoMode.None:
                default:
                    throw new NotImplementedException();
            }

            Logger.LogTrace("再実施可能");

            return true;
        }

        string CreateRedoNotifyLogMessage()
        {
            //var message = "@再試行";
            var message = Parameter.RedoData.RedoMode switch
            {
                RedoMode.Timeout => TextUtility.ReplaceFromDictionary(
                    Properties.Resources.String_RedoExecutor_Retry_Timout_Format,
                    new Dictionary<string, string>() {
                        ["NOW-TIME"] = Stopwatch!.Elapsed.ToString(),
                        ["MAX-TIME"] = Parameter.RedoData.WaitTime.ToString(),
                    }
                ),
                RedoMode.Count => TextUtility.ReplaceFromDictionary(
                    Properties.Resources.String_RedoExecutor_Retry_CountMax_Format,
                    new Dictionary<string, string>() {
                        ["NOW-COUNT"] = RetryCount.ToString(),
                        ["MAX-COUNT"] = Parameter.RedoData.RetryCount.ToString(),
                    }
                ),
                RedoMode.TimeoutOrCount => TextUtility.ReplaceFromDictionary(
                    Properties.Resources.String_RedoExecutor_Retry_TimeoutOrCountMax_Format,
                    new Dictionary<string, string>() {
                        ["NOW-TIME"] = Stopwatch!.Elapsed.ToString(),
                        ["MAX-TIME"] = Parameter.RedoData.WaitTime.ToString(),
                        ["NOW-COUNT"] = RetryCount.ToString(),
                        ["MAX-COUNT"] = Parameter.RedoData.RetryCount.ToString(),
                    }
                ),
                _ => throw new NotImplementedException(),
            };

            return TextUtility.ReplaceFromDictionary(
                Properties.Resources.String_RedoExecutor_Cancel_Format,
                new Dictionary<string, string>() {
                    ["MESSAGE"] = message,
                }
            );
        }

        void Execute()
        {
            PutNotifyLog(false, CreateRedoNotifyLogMessage());

            var result = Executor.Execute(FirstResult.Kind, Parameter.Path, Parameter.Custom, Parameter.EnvironmentVariableItems, LauncherRedoData.GetDisable(), Parameter.Screen);
            RetryCount += 1;
            Watching(result.Process!, true);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(WaitEndTimer != null) {
                    WaitEndTimer.Elapsed -= WaitEndTimer_Elapsed;
                    if(disposing) {
                        WaitEndTimer.Dispose();
                    }
                }

                if(CurrentProcess != null) {
                    CurrentProcess.Exited -= Process_Exited;
                }

            }

            base.Dispose(disposing);
        }

        #endregion

        private void Process_Exited(object? sender, EventArgs e)
        {
            Debug.Assert(CurrentProcess != null);

            CurrentProcess.Exited -= Process_Exited;

            if(Check(CurrentProcess)) {
                Execute();
            } else {
                OnExited();
            }
        }

        private void WaitEndTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Debug.Assert(CurrentProcess != null);
            Debug.Assert(WaitEndTimer != null);

            if(NotifyLogId != Guid.Empty) {
                PutNotifyLog(true, Properties.Resources.String_RedoExecutor_CancelWatch);
            }

            OnExited();
        }
    }
}
