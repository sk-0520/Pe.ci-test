using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Library.Base;
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

        private string? _notifyLogHeader;

        #endregion

        public RedoExecutor(LauncherExecutor executor, LauncherFileExecuteResult firstResult, RedoParameter parameter, INotifyManager notifyManager, ILoggerFactory loggerFactory)
        {
            if(firstResult.Data == null) {
                throw new ArgumentException(null, $"{nameof(firstResult)}.{nameof(firstResult.Data)}");
            }
            if(parameter.RedoData.RedoMode == RedoMode.None) {
                throw new ArgumentException(null, $"{nameof(parameter)}.{nameof(parameter.RedoData)}.{nameof(parameter.RedoData.RedoMode)}");
            }

            Logger = loggerFactory.CreateLogger(GetType());

            Executor = executor;
            FirstResult = firstResult;
            Parameter = parameter;
            NotifyManager = notifyManager;

            if(Parameter.RedoData.RedoMode == RedoMode.Timeout || Parameter.RedoData.RedoMode == RedoMode.TimeoutOrCount) {
                Stopwatch = Stopwatch.StartNew();
                WaitEndTimer = new System.Timers.Timer() {
                    Interval = (int)(Parameter.RedoData.WaitTime + Stopwatch.Elapsed).TotalMilliseconds,
                    AutoReset = false,
                };
                WaitEndTimer.Elapsed += WaitEndTimer_Elapsed;
                WaitEndTimer.Start();
            }
            if(FirstResult.Process != null) {
                _ = WatchingAsync(FirstResult.Process, false, CancellationToken.None).ConfigureAwait(false);
            }
        }

        #region property

        private ILogger Logger { get; }

        private NotifyLogId NotifyLogId { get; set; }

        private LauncherFileExecuteResult FirstResult { get; }
        private LauncherExecutor Executor { get; }
        private RedoParameter Parameter { get; }

        private INotifyManager NotifyManager { get; }

        private Stopwatch? Stopwatch { get; }

        private Process? CurrentProcess { get; set; }
        private System.Timers.Timer? WaitEndTimer { get; set; }

        private int RetryCount { get; set; }

        public bool IsExited { get; private set; }

        private string NotifyLogHeader
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

        private async Task PutNotifyLogAsync(bool isComplete, string message, CancellationToken cancellationToken)
        {
            if(isComplete) {
                if(NotifyLogId != NotifyLogId.Empty) {
                    NotifyManager.ClearLog(NotifyLogId);
                }
                NotifyLogId = await NotifyManager.AppendLogAsync(new NotifyMessage(NotifyLogKind.Normal, NotifyLogHeader, new NotifyLogContent(message)), cancellationToken);
            } else {
                if(NotifyLogId == NotifyLogId.Empty) {
                    NotifyLogId = await NotifyManager.AppendLogAsync(new NotifyMessage(NotifyLogKind.Command, NotifyLogHeader, new NotifyLogContent(message), StopWatch), cancellationToken);
                } else {
                    if(NotifyManager.ExistsLog(NotifyLogId)) {
                        NotifyManager.ReplaceLog(NotifyLogId, message);
                    } else {
                        NotifyLogId = await NotifyManager.AppendLogAsync(new NotifyMessage(NotifyLogKind.Command, NotifyLogHeader, new NotifyLogContent(message), StopWatch), cancellationToken);
                    }
                }
            }
        }

        private bool IsTimeout() => Stopwatch != null && Parameter.RedoData.WaitTime < Stopwatch.Elapsed;
        private bool IsMaxRetry() => Parameter.RedoData.RetryCount <= RetryCount;

        private void OnExited()
        {
            IsExited = true;
            Exited?.Invoke(this, EventArgs.Empty);

            //NotifyManager.FadeoutLog(NotifyLogId);

            Dispose();
        }

        private void StopWatch()
        {
            Logger.LogInformation("{0}: 再試行中断", Parameter.Custom.Caption);
            Dispose();
        }

        private async Task WatchingAsync(Process process, bool isContinue, CancellationToken cancellationToken)
        {
            CurrentProcess = process;
            CurrentProcess.EnableRaisingEvents = true;

            if(isContinue && CurrentProcess.HasExited) {
                if(await CheckAsync(CurrentProcess, cancellationToken)) {
                    await ExecuteAsync(cancellationToken);
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
        private async Task<bool> CheckAsync(Process process, CancellationToken cancellationToken)
        {
            if(!process.HasExited) {
                Logger.LogWarning("到達不可");
                return false;
            }

            if(Parameter.RedoData.SuccessExitCodes.Any(i => i == process.ExitCode)) {
                Logger.LogInformation("正常終了コードのため再試行不要: {0}", process.ExitCode);
                if(NotifyLogId != NotifyLogId.Empty) {
                    await PutNotifyLogAsync(true, Properties.Resources.String_RedoExecutor_SuccessExit, cancellationToken);
                }

                return false;
            }

            switch(Parameter.RedoData.RedoMode) {
                case RedoMode.Timeout:
                    if(IsTimeout()) {
                        await PutNotifyLogAsync(true, Properties.Resources.String_RedoExecutor_Timeout, cancellationToken);
                        Logger.LogInformation("タイムアウト");
                        return false;
                    }
                    break;

                case RedoMode.Count:
                    if(IsMaxRetry()) {
                        await PutNotifyLogAsync(true, Properties.Resources.String_RedoExecutor_CountMax, cancellationToken);
                        Logger.LogInformation("試行回数超過");
                        return false;
                    }
                    break;

                case RedoMode.TimeoutOrCount:
                    if(IsTimeout() || IsMaxRetry()) {
                        await PutNotifyLogAsync(true, Properties.Resources.String_RedoExecutor_TimeoutOrCountMax, cancellationToken);
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

        private string CreateRedoNotifyLogMessage()
        {
            //var message = "@再試行";
            var message = Parameter.RedoData.RedoMode switch {
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
                        ["NOW-COUNT"] = (RetryCount + 1).ToString(CultureInfo.InvariantCulture),
                        ["MAX-COUNT"] = Parameter.RedoData.RetryCount.ToString(CultureInfo.InvariantCulture),
                    }
                ),
                RedoMode.TimeoutOrCount => TextUtility.ReplaceFromDictionary(
                    Properties.Resources.String_RedoExecutor_Retry_TimeoutOrCountMax_Format,
                    new Dictionary<string, string>() {
                        ["NOW-TIME"] = Stopwatch!.Elapsed.ToString(),
                        ["MAX-TIME"] = Parameter.RedoData.WaitTime.ToString(),
                        ["NOW-COUNT"] = (RetryCount + 1).ToString(CultureInfo.InvariantCulture),
                        ["MAX-COUNT"] = Parameter.RedoData.RetryCount.ToString(CultureInfo.InvariantCulture),
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

        private async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await PutNotifyLogAsync(false, CreateRedoNotifyLogMessage(),cancellationToken);

            var result = (LauncherFileExecuteResult) await Executor.ExecuteAsync(FirstResult.Kind, Parameter.Path, Parameter.Custom, Parameter.EnvironmentVariableItems, LauncherRedoData.GetDisable(), Parameter.Screen, cancellationToken);
            RetryCount += 1;
            await WatchingAsync(result.Process!, true, cancellationToken);
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "<保留中>")]
        private async void Process_Exited(object? sender, EventArgs e)
        {
            Debug.Assert(CurrentProcess != null);

            CurrentProcess.Exited -= Process_Exited;

            if(await CheckAsync(CurrentProcess, CancellationToken.None)) {
                await ExecuteAsync(CancellationToken.None);
            } else {
                OnExited();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "<保留中>")]
        private async void WaitEndTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Debug.Assert(CurrentProcess != null);
            Debug.Assert(WaitEndTimer != null);

            if(NotifyLogId != NotifyLogId.Empty) {
                await PutNotifyLogAsync(true, Properties.Resources.String_RedoExecutor_CancelWatch, CancellationToken.None);
            }

            OnExited();
        }
    }
}
