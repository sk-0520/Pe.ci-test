using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.ViewModels;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Preferences
{
    /// <summary>
    /// 設定機能構築時に Pe から渡されるデータ。
    /// </summary>
    /// <remarks>
    /// <para>Pe から提供される。</para>
    /// </remarks>
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
