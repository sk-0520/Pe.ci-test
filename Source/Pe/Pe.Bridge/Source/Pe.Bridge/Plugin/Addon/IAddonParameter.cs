using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    /// <summary>
    /// アドオン機能構築時に Pe から渡されるデータ。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IAddonParameter: IPluginParameter
    {
        /// <summary>
        /// <see cref="IUserAgent"/> 生成器。
        /// </summary>
        IUserAgentFactory UserAgentFactory { get; }
        /// <summary>
        /// 実行処理。
        /// </summary>
        IAddonExecutor AddonExecutor { get; }
    }
}
