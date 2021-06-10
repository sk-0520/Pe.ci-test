using System;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// 標準入出力設定。
    /// </summary>
    public class SettingAppStandardInputOutputSettingData: DataBase
    {
        #region property

        /// <summary>
        /// フォントID。
        /// </summary>
        public Guid FontId { get; set; }
        /// <summary>
        /// 標準出力 前景色。
        /// </summary>
        public Color OutputForegroundColor { get; set; }
        /// <summary>
        /// 標準出力 背景色。
        /// </summary>
        public Color OutputBackgroundColor { get; set; }
        /// <summary>
        /// 標準エラー 前景色。
        /// </summary>
        public Color ErrorForegroundColor { get; set; }
        /// <summary>
        /// 標準エラー 背景色。
        /// </summary>
        public Color ErrorBackgroundColor { get; set; }
        /// <summary>
        /// 最前面表示。
        /// </summary>
        public bool IsTopmost { get; set; }

        #endregion
    }
}
