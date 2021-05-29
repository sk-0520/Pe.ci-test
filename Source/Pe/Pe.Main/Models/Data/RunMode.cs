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
    }

    internal static class RunModeUtility
    {
        #region property

        #endregion

        #region function

        public static RunMode Parse(string? value)
        {
            switch(value?.ToLowerInvariant()) {
                case "crash-report":
                    return RunMode.CrashReport;

                default:
                    return RunMode.Normal;
            }
        }

        #endregion
    }
}
