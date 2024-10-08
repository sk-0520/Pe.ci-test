using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.StandardInputOutput;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
{
    public interface ILauncherExecuteResult: IResultFailure<Exception>
    {
        #region property

        LauncherItemKind Kind { get; }

        object? Data { get; }

        #endregion
    }

    internal sealed class LauncherExecuteErrorResult: ILauncherExecuteResult
    {
        public LauncherExecuteErrorResult(LauncherItemKind launcherItemKind, Exception exception)
        {
            Kind = launcherItemKind;
            FailureType = exception.GetType();
            FailureValue = exception;
        }

        #region ILauncherExecuteResult

        public object? Data { get; set; }

        public LauncherItemKind Kind { get; }

        public bool Success => false;

        public Type FailureType { get; }

        public Exception FailureValue { get; }

        #endregion
    }

    public sealed class LauncherFileExecuteResult: ILauncherExecuteResult
    {
        #region function

        public static LauncherFileExecuteResult Error(Exception ex)
        {
            return new LauncherFileExecuteResult() {
                Success = false,
                FailureType = ex.GetType(),
                FailureValue = ex,
            };
        }

        #endregion

        #region ILauncherExecuteResult

        public object? Data { get; set; }

        public LauncherItemKind Kind { get; set; }

        public Process? Process => (Process?)Data;

        public bool Success { get; set; }

        public Type? FailureType { get; set; }

        [AllowNull]
        public Exception FailureValue { get; set; }

        #endregion
    }

    public enum LauncherAddonExecuteKind
    {
        None,
        /// <summary>
        /// 実行。
        /// </summary>
        Execute,
        /// <summary>
        /// すでに実行中。
        /// </summary>
        Duplicate,
    }
    public sealed class LauncherAddonExecuteResult: ILauncherExecuteResult
    {
        #region function

        public static LauncherFileExecuteResult Error(Exception ex)
        {
            return new LauncherFileExecuteResult() {
                Success = false,
                FailureType = ex.GetType(),
                FailureValue = ex,
            };
        }

        #endregion

        #region ILauncherExecuteResult

        public object? Data { get; set; }
        public LauncherAddonExecuteKind ExecuteKind => Data != null ? (LauncherAddonExecuteKind)Data : LauncherAddonExecuteKind.None;

        public LauncherItemKind Kind { get; set; }

        public bool Success { get; set; }

        public Type? FailureType { get; set; }

        [AllowNull]
        public Exception FailureValue { get; set; }

        #endregion
    }

    public class LauncherExecutor
    {
        public LauncherExecutor(EnvironmentPathExecuteFileCache environmentPathExecuteFileCache, IOrderManager orderManager, INotifyManager notifyManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            OrderManager = orderManager;
            NotifyManager = notifyManager;
            DispatcherWrapper = dispatcherWrapper;
            EnvironmentPathExecuteFileCache = environmentPathExecuteFileCache;
        }

        #region property

        private IOrderManager OrderManager { get; }
        private INotifyManager NotifyManager { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }
        private EnvironmentPathExecuteFileCache EnvironmentPathExecuteFileCache { get; }

        #endregion

        #region function

        private async Task<ILauncherExecuteResult> ExecuteFilePathAsync(ILauncherExecutePathParameter pathParameter, ILauncherExecuteCustomParameter customParameter, IEnumerable<LauncherEnvironmentVariableData> environmentVariableItems, IReadOnlyLauncherRedoData redoData, IScreen screen, CancellationToken cancellationToken)
        {
            var process = new Process();
            var startInfo = process.StartInfo;

            // 実行パス
            startInfo.FileName = Environment.ExpandEnvironmentVariables(pathParameter.Path ?? string.Empty);
            startInfo.UseShellExecute = true;

            // 引数
            startInfo.Arguments = pathParameter.Option;

            // 管理者
            if(customParameter.RunAdministrator) {
                startInfo.Verb = "runas";
            }

            // 作業ディレクトリ
            if(!string.IsNullOrWhiteSpace(pathParameter.WorkDirectoryPath)) {
                startInfo.WorkingDirectory = Environment.ExpandEnvironmentVariables(pathParameter.WorkDirectoryPath);
            } else if(Path.IsPathRooted(startInfo.FileName) && IOUtility.Exists(startInfo.FileName)) {
                startInfo.WorkingDirectory = Path.GetDirectoryName(startInfo.FileName)!;
            }

            // 表示方法
            startInfo.WindowStyle = customParameter.ShowMode switch {
                ShowMode.Normal => ProcessWindowStyle.Normal,
                ShowMode.Hidden => ProcessWindowStyle.Hidden,
                ShowMode.Maximized => ProcessWindowStyle.Maximized,
                ShowMode.Minimized => ProcessWindowStyle.Minimized,
                _ => throw new Exception(customParameter.ShowMode.ToString()),
            };

            // 環境変数
            if(customParameter.IsEnabledCustomEnvironmentVariable) {
                startInfo.UseShellExecute = false;
                var envVars = startInfo.EnvironmentVariables;
                // 追加・更新
                foreach(var item in environmentVariableItems.Where(i => !i.IsRemove)) {
                    envVars[item.Name] = item.Value;
                }
                // 削除
                foreach(var item in environmentVariableItems.Where(i => i.IsRemove)) {
                    if(envVars.ContainsKey(item.Name)) {
                        envVars.Remove(item.Name);
                    }
                }
            }

            var streamWatch = false;
            // 出力取得
            //StreamForm streamForm = null;
            if(customParameter.IsEnabledStandardInputOutput) {
                streamWatch = true;
                process.EnableRaisingEvents = true;
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.RedirectStandardInput = true;
                startInfo.StandardOutputEncoding = customParameter.StandardInputOutputEncoding;
                startInfo.StandardErrorEncoding = customParameter.StandardInputOutputEncoding;
                startInfo.StandardInputEncoding = customParameter.StandardInputOutputEncoding;
            }

            var result = new LauncherFileExecuteResult() {
                Kind = LauncherItemKind.File,
                Data = process,
            };
            RedoExecutor? redoExecutor = null;
            if(redoData.RedoMode != RedoMode.None) {
                redoExecutor = new RedoExecutor(
                    new LauncherExecutor(EnvironmentPathExecuteFileCache, OrderManager, NotifyManager, DispatcherWrapper, LoggerFactory),
                    result,
                    new RedoParameter(
                        pathParameter,
                        customParameter,
                        environmentVariableItems as IReadOnlyCollection<LauncherEnvironmentVariableData> ?? environmentVariableItems.ToList(),
                        redoData,
                        screen
                    ),
                    NotifyManager,
                    LoggerFactory
                );
            }

            StandardInputOutputElement? stdIoElement = null;
            if(streamWatch) {
                process.EnableRaisingEvents = true;
                stdIoElement = await OrderManager.CreateStandardInputOutputElementAsync(customParameter.Caption, process, screen, cancellationToken);
                //DispatcherWrapper.BeginAsync(element => {
                //    element.StartView();
                //    element!.PreparateReceiver();
                //}, stdioElement, DispatcherPriority.Send);
            }

            result.Success = process.Start();

            if(redoExecutor != null) {
                OrderManager.AddRedoItem(redoExecutor);
            }

            if(streamWatch) {
                Debug.Assert(stdIoElement != null);
                // 受信前に他の処理を終わらせるため少し待つ
                await DispatcherWrapper.BeginAsync(element => {
                    element.StartView();
                    element.PreparateReceiver();
                    if(element.PreparedReceive) {
                        element.RunReceiver();
                    } else {
                        Logger.LogError("受信準備完了せず");
                    }
                }, stdIoElement, DispatcherPriority.Send);
            }

            return result;
        }

        public Task<ILauncherExecuteResult> ExecuteAsync(LauncherItemKind kind, ILauncherExecutePathParameter pathParameter, ILauncherExecuteCustomParameter customParameter, IReadOnlyCollection<LauncherEnvironmentVariableData> environmentVariableItems, IReadOnlyLauncherRedoData redoData, IScreen screen, CancellationToken cancellationToken)
        {
            if(pathParameter == null) {
                throw new ArgumentNullException(nameof(pathParameter));
            }
            if(customParameter == null) {
                throw new ArgumentNullException(nameof(customParameter));
            }
            if(environmentVariableItems == null) {
                throw new ArgumentNullException(nameof(environmentVariableItems));
            }

            switch(kind) {
                case LauncherItemKind.File:
                    return ExecuteFilePathAsync(pathParameter, customParameter, environmentVariableItems, redoData, screen, cancellationToken);

                default:
                    throw new NotImplementedException();
            }
        }

        public ILauncherExecuteResult OpenParentDirectory(LauncherItemKind kind, ILauncherExecutePathParameter pathParameter)
        {
            Debug.Assert(kind == LauncherItemKind.File);

            if(pathParameter == null) {
                throw new ArgumentNullException(nameof(pathParameter));
            }

            var path = Environment.ExpandEnvironmentVariables(pathParameter.Path ?? string.Empty);
            var fullPath = EnvironmentPathExecuteFileCache.ToFullPathIfExistsCommand(path, LoggerFactory);
            try {
                var systemExecutor = new SystemExecutor();
                var process = systemExecutor.OpenDirectoryWithFileSelect(fullPath);
                var result = new LauncherFileExecuteResult() {
                    Kind = kind,
                    Data = process,
                };

                return result;
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                return LauncherFileExecuteResult.Error(ex);
            }
        }

        public ILauncherExecuteResult OpenWorkingDirectory(LauncherItemKind kind, ILauncherExecutePathParameter pathParameter)
        {
            Debug.Assert(kind == LauncherItemKind.File);

            if(pathParameter == null) {
                throw new ArgumentNullException(nameof(pathParameter));
            }

            var path = Environment.ExpandEnvironmentVariables(pathParameter.WorkDirectoryPath ?? string.Empty);
            var fullPath = EnvironmentPathExecuteFileCache.ToFullPathIfExistsCommand(path, LoggerFactory);
            try {
                var systemExecutor = new SystemExecutor();
                var process = systemExecutor.ExecuteFile(fullPath);
                var result = new LauncherFileExecuteResult() {
                    Kind = kind,
                    Data = process,
                };

                return result;
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                return LauncherFileExecuteResult.Error(ex);
            }
        }

        public void ShowProperty(ILauncherExecutePathParameter pathParameter)
        {
            if(pathParameter == null) {
                throw new ArgumentNullException(nameof(pathParameter));
            }

            var path = Environment.ExpandEnvironmentVariables(pathParameter.Path ?? string.Empty);
            var fullPath = EnvironmentPathExecuteFileCache.ToFullPathIfExistsCommand(path, LoggerFactory);
            var systemExecutor = new SystemExecutor();
            systemExecutor.ShowProperty(fullPath);
        }

        #endregion
    }
}
