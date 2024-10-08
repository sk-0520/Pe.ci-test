using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// データベースへの遅延書き込み。
    /// </summary>
    public interface IDatabaseDelayWriter: IFlushable, IDisposed
    {
        #region property

        /// <summary>
        /// 停止中か。
        /// </summary>
        bool IsPausing { get; }

        #endregion

        #region function

        /// <summary>
        /// 周期処理を一時停止。
        /// </summary>
        /// <returns></returns>
        IDisposer Pause();
        /// <summary>
        /// DB処理を遅延実行。
        /// </summary>
        /// <param name="action">DB処理本体。</param>
        void Stock(Action<IDatabaseTransaction> action);
        /// <summary>
        /// DB処理を遅延実行。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="uniqueKey"/>でグルーピングし、一番若い処理が実行される。</para>
        /// <para><see cref="UniqueKeyPool"/>を用いる前提。</para>
        /// </remarks>
        /// <param name="action">DB処理本体。</param>
        /// <param name="uniqueKey">一意オブジェクト。</param>
        void Stock(Action<IDatabaseTransaction> action, object uniqueKey);

        /// <summary>
        /// ため込んでいるDB処理をなかったことにする。
        /// </summary>
        /// <remarks>
        /// <para>特定の状況でしか使い道がないので使用には注意すること。</para>
        /// </remarks>
        void ClearStock();

        #endregion
    }
}
