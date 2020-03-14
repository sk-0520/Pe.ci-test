using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public class RunMode
    {
        #region property

        /// <summary>
        /// 通常起動。
        /// </summary>
        public string Normal { get; } = string.Empty;
        /// <summary>
        /// クラッシュレポート送信起動。
        /// </summary>
        public string CrashReport { get; } = "crash-report";

        #endregion
    }
}
