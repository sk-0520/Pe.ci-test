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
    public sealed class StartupRegister
    {
        public StartupRegister(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        string StartupFileName { get; } = "Pe.lnk";
        string OldStartupFileName { get; } = "PeMain.lnk";

        #endregion

        #region function

        /// <summary>
        /// スタートアップが存在するか
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            var assemblyPath = Assembly.GetExecutingAssembly().Location;

            var startupShortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), StartupFileName);
            if(File.Exists(startupShortcutPath)) {
                return true;
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool Register()
        {
            var startupShortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), StartupFileName);
            try {
                if(File.Exists(startupShortcutPath)) {
                    File.Delete(startupShortcutPath);
                }
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                return false;
            }

            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            using(var shortcut = new ShortcutFile()) {
                shortcut.TargetPath = assemblyPath;
#pragma warning disable CS8601 // Null 参照割り当ての可能性があります。
                shortcut.WorkingDirectory = Path.GetDirectoryName(assemblyPath);
#pragma warning restore CS8601 // Null 参照割り当ての可能性があります。
                shortcut.IconPath = assemblyPath;
#if DEBUG || BETA
                Logger.LogInformation("skip!");
#else
                shortcut.Save(startupShortcutPath);
#endif
            }

            // 古いスタートアップが存在すれば破棄しておく
            var oldStartupShortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), OldStartupFileName);
            try {
                if(File.Exists(oldStartupShortcutPath)) {
                    File.Delete(oldStartupShortcutPath);
                }
            } catch(Exception ex) {
                Logger.LogWarning(ex, ex.Message);
            }

            return true;
        }

        #endregion
    }
}
