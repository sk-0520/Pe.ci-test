using System;
using System.Runtime.Serialization;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// 標準入出力設定。
    /// </summary>
    [Serializable, DataContract]
    public class SettingAppStandardInputOutputSettingData
    {
        #region property

        /// <summary>
        /// フォントID。
        /// </summary>
        public FontId FontId { get; set; }
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
