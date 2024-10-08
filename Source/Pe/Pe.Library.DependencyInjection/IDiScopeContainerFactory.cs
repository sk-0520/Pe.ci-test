//#define ENABLED_PRISM7

#if ENABLED_PRISM7
using Prism.Ioc;
#endif

namespace ContentTypeTextNet.Pe.Library.DependencyInjection
{
    public interface IDiScopeContainerFactory
    {
        /// <summary>
        /// 限定的なDIコンテナを作成。
        /// </summary>
        /// <returns>現在マッピングを複製したDIコンテナ。</returns>
        IScopeDiContainer Scope();
    }
}
