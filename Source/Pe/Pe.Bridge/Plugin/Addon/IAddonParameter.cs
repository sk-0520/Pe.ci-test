using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.ViewModels;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    /// <summary>
    /// アドオン機能構築時に Pe から渡されるデータ。
    /// </summary>
    /// <remarks>
    /// <para>Pe から提供される。</para>
    /// </remarks>
    public interface IAddonParameter: IPluginParameter
    {
        #region property

        /// <summary>
        /// <see cref="IHttpUserAgent"/> 生成器。
        /// </summary>
        IHttpUserAgentFactory HttpUserAgentFactory { get; }
        /// <summary>
        /// 実行処理。
        /// </summary>
        IAddonExecutor AddonExecutor { get; }

        /// <summary>
        /// ViewModelの面倒実装部分。
        /// </summary>
        ISkeletonImplements SkeletonImplements { get; }

        #endregion
    }
}
