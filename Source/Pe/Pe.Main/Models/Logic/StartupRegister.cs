#if DEBUG || BETA
#   define SKIP_REGISTER
#endif

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    internal sealed class StartupParameter
    {
        #region property

        /// <summary>
        /// 起動時に待機するか。
        /// </summary>
        public bool DelayStartup { get; set; }
        /// <summary>
        /// 起動待ち時間。
        /// </summary>
        public TimeSpan StartupWaitTime { get; set; }
        /// <summary>
        /// その他引数。
        /// </summary>
        public string Argument { get; set; } = string.Empty;

        #endregion
    }

    internal sealed class StartupRegister
    {
        public StartupRegister(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        string StartupFileName { get; } =
#if DEBUG
            "Pe-debug.lnk"
#elif BETA
            "Pe-beta.lnk"
#else
            "Pe.lnk"
#endif
        ;

        string OldStartupFileName { get; } = "PeMain.lnk";

        string StartupFilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), StartupFileName);
        string OldStartupFilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), OldStartupFileName);

#endregion

        #region function

        /// <summary>
        /// スタートアップが存在するか
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            return File.Exists(StartupFilePath);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool Register(StartupParameter startupParameter)
        {
            Unregister();

            try {
                // 完全固定のブートストラップ前提
                var assemblyPath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))!, "Pe.exe");
                using(var shortcut = new ShortcutFile()) {
                    shortcut.TargetPath = assemblyPath;
                    shortcut.WorkingDirectory = Path.GetDirectoryName(assemblyPath)!;
                    shortcut.IconPath = assemblyPath;

                    var arguments = string.Empty;
                    if(startupParameter.DelayStartup) {
                        arguments = $"--wait {(int)startupParameter.StartupWaitTime.TotalMilliseconds}";
                    }
                    if(!string.IsNullOrWhiteSpace(startupParameter.Argument)) {
                        if(string.IsNullOrEmpty(arguments)) {
                            arguments = startupParameter.Argument;
                        } else {
                            arguments += " " + startupParameter.Argument;
                        }
                    }

                    shortcut.Arguments = arguments;

#if SKIP_REGISTER
                    Logger.LogInformation("スタートアップ登録処理はシンボル設定により未実施");
#else
                    shortcut.Save(StartupFilePath);
#endif
                }
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                return false;
            }

            // 古いスタートアップが存在すれば破棄しておく
            try {
                if(File.Exists(OldStartupFilePath)) {
                    File.Delete(OldStartupFilePath);
                }
            } catch(Exception ex) {
                Logger.LogWarning(ex, ex.Message);
            }

            return true;
        }

        public bool Unregister()
        {
            try {
                if(File.Exists(StartupFilePath)) {
                    File.Delete(StartupFilePath);
                }
                return true;
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public IResultSuccessValue<StartupParameter> GetStartupParameter()
        {
            if(!Exists()) {
                return ResultSuccessValue.Failure<StartupParameter>();
            }

            try {
                var shortcutFile = new ShortcutFile(StartupFilePath);
                var startupParameter = new StartupParameter() {
                    StartupWaitTime = TimeSpan.FromSeconds(3),
                };
                var arguments = shortcutFile.Arguments;
                var args = arguments.Split(' ');
                var commandLine = new CommandLine(args, false);
                var waitKey = commandLine.Add(longKey: "wait", hasValue: true);
                if(commandLine.Parse()) {
                    if(commandLine.Values.TryGetValue(waitKey, out var waitTimes)) {
                        if(int.TryParse(waitTimes.First, out var waitTime)) {
                            if(0 < waitTime) {
                                startupParameter.StartupWaitTime = TimeSpan.FromMilliseconds(waitTime);
                                startupParameter.DelayStartup = true;
                            }
                        }
                    }
                    if(commandLine.Unknowns.Any()) {
                        startupParameter.Argument = string.Join(" ", commandLine.Unknowns);
                    }
                }
                return ResultSuccessValue.Success(startupParameter);
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
            }

            return ResultSuccessValue.Failure<StartupParameter>();
        }

        #endregion
    }
}
