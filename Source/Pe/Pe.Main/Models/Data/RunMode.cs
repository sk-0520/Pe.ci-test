using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public enum RunMode
    {
        Normal,
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
