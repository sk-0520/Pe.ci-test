using System;
using System.Runtime.Serialization;

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

    /// <summary>
    /// アップデート設定。
    /// </summary>
    [Serializable, DataContract]
    public class SettingAppUpdateSettingData
    {
        #region property

        /// <summary>
        /// アップデート方法。
        /// </summary>
        public UpdateKind UpdateKind { get; set; }

        #endregion
    }
}
