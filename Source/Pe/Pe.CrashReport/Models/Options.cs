using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.CrashReport.Models
{
    public class Options
    {
        #region property

        /// <summary>
        /// Pe から呼び出されたか。
        /// <para>道徳心が大事。</para>
        /// </summary>
        [CommandLine(longKey: "call-from-pe", hasValue: false)]
        public bool CallFromPe { get; set; }


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
