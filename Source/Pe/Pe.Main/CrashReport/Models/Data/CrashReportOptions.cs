using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Models.Data
{
    public class CrashReportOptions
    {
        #region property

        /// <summary>
        /// 言語。
        /// </summary>
        [CommandLine(longKey: "language", hasValue: true)]
        public string Language { get; set; } = string.Empty;

        /// <summary>
        /// 自動送信するか。
        /// </summary>
        [CommandLine(longKey: "auto-send", hasValue: false)]
        public bool AutoSend { get; set; }

        /// <summary>
        /// 送信先。
        /// </summary>
        [CommandLine(longKey: "post-uri", hasValue: true)]
        public string PostUri { get; set; } = string.Empty;
        /// <summary>
        /// 送信先。
        /// </summary>
        [CommandLine(longKey: "src-uri", hasValue: true)]
        public string SourceUri { get; set; } = string.Empty;

        /// <summary>
        /// 生クラッシュレポートのファイルパス。
        /// </summary>
        [CommandLine(longKey: "report-raw-file", hasValue: true)]
        public string CrashReportRawFilePath { get; set; } = string.Empty;

        /// <summary>
        /// 送信時に保存されるクラッシュレポートのファイルパス。
        /// </summary>
        [CommandLine(longKey: "report-save-file", hasValue: true)]
        public string CrashReportSaveFilePath { get; set; } = string.Empty;

        /// <summary>
        /// Pe の実行コマンド。
        /// </summary>
        [CommandLine(longKey: "execute-command", hasValue: true)]
        public string ExecuteCommand { get; set; } = string.Empty;

        /// <summary>
        /// Pe の起動時オプション。
        /// </summary>
        [CommandLine(longKey: "execute-argument", hasValue: true)]
        public string ExecuteArgument { get; set; } = string.Empty;

        #endregion
    }
}
