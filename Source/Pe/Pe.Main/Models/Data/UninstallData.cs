using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    [Flags]
    public enum UninstallTarget
    {
        /// <summary>
        /// なし。
        /// </summary>
        None = 0,
        /// <summary>
        /// アプリケーション。
        /// </summary>
        Application = 0b_0000_0001,
        /// <summary>
        /// アンインストールバッチファイル。
        /// </summary>
        Batch = 0b_0000_0010,
        /// <summary>
        /// ユーザーデータ。
        /// </summary>
        User = 0b_0000_0100,
        /// <summary>
        /// 端末データ。
        /// </summary>
        Machine = 0b_0000_1000,
        /// <summary>
        /// 一時データ。
        /// </summary>
        Temporary = 0b_0001_0000,
    }


}
