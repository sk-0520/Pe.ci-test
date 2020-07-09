using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    /// <summary>
    /// プラグイン機能構築時に Pe から渡されるデータ。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IPluginParameter
    {
        #region property

        IPlatformTheme PlatformTheme { get; }
        IImageLoader ImageLoader { get; }
        IDispatcherWrapper DispatcherWrapper { get; }
        ILoggerFactory LoggerFactory { get; }

        #endregion
    }
}
