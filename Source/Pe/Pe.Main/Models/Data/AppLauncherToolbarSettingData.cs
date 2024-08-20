using System;
using System.Runtime.Serialization;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// ランチャーツールバー設定。
    /// </summary>
    [Serializable, DataContract]
    public class AppLauncherToolbarSettingData
    {
        #region property

        /// <summary>
        /// ボタンへのD&amp;D実行時の処理方法。
        /// </summary>
        public LauncherToolbarContentDropMode ContentDropMode { get; set; }
        /// <summary>
        /// D&amp;Dファイルがショートカットの場合の処理方法。
        /// </summary>
        public LauncherToolbarShortcutDropMode ShortcutDropMode { get; set; }
        /// <summary>
        /// グループメニューの位置。
        /// </summary>
        public LauncherGroupPosition GroupMenuPosition { get; set; }

        #endregion
    }
}
