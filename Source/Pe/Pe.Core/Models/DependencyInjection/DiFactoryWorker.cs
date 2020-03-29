using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.DependencyInjection
{
    /// <summary>
    /// マッピング生成処理。
    /// </summary>
    /// <returns></returns>
    public delegate object DiCreator();

    /// <summary>
    /// マッピング生成キャッシュ。
    /// </summary>
    public sealed class DiFactoryWorker : DisposerBase
    {
        public DiFactoryWorker(DiLifecycle lifecycle, DiCreator creator, object bind)
        {
            Lifecycle = lifecycle;
            Creator = creator;
        }

        #region property

        public DiLifecycle Lifecycle { get; }
        DiCreator Creator { get; }

        bool CreatedSingleton { get; set; }

        #endregion

        #region function

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
