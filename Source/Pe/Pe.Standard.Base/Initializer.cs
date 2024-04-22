using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ContentTypeTextNet.Pe.Standard.Base
{
    /// <summary>
    /// <see cref="ISupportInitialize"/>の初期化から初期化終了までを<see langword="using" />で実施できるようにする。
    /// </summary>
    public class Initializer<TSupportInitialize>: DisposerBase
        where TSupportInitialize : ISupportInitialize
    {
        #region variable

        private TSupportInitialize? _target;

        #endregion

        public Initializer(TSupportInitialize target)
        {
            this._target = target;
        }

        #region property

        public TSupportInitialize Target
        {
            get
            {
                ThrowIfDisposed();

                if(this._target is null) {
                    throw new InvalidOperationException();
                }

                return this._target;
            }
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(this._target != null) {
                    this._target.EndInit();
                    this._target = default;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// <see cref="Initializer{TSupportInitialize}"/> のラッパー。
    /// </summary>
    public static class Initializer
    {
        #region function

        /// <summary>
        /// 初期化用処理を簡略化。
        /// </summary>
        /// <remarks>
        /// <para>多分こいつしか使わない。</para>
        /// </remarks>
        /// <example>
        /// using(Initializer.Begin(obj)) {
        ///     obj.Property = xxx;
        /// }
        /// </example>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Initializer<TSupportInitialize> Begin<TSupportInitialize>(TSupportInitialize target)
            where TSupportInitialize : ISupportInitialize
        {
            var result = new Initializer<TSupportInitialize>(target);

            result.Target.BeginInit();

            return result;
        }

        #endregion
    }
}
