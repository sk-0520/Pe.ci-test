using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.StandardInputOutput;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
{
    public interface ILauncherExecuteResult : IResultFailureValue<Exception>
    {
        #region property

        LauncherItemKind Kind { get; }

        Process? Process { get; }

        #endregion
    }

    public class LauncherExecuteResult : ILauncherExecuteResult
    {
        #region function

        public static LauncherExecuteResult Error(Exception ex)
        {
            return new LauncherExecuteResult() {
                Success = false,
                FailureType = ex.GetType(),
                FailureValue = ex,
            };
        }

        #endregion

        #region ILauncherExecuteResult

        public LauncherItemKind Kind { get; set; }

        public Process? Process { get; set; }

        public bool Success { get; set; }

        public Type? FailureType { get; set; }

        public Exception? FailureValue { get; set; }

        #endregion
    }


    public class LauncherExecutor
    {
        public LauncherExecutor(IOrderManager orderManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            OrderManager = orderManager;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        IOrderManager OrderManager { get; }
        IDispatcherWrapper DispatcherWrapper { get; }
        ILogger Logger { get; }

        #endregion

        #region function

        ILauncherExecuteResult ExecuteFilePath(LauncherItemKind kind, ILauncherExecutePathParameter pathParameter, ILauncherExecuteCustomParameter customParameter, IEnumerable<LauncherEnvironmentVariableData> environmentVariableItems, IScreen screen)
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
            } else if(Path.IsPathRooted(startInfo.FileName) && FileUtility.Exists(startInfo.FileName)) {
                startInfo.WorkingDirectory = Path.GetDirectoryName(startInfo.FileName);
            }

            // 環境変数
            if(customParameter.IsEnabledCustomEnvironmentVariable) {
                startInfo.UseShellExecute = false;
                var envs = startInfo.EnvironmentVariables;
                // 追加・更新
                foreach(var item in environmentVariableItems.Where(i => !i.IsRemove)) {
                    envs[item.Name] = item.Value;
                }
                // 削除
                foreach(var item in environmentVariableItems.Where(i => i.IsRemove)) {
                    if(envs.ContainsKey(item.Name)) {
                        envs.Remove(item.Name);
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

            var result = new LauncherExecuteResult() {
                Kind = kind,
                Process = process,
            };

            StandardInputOutputElement? stdioElement = null;
            if(streamWatch) {
                process.EnableRaisingEvents = true;
                stdioElement = OrderManager.CreateStandardInputOutputElement(customParameter.Caption, process, screen);
                DispatcherWrapper.Begin(element => {
                    element.StartView();
                }, stdioElement);
            }

            result.Success = process.Start();
            if(streamWatch) {
                stdioElement!.PreparateReceiver();
                // 受信前に他の処理を終わらせるため少し待つ
                DispatcherWrapper.Begin(element => {
                    element.RunReceiver();
                }, stdioElement, DispatcherPriority.ApplicationIdle);
            }

            return result;
        }

        public ILauncherExecuteResult Execute(LauncherItemKind kind, ILauncherExecutePathParameter pathParameter, ILauncherExecuteCustomParameter customParameter, IEnumerable<LauncherEnvironmentVariableData> environmentVariableItems, IScreen screen)
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

            return ExecuteFilePath(kind, pathParameter, customParameter, environmentVariableItems, screen);
        }

        public ILauncherExecuteResult OpenParentDirectory(LauncherItemKind kind, ILauncherExecutePathParameter pathParameter)
        {
            if(pathParameter == null) {
                throw new ArgumentNullException(nameof(pathParameter));
            }

            var path = Environment.ExpandEnvironmentVariables(pathParameter.Path ?? string.Empty);
            try {
                var systemExecutor = new SystemExecutor();
                var process = systemExecutor.OpenDirectoryWithFileSelect(path);
                var result = new LauncherExecuteResult() {
                    Kind = kind,
                    Process = process,
                };

                return result;
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                return LauncherExecuteResult.Error(ex);
            }
        }

        public ILauncherExecuteResult OpenWorkingDirectory(LauncherItemKind kind, ILauncherExecutePathParameter pathParameter)
        {
            if(pathParameter == null) {
                throw new ArgumentNullException(nameof(pathParameter));
            }

            var path = Environment.ExpandEnvironmentVariables(pathParameter.WorkDirectoryPath ?? string.Empty);
            try {
                var systemExecutor = new SystemExecutor();
                var process = systemExecutor.ExecuteFile(path!);
                var result = new LauncherExecuteResult() {
                    Kind = kind,
                    Process = process,
                };

                return result;
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                return LauncherExecuteResult.Error(ex);
            }
        }

        public void ShowProperty(ILauncherExecutePathParameter pathParameter)
        {
            if(pathParameter == null) {
                throw new ArgumentNullException(nameof(pathParameter));
            }

            var path = Environment.ExpandEnvironmentVariables(pathParameter.Path ?? string.Empty);
            var systemExecutor = new SystemExecutor();
            systemExecutor.ShowProperty(path);
        }

        #endregion

        #region
        #endregion
    }


}
