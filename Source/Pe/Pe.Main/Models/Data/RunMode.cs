using System;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// 実行方法。
    /// </summary>
    public enum RunMode
    {
        /// <summary>
        /// 通常。
        /// </summary>
        Normal,
        /// <summary>
        /// クラッシュレポート。
        /// </summary>
        CrashReport,
        /// <summary>
        /// 一時的プロセス間通信。
        /// </summary>
        InterProcessCommunication,
    }

    internal static class RunModeUtility
    {
        #region property

        #endregion

        #region function

        public static string ToString(RunMode runMode) => runMode switch {
            RunMode.Normal => "normal",
            RunMode.CrashReport => "crash-report",
            RunMode.InterProcessCommunication => "ipc",
            _ => throw new NotImplementedException(),
        };

        public static RunMode Parse(string? value)
        {
            switch(value?.ToLowerInvariant()) {
                case "crash-report":
                    return RunMode.CrashReport;

                case "ipc":
                    return RunMode.InterProcessCommunication;

                default:
                    return RunMode.Normal;
            }
        }

        /// <summary>
        /// 単体プロセスのみの実行か。
        /// </summary>
        /// <param name="runMode"></param>
        /// <returns></returns>
        public static bool IsSingleProcessOnly(RunMode runMode)
        {
            return runMode == RunMode.Normal;
        }

        public static bool CheckBetaModeAlert(RunMode runMode)
        {
            return runMode == RunMode.Normal;
        }

        public static bool CanTestPluginInstall(RunMode runMode)
        {
            return runMode == RunMode.Normal;
        }

        /// <summary>
        /// インフラ構築を行うか。
        /// </summary>
        /// <param name="runMode"></param>
        /// <returns></returns>
        public static bool IsBuildInfrastructure(RunMode runMode)
        {
            return runMode != RunMode.CrashReport;
        }

        /// <summary>
        /// 使用許諾表示を行うか。
        /// </summary>
        /// <param name="runMode"></param>
        /// <returns></returns>
        public static bool IsUserAccept(RunMode runMode)
        {
            return runMode == RunMode.Normal;
        }

        /// <summary>
        /// 初期構築処理を行うか。
        /// </summary>
        /// <param name="runMode"></param>
        /// <returns></returns>
        public static bool NeedFirstSetup(RunMode runMode)
        {
            return runMode == RunMode.Normal;
        }

        /// <summary>
        /// ファイル構築処理を行うか。
        /// </summary>
        /// <param name="runMode"></param>
        /// <returns></returns>
        public static bool IsBuildFileSystem(RunMode runMode)
        {
            return runMode == RunMode.Normal;
        }
        /// <summary>
        /// DB構築処理を行うか。
        /// </summary>
        /// <param name="runMode"></param>
        /// <returns></returns>
        public static bool IsBuildPersistence(RunMode runMode)
        {
            return runMode == RunMode.Normal;
        }

        public static bool IsBuildWebView(RunMode runMode)
        {
            return runMode == RunMode.Normal;
        }

        #endregion
    }
}
