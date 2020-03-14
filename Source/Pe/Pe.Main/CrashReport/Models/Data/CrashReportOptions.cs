using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Models.Data
{
    internal class CrashReportOptions
    {
        #region property

        /// <summary>
        /// 自動送信するか。
        /// </summary>
        [CommandLine(longKey: "auto-send", hasValue: false)]
        public bool AutoSend { get; set; }

        /// <summary>
        /// クラッシュレポートのファイルパス。
        /// </summary>
        [CommandLine(longKey: "report-file", hasValue: true)]
        public string CrashReportFilePath { get; set; } = string.Empty;

        /// <summary>
        /// クラッシュレポートのファイルパス。
        /// </summary>
        [CommandLine(longKey: "arguments", hasValue: true)]
        public string Arguments { get; set; } = string.Empty;

        #endregion
    }
}
