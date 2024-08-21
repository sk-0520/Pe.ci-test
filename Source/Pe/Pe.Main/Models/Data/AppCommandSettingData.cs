using System;
using System.Runtime.Serialization;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// コマンド設定。
    /// </summary>
    [Serializable, DataContract]
    public class SettingAppCommandSettingData
    {
        public SettingAppCommandSettingData()
        { }

        #region property

        /// <summary>
        /// フォントID。
        /// </summary>
        public FontId FontId { get; set; }
        /// <summary>
        /// アイコンサイズ。
        /// </summary>
        public IconBox IconBox { get; set; }
        /// <summary>
        /// テキスト状態の横幅。
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// 非表示までの時間。
        /// </summary>
        public TimeSpan HideWaitTime { get; set; }

        #endregion
    }
}
