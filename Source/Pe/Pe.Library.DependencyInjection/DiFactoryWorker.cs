using System;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Library.DependencyInjection
{
    /// <summary>
    /// マッピング生成処理。
    /// </summary>
    /// <returns></returns>
    public delegate object DiCreator();

    /// <summary>
    /// マッピング生成キャッシュ。
    /// </summary>
    public sealed class DiFactoryWorker: DisposerBase
    {
        public DiFactoryWorker(DiLifecycle lifecycle, DiCreator creator, object bind)
        {
            Lifecycle = lifecycle;
            Creator = creator;
        }

        #region property

        /// <summary>
        /// ライフサイクル。
        /// </summary>
        public DiLifecycle Lifecycle { get; }
        /// <summary>
        /// 生成処理。
        /// </summary>
        private DiCreator Creator { get; }

        /// <summary>
        /// シングルトンデータとして作成されているか。
        /// </summary>
        /// <remarks>
        /// <para>シングルトンの場合に、<see cref="DiFactoryWorker"/>の<see cref="Dispose"/>時に対象が<see cref="IDisposable"/>を実装していれば<see cref="IDisposable.Dispose"/>を呼び出す。</para>
        /// </remarks>
        private bool CreatedSingleton { get; set; }

        #endregion

        #region function

        /// <summary>
        /// 生成。
        /// </summary>
        /// <returns></returns>
        public object Create()
        {
            var result = Creator();

            if(Lifecycle == DiLifecycle.Singleton) {
                CreatedSingleton = true;
            }

            return result;
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    if(CreatedSingleton) {
                        var createdObject = Create();
                        if(createdObject is IDisposable disposer) {
                            disposer.Dispose();
                        }
                    }
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
