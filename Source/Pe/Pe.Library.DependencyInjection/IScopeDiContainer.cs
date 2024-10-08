using System;

namespace ContentTypeTextNet.Pe.Library.DependencyInjection
{
    /// <summary>
    /// 限定的なDIコンテナ。
    /// </summary>
    public interface IScopeDiContainer: IDiRegisterContainer, IDisposable
    {
        /// <summary>
        /// ただ単純な登録。
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="lifecycle"></param>
        /// <returns></returns>
        IScopeDiContainer Register<TObject>(DiLifecycle lifecycle)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        ;
    }
}
