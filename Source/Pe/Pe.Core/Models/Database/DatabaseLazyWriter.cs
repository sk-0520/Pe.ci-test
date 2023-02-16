using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Standard.Base;
using ContentTypeTextNet.Pe.Standard.Base.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    /// <summary>
    /// データベースへの遅延書き込み。
    /// </summary>
    public interface IDatabaseLazyWriter: IFlushable, IDisposedChackable
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
        /// <para><paramref name="uniqueKey"/>でグルーピングし、一番若い処理が実行される。</para>
        /// <para><see cref="UniqueKeyPool"/>を用いる前提。</para>
        /// </summary>
        /// <param name="action">DB処理本体。</param>
        /// <param name="uniqueKey">一意オブジェクト。</param>
        void Stock(Action<IDatabaseTransaction> action, object uniqueKey);

        /// <summary>
        /// ため込んでいるDB処理をなかったことにする。
        /// <para>特定の状況でしか使い道がないので使用には注意すること。</para>
        /// </summary>
        void ClearStock();

        #endregion
    }

    public class DatabaseLazyWriter: DisposerBase, IDatabaseLazyWriter
    {
        #region define

        private readonly struct LazyStockItem
        {
            public LazyStockItem(Action<IDatabaseTransaction> action)
            {
                Action = action;
                StockTimestamp = DateTime.UtcNow;
            }

            #region property

            public Action<IDatabaseTransaction> Action { get; }
            [DateTimeKind(DateTimeKind.Utc)]
            public DateTime StockTimestamp { get; }

            #endregion
        }

        #endregion

        #region variable

        private readonly object _timerLocker = new object();

        #endregion

        public DatabaseLazyWriter(IDatabaseBarrier databaseBarrier, TimeSpan pauseRetryTime, ILoggerFactory loggerFactory)
        {
            if(databaseBarrier == null) {
                throw new ArgumentNullException(nameof(databaseBarrier));
            }
            if(loggerFactory == null) {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            DatabaseBarrier = databaseBarrier;
            PauseRetryTime = pauseRetryTime;
            Logger = loggerFactory.CreateLogger(GetType());

            LazyTimer = new Timer(LazyCallback);
        }

        #region property

        private IDatabaseBarrier DatabaseBarrier { get; }

        /// <summary>
        /// リトライ間隔。
        /// </summary>
        private TimeSpan PauseRetryTime { get; }
        private ILogger Logger { get; }

        private Timer LazyTimer { get; [Unused(UnusedKinds.Dispose)] set; }

        private IList<LazyStockItem> StockItems { get; } = new List<LazyStockItem>();
        private IDictionary<object, LazyStockItem> UniqueItems { get; } = new Dictionary<object, LazyStockItem>();

        #endregion

        #region function

        private void StopTimer()
        {
            LazyTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }
        private void StartTimer()
        {
            ThrowIfDisposed();

            LazyTimer.Change(PauseRetryTime, PauseRetryTime);
        }

        private void StockCore(Action<IDatabaseTransaction> action, object? uniqueKey)
        {
            lock(this._timerLocker) {
                StopTimer();

                // 既に登録されている処理が存在する場合は破棄しておく
                if(uniqueKey != null) {
                    if(UniqueItems.TryGetValue(uniqueKey, out var stockedItem)) {
                        Logger.LogTrace("待機処理破棄: {0} {1}", stockedItem.StockTimestamp, uniqueKey.GetHashCode());
                        StockItems.Remove(stockedItem);
                        UniqueItems.Remove(uniqueKey);
                    }
                }

                var item = new LazyStockItem(action);
                StockItems.Add(item);
                if(uniqueKey != null) {
                    UniqueItems.Add(uniqueKey, item);
                }

                StartTimer();
            }
        }

        private void LazyCallback(object? state)
        {
            if(IsPausing) {
                return;
            }
            Flush();
        }

        private void FlushCore(LazyStockItem[] stockItems)
        {
            using var transaction = DatabaseBarrier.WaitWrite();
            foreach(var stockItem in stockItems) {
                stockItem.Action(transaction);
            }
            transaction.Commit();
        }


        #endregion

        #region IDatabaseLazyWriter

        /// <inheritdoc cref="IDatabaseLazyWriter.IsPausing"/>
        public bool IsPausing { get; private set; }

        /// <inheritdoc cref="IDatabaseLazyWriter.Pause"/>
        public IDisposer Pause()
        {
            ThrowIfDisposed();

            IsPausing = true;
            return new ActionDisposer(d => {
                IsPausing = false;
            });
        }

        /// <inheritdoc cref="IDatabaseLazyWriter.Stock(Action{IDatabaseTransaction}, object)"/>
        public void Stock(Action<IDatabaseTransaction> action, object uniqueKey)
        {
            if(uniqueKey == null) {
                throw new ArgumentNullException(nameof(uniqueKey));
            }
#if DEBUG
            if(uniqueKey is UniqueKeyPool) {
                Debug.Assert(false, $"完全な事故: {nameof(UniqueKeyPool)}.{nameof(UniqueKeyPool.Get)} を使用していない可能性あり");
            }
#endif

            ThrowIfDisposed();

            StockCore(action, uniqueKey);
        }

        /// <inheritdoc cref="IDatabaseLazyWriter.Stock(Action{IDatabaseTransaction})"/>
        public void Stock(Action<IDatabaseTransaction> action)
        {
            ThrowIfDisposed();
            StockCore(action, null);
        }

        /// <inheritdoc cref="IDatabaseLazyWriter.ClearStock"/>
        public void ClearStock()
        {
            ThrowIfDisposed();

            lock(this._timerLocker) {
                StopTimer();

                StockItems.Clear();

                StartTimer();
            }
        }

        #endregion

        #region IFlush

        void Flush(bool disposing)
        {
            LazyStockItem[] items;
            lock(this._timerLocker) {
                if(disposing) {
                    StopTimer();
                }
                items = StockItems.ToArray();
                StockItems.Clear();
                UniqueItems.Clear();
            }

            if(items.Length == 0) {
                return;
            }

            FlushCore(items);
        }

        /// <inheritdoc cref="IFlushable.Flush"/>
        public void Flush()
        {
            Flush(true);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Flush(disposing);
                StockItems.Clear();
                UniqueItems.Clear();
                if(disposing) {
                    LazyTimer.Dispose();
                }
                LazyTimer = null!;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
