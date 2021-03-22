using System;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    [Flags]
    public enum UninstallTarget
    {
        /// <summary>
        /// なし。
        /// </summary>
        [EnumResource]
        None = 0,
        /// <summary>
        /// アプリケーション。
        /// </summary>
        [EnumResource]
        Application = 0b_0000_0001,
        /// <summary>
        /// アンインストールバッチファイル。
        /// </summary>
        [EnumResource]
        Batch = 0b_0000_0010,
        /// <summary>
        /// ユーザーデータ。
        /// </summary>
        [EnumResource]
        User = 0b_0000_0100,
        /// <summary>
        /// 端末データ。
        /// </summary>
        [EnumResource]
        Machine = 0b_0000_1000,
        /// <summary>
        /// 一時データ。
        /// </summary>
        [EnumResource]
        Temporary = 0b_0001_0000,
    }


}
