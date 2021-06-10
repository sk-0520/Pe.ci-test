using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// 通知ログ設定。
    /// </summary>
    public class SettingAppNotifyLogSettingData: DataBase
    {
        #region property

        /// <summary>
        /// 表示するか。
        /// </summary>
        public bool IsVisible { get; set; }
        /// <summary>
        /// 表示位置。
        /// </summary>
        public NotifyLogPosition Position { get; set; }

        #endregion
    }
}
