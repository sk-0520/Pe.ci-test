using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    class LazyStockItem
    {
        public LazyStockItem(Action<IDatabaseTransaction> action)
        {
            Action = action;
        }

        #region property

        public Action<IDatabaseTransaction> Action { get; }
        [Timestamp(DateTimeKind.Unspecified)]
        public DateTime StockTimestamp { get; } = DateTime.UtcNow;

        #endregion
    }

    public interface IDatabaseLazyWriter: IFlushable
    {
        #region property

        bool IsPausing { get; }

        #endregion

        #region function

        IDisposer Pause();
        void Stock(Action<IDatabaseTransaction> action);
        void Stock(Action<IDatabaseTransaction> action, object uniqueKey);

        #endregion
    }

    public class DatabaseLazyWriter : DisposerBase, IDatabaseLazyWriter
    {
        #region variable

        object _timerLocker = new object();

        #endregion

        public DatabaseLazyWriter(IDatabaseBarrier databaseBarrier, TimeSpan waitTime, ILoggerFactory loggerFactory)
            : this(databaseBarrier, waitTime, TimeSpan.FromSeconds(1), loggerFactory)
        { }

        public DatabaseLazyWriter(IDatabaseBarrier databaseBarrier, TimeSpan waitTime, TimeSpan pauseRetryTime, ILoggerFactory loggerFactory)
        {
            if(databaseBarrier == null) {
                throw new ArgumentNullException(nameof(databaseBarrier));
            }
            if(waitTime == Timeout.InfiniteTimeSpan) {
                throw new ArgumentException(nameof(waitTime));
            }
            if(loggerFactory == null) {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            DatabaseBarrier = databaseBarrier;
            WaitTime = waitTime;
            PauseRetryTime = pauseRetryTime;
            Logger = loggerFactory.CreateLogger(GetType());

#pragma warning disable CS8622 // パラメーターの型における参照型の Null 許容性が、対象のデリゲートと一致しません。
            LazyTimer = new Timer(LazyCallback);
#pragma warning restore CS8622 // パラメーターの型における参照型の Null 許容性が、対象のデリゲートと一致しません。
        }

        #region property

        IDatabaseBarrier DatabaseBarrier { get; }
        TimeSpan WaitTime { get; }
        TimeSpan PauseRetryTime { get; }
        ILogger Logger { get; }

        Timer LazyTimer { get; }

        IList<LazyStockItem> StockItems { get; } = new List<LazyStockItem>();
        IDictionary<object, LazyStockItem> UniqueItems { get; } = new Dictionary<object, LazyStockItem>();

        public bool IsPausing { get; private set; }

        #endregion

        #region function

        void StopTimer()
        {
            LazyTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }
        void StartTimer()
        {
            LazyTimer.Change(WaitTime, PauseRetryTime);
        }

        void StockCore(Action<IDatabaseTransaction> action, object? uniqueKey)
        {
            lock(this._timerLocker) {
                StopTimer();

                // 既に登録されている処理が存在する場合は破棄しておく
                if(uniqueKey != null) {
                    if(UniqueItems.TryGetValue(uniqueKey, out var stockedItem)) {
                        Logger.LogTrace($"待機処理破棄: {stockedItem.StockTimestamp} {uniqueKey.GetHashCode()}");
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

        public void Stock(Action<IDatabaseTransaction> action)
        {
            ThrowIfDisposed();
            StockCore(action, null);
        }

        void LazyCallback(object state)
        {
            if(IsPausing) {
                return;
            }
            Flush();
        }

        void FlushCore(LazyStockItem[] stockItems)
        {
            using(var transaction = DatabaseBarrier.WaitWrite()) {
                foreach(var stockItem in stockItems) {
                    stockItem.Action(transaction);
                }
                transaction.Commit();
            }
        }

        public IDisposer Pause()
        {
            IsPausing = true;
            return new ActionDisposer(() => {
                IsPausing = false;
            });
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
                if(disposing) {
                    LazyTimer.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
