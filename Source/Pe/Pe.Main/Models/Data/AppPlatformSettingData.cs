using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// プラットフォーム設定。
    /// </summary>
    public class SettingAppPlatformSettingData: DataBase
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
