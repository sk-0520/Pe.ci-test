using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    /// <summary>
    /// プラグイン初期化時の Pe との架け橋。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IPluginInitializeContext
    {
        #region property

        /// <summary>
        /// ストレージ操作。
        /// </summary>
        IPluginStorage Storage { get; }

        #endregion
    }

    /// <summary>
    /// プラグインと Pe の架け橋。
    /// <para>持ち歩かないこと(必要箇所で都度渡すので勘弁して)。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IPluginContext
    {
        #region property

        /// <summary>
        /// ストレージ操作。
        /// </summary>
        IPluginStorage Storage { get; }

        IUserAgentFactory UserAgentFactory { get; }

        #endregion
    }
}
