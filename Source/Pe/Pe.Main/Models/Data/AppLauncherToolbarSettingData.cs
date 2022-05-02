using System;
using System.Runtime.Serialization;
using ContentTypeTextNet.Pe.Core.Models.Data;

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
        /// グループメニューの位置。
        /// </summary>
        public LauncherGroupPosition GroupMenuPosition { get; set; }

        #endregion
    }
}
