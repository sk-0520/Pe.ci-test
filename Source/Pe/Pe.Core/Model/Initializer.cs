using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ContentTypeTextNet.Pe.Common.Model;

namespace ContentTypeTextNet.Pe.Core.Model
{
    /// <summary>
    /// <see cref="ISupportInitialize"/>の初期化から初期化終了までを using で実施できるようにする。
    /// </summary>
    public class Initializer : DisposerBase
    {
        public Initializer(ISupportInitialize target)
        {
            Target = target;
        }

        #region property

        public ISupportInitialize Target { get; private set; }

        #endregion

        #region function

        /// <summary>
        /// 初期化用処理を簡略化。
        /// <para>多分こいつしか使わない。</para>
        /// </summary>
        /// <example>
        /// using(Initializer.BeginInitialize(obj)) {
        ///     obj.Property = xxx;
        /// }
        /// </example>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Initializer BeginInitialize(ISupportInitialize target)
        {
            var result = new Initializer(target);
            result.Target.BeginInit();

            return result;
        }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(Target != null) {
                    Target.EndInit();
                    Target = null!;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
