using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;

namespace ContentTypeTextNet.Pe.Main.Model.Logic
{
    class LazyStockItem
    {
        public LazyStockItem(Action<ApplicationDatabaseBarrierTransaction> action)
        {
            Action = action;
        }

        #region property

        public Action<ApplicationDatabaseBarrierTransaction> Action { get; }
        [Timestamp(DateTimeKind.Unspecified)]
        public DateTime StockTimestamp { get; } = DateTime.UtcNow;

        #endregion
    }

    public class UniqueKeyPool
    {
        public UniqueKeyPool()
        { }

        #region property

        ConcurrentDictionary<string, object> Pool { get; } = new ConcurrentDictionary<string, object>();

        #endregion

        #region function

        public object Get([CallerMemberName] string callerMemberName = default(string), [CallerLineNumber] int callerLineNumber = -1)
        {
            var sb = new StringBuilder(callerMemberName.Length + 1 + callerLineNumber);
            sb.Append(callerMemberName);
            sb.Append('.');
            sb.Append(callerLineNumber);

            var result = Pool.GetOrAdd(sb.ToString(), k => new object());
            return result;
        }

        #endregion
    }

    public class DatabaseLazyWriter : DisposerBase
    {
        #region variable

        object _timerLocker = new object();

        #endregion

        public DatabaseLazyWriter(IApplicationDatabaseBarrier databaseBarrier, TimeSpan waitTime, ILoggerFactory loggerFactory)
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
            Logger = loggerFactory.CreateTartget(GetType());

            LazyTimer = new Timer(LazyCallback);
        }

        #region property

        IApplicationDatabaseBarrier DatabaseBarrier { get; }
        TimeSpan WaitTime { get; }
        ILogger Logger { get; }

        Timer LazyTimer { get; }

        IList<LazyStockItem> StockItems { get; } = new List<LazyStockItem>();
        IDictionary<object, LazyStockItem> UniqueItems { get; } = new Dictionary<object, LazyStockItem>();

        #endregion

        #region function

        void StopTimer()
        {
            LazyTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }
        void StartTimer()
        {
            LazyTimer.Change(WaitTime, Timeout.InfiniteTimeSpan);
        }

        void StockCore(Action<ApplicationDatabaseBarrierTransaction> action, object uniqueKey)
        {
            lock(this._timerLocker) {
                StopTimer();

                // 既に登録されている処理が存在する場合は破棄しておく
                if(uniqueKey != null) {
                    if(UniqueItems.TryGetValue(uniqueKey, out var stockedItem)) {
                        Logger.Trace($"待機処理破棄: {stockedItem.StockTimestamp} {uniqueKey.GetHashCode()}");
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

        public void Stock(Action<ApplicationDatabaseBarrierTransaction> action, object uniqueKey)
        {
            if(uniqueKey == null) {
                throw new ArgumentNullException(nameof(uniqueKey));
            }

            ThrowIfDisposed();

            StockCore(action, uniqueKey);
        }

        public void Stock(Action<ApplicationDatabaseBarrierTransaction> action)
        {
            ThrowIfDisposed();
            StockCore(action, null);
        }

        void LazyCallback(object state)
        {
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

        public void Flush()
        {
            ThrowIfDisposed();

            LazyStockItem[] items;
            lock(this._timerLocker) {
                StopTimer();
                items = StockItems.ToArray();
                StockItems.Clear();
                UniqueItems.Clear();
            }

            if(items.Length == 0) {
                return;
            }

            FlushCore(items);
        }


        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Flush();

                if(disposing) {
                    LazyTimer.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
