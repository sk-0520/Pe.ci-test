using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// <see cref="ISupportInitialize"/>の初期化から初期化終了までを<c>using</c>で実施できるようにする。
    /// </summary>
    public class Initializer: DisposerBase
    {
        private Initializer(ISupportInitialize target)
        {
            Target = target;
        }

        #region property

        private ISupportInitialize? Target { get; [Unuse(UnuseKinds.Dispose)] set; }

        #endregion

        #region function

        /// <summary>
        /// 初期化用処理を簡略化。
        /// <para>多分こいつしか使わない。</para>
        /// </summary>
        /// <example>
        /// using(Initializer.Begin(obj)) {
        ///     obj.Property = xxx;
        /// }
        /// </example>
        /// <param name="target"></param>
        /// <returns></returns>
        public static IDisposable Begin(ISupportInitialize target)
        {
            var result = new Initializer(target);
            target.BeginInit();

            return result;
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(Target != null) {
                    Target.EndInit();
                    Target = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
