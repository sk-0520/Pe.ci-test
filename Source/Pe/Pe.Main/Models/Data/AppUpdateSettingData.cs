using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// アップデート方法。
    /// </summary>
    public enum UpdateKind
    {
        /// <summary>
        /// 実施しない。
        /// </summary>
        [EnumResource]
        None,
        /// <summary>
        /// 通知のみ。
        /// </summary>
        [EnumResource]
        Notify,
        /// <summary>
        /// 自動実行。
        /// </summary>
        [EnumResource]
        Auto,
    }

    public class SettingAppUpdateSettingData: DataBase
    {
        #region property

        public UpdateKind UpdateKind { get; set; }

        #endregion
    }
}
