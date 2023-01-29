using System;
using System.Runtime.Serialization;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// プラットフォーム設定。
    /// </summary>
    [Serializable, DataContract]
    public class SettingAppPlatformSettingData
    {
        #region property

        /// <summary>
        /// アイドル状態を抑制。
        /// </summary>
        public bool SuppressSystemIdle { get; set; }
        /// <summary>
        /// エクスプローラ補助。
        /// </summary>
        public bool SupportExplorer { get; set; }

        #endregion
    }
}
