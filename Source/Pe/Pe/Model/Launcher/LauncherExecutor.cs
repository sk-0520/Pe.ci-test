using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Manager;

namespace ContentTypeTextNet.Pe.Main.Model.Launcher
{
    public interface ILauncherExecuteResult : IResultFailureValue<Exception>
    {
        #region property

        LauncherItemKind Kind { get; }

        Process Process { get; }

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

        public Process Process { get; set; }

        public bool Success { get; set; }

        public Type FailureType { get; set; }

        public Exception FailureValue { get; set; }

        #endregion
    }


    public class LauncherExecutor
    {
        public LauncherExecutor(IOrderManager orderManager, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateTartget(GetType());
            OrderManager = orderManager;
        }

        #region property

        IOrderManager OrderManager { get; }
        ILogger Logger { get; }

        #endregion

        #region function

        ILauncherExecuteResult ExecuteFilePath(LauncherItemKind kind, ILauncherExecutePathParameter pathParameter, ILauncherExecuteCustomParameter customParameter, IEnumerable<LauncherEnvironmentVariableItem> environmentVariableItems, Screen screen)
        {
            var process = new Process();
            var startInfo = process.StartInfo;

            // 実行パス
            startInfo.FileName = Environment.ExpandEnvironmentVariables(pathParameter.Path);

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
                foreach(var item in environmentVariableItems.Where(i => i.Kind == LauncherEnvironmentVariableKind.Update)) {
                    envs[item.Name] = item.Value;
                }
                // 削除
                foreach(var item in environmentVariableItems.Where(i => i.Kind == LauncherEnvironmentVariableKind.Remove)) {
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
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.RedirectStandardInput = true;
            }

            var result = new LauncherExecuteResult() {
                Kind = kind,
                Process = process,
            };

            //旧処理
            if(streamWatch) {
                var stdioElement = OrderManager.CreateStandardInputOutputElement($"{result.Process.StartInfo.FileName}", process);
                //    var streamData = new StreamData(launcherItem, screen, process);
                //    streamWindow = (LauncherItemStreamWindow)appSender.SendCreateWindow(WindowKind.LauncherStream, streamData, null);
                //    streamWindow.ViewModel.Start();
            }

            process.Start();
            if(streamWatch) {

            }

            return result;
        }

        public ILauncherExecuteResult Execute(LauncherItemKind kind, ILauncherExecutePathParameter pathParameter, ILauncherExecuteCustomParameter customParameter, IEnumerable<LauncherEnvironmentVariableItem> environmentVariableItems, Screen screen)
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

            var path = PathUtility.ExpandFilePath(pathParameter.Path);
            var parentDirPath = Path.GetDirectoryName(path);
            try {
                var process = Process.Start(parentDirPath);
                var result = new LauncherExecuteResult() {
                    Kind = kind,
                    Process = process,
                };

                return result;
            } catch(Exception ex) {
                Logger.Error(ex);
                return LauncherExecuteResult.Error(ex);
            }
        }

        public ILauncherExecuteResult OpenWorkingDirectory(LauncherItemKind kind, ILauncherExecutePathParameter pathParameter)
        {
            if(pathParameter == null) {
                throw new ArgumentNullException(nameof(pathParameter));
            }

            var path = PathUtility.ExpandFilePath(pathParameter.WorkDirectoryPath);
            try {
                var process = Process.Start(path);
                var result = new LauncherExecuteResult() {
                    Kind = kind,
                    Process = process,
                };

                return result;
            } catch(Exception ex) {
                Logger.Error(ex);
                return LauncherExecuteResult.Error(ex);
            }
        }

        #endregion

        #region
        #endregion
    }
}
