using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
    public class LazyStockItem
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

        //ConcurrentQueue 使うことも考えたけどどうせタイマーのロックあるし普通版
        //TODO: Queue である必要あんのか？
        Queue<LazyStockItem> StockItems { get; } = new Queue<LazyStockItem>();

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

        public void Stock(Action<ApplicationDatabaseBarrierTransaction> action)
        {
            ThrowIfDisposed();

            lock(this._timerLocker) {
                StopTimer();

                StockItems.Enqueue(new LazyStockItem(action));

                StartTimer();
            }
        }

        void LazyCallback(object state)
        {
            Flush();
        }

        void FlushCore(IEnumerable<LazyStockItem> stockItems)
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
