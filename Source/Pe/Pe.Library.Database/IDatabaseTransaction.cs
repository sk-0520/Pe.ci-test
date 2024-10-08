using System;
using System.Data;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// データベース実装におけるトランザクション処理。
    /// </summary>
    /// <remarks>
    /// <para>これが実体化されてればトランザクション中でしょうね。</para>
    /// <para>コミット・ロールバックは一回だけ実行される使用を想定している。</para>
    /// </remarks>
    public interface IDatabaseTransaction: IDatabaseContext, IDatabaseContexts, IDisposable
    {
        #region property

        /// <summary>
        /// CRL上のトランザクション実体。
        /// </summary>
        /// <remarks>
        /// <para>トランザクションを開始しない場合 <see langword="null" /> となり、扱いは <see cref="IDatabaseTransaction"/> 実装側依存となる。</para>
        /// </remarks>
        IDbTransaction? Transaction { get; }

        #endregion

        #region function

        /// <summary>
        /// コミット！
        /// </summary>
        void Commit();

        /// <summary>
        /// なかったことにしたい人生の一部。
        /// </summary>
        void Rollback();

        #endregion
    }
}
