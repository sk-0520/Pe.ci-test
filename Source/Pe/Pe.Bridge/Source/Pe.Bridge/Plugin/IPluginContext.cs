using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    public interface IPluginInitializeContext
    {
        #region property
        #endregion
    }

    /// <summary>
    /// プラグインとPeの架け橋。
    /// <para>持ち歩かないこと(必要箇所で都度渡すので勘弁して)。</para>
    /// </summary>
    public interface IPluginContext
    {
        #region property

        /// <summary>
        /// ストレージ操作。
        /// </summary>
        IPluginStorage Storage { get; }

        #endregion
    }
}
