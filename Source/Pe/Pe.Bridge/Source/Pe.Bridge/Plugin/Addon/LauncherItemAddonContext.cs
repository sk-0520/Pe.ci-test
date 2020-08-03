using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    /// <summary>
    /// ランチャーアイテムアドオンと Pe の架け橋。
    /// <para>ランチャーアイテムアドオン内で持ち歩くのは一応OKだけど持ち歩かない方針(なんのこっちゃ)。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface ILauncherItemAddonContext: ILauncherItemId
    {
        #region property

        /// <summary>
        /// ストレージ操作。
        /// </summary>
        ILauncherItemAddonStorage Storage { get; }

        #endregion
    }
}
