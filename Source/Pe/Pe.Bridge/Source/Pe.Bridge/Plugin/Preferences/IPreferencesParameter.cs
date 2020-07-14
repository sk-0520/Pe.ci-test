using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.ViewModels;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Preferences
{
    /// <summary>
    /// 設定機能構築時に Pe から渡されるデータ。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IPreferencesParameter: IPluginParameter
    {
        #region property

        /// <summary>
        /// <see cref="IHttpUserAgent"/> 生成器。
        /// </summary>
        IHttpUserAgentFactory HttpUserAgentFactory { get; }

        /// <summary>
        /// ViewModelの面倒実装部分。
        /// </summary>
        ISkeletonImplements SkeletonImplements { get; }

        #endregion
    }
}
