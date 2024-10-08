using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// 参照中の値を管理。
    /// </summary>
    /// <typeparam name="TValue">参照対象型。</typeparam>
    internal class ReferenceItem<TValue>
        where TValue : class
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="value">参照値。</param>
        /// <param name="timeout">生存時間。</param>
        /// <param name="isManage"><see cref="IDisposable.Dispose"/> 管理対象か。</param>
        public ReferenceItem(TValue value, TimeSpan timeout, bool isManage)
        {
            Value = value;
            Timeout = timeout;
            IsManage = isManage;
            LifeTime = Stopwatch.StartNew();
        }

        #region property

        /// <summary>
        /// 参照値。
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// 生存時間。
        /// </summary>
        private Stopwatch LifeTime { get; set; }
        /// <summary>
        /// 死ぬまでの猶予。
        /// </summary>
        private TimeSpan Timeout { get; }
        /// <summary>
        /// <typeparamref name="TValue"/>が<see cref="IDisposable"/>なら<see cref="IDisposable.Dispose"/>の面倒を見てあげるか。
        /// </summary>
        public bool IsManage { get; }

        /// <summary>
        /// 生存中か。
        /// </summary>
        public bool Alive => LifeTime.Elapsed < Timeout;

        #endregion

        #region function

        /// <summary>
        /// 再使用可能にする。
        /// </summary>
        public void Recycle()
        {
            LifeTime.Restart();
        }

        #endregion
    }

    /// <summary>
    /// 参照をふわっと管理。
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ReferencePool<TKey, TValue>: DisposerBase
        where TKey : notnull
        where TValue : class
    {
        public ReferencePool(TimeSpan garbageTime, TimeSpan defaultTimeout, bool defaultManage, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            DefaultTimeout = defaultTimeout;
            DefaultManage = defaultManage;
            Timer = new Timer() {
                Interval = garbageTime.TotalMilliseconds,
            };
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();
        }

        #region property

        private ILogger Logger { get; }

        /// <summary>
        /// キャッシュの中身。
        /// </summary>
        private ConcurrentDictionary<TKey, ReferenceItem<TValue>> Store { get; } = new ConcurrentDictionary<TKey, ReferenceItem<TValue>>();

        /// <summary>
        /// キャッシュ時間。
        /// </summary>
        public TimeSpan DefaultTimeout { get; }
        public bool DefaultManage { get; }

        private Timer Timer { get; }

        #endregion

        #region function


        private TValue GetCore(TKey key, TimeSpan timeout, bool isManage, Func<TKey, TValue> creator)
        {
            static ReferenceItem<TValue> GetOrAdd(ConcurrentDictionary<TKey, ReferenceItem<TValue>> map, TKey key, TimeSpan timeout, bool isManage, Func<TKey, TValue> creator, ILogger logger)
            {
                return map.GetOrAdd(key, (key, args) => {
#if DEBUG
                    logger.LogTrace("参照アイテム生成: {0}", key);
#endif
                    var value = args.creator(key);
                    var item = new ReferenceItem<TValue>(value, args.timeout, args.isManage);
                    return item;
                }, (timeout, isManage, creator));
            }

            var result = GetOrAdd(Store, key, timeout, isManage, creator, Logger);
            lock(result) {
                if(result.Alive) {
#if DEBUG
                    Logger.LogTrace("参照アイテム生成/再使用: {0}", key);
#endif
                    result.Recycle();
                    return result.Value;
                }
            }

            if(Store.TryRemove(key, out var livingDead)) {
                IncinerateCore(key, livingDead);
            }

            return GetOrAdd(Store, key, timeout, isManage, creator, Logger).Value;
        }

        public TValue Get(TKey key, Func<TKey, TValue> creator)
        {
            return GetCore(key, DefaultTimeout, DefaultManage, creator);
        }
        public TValue Get(TKey key, TimeSpan timeout, Func<TKey, TValue> creator)
        {
            return GetCore(key, timeout, DefaultManage, creator);
        }
        public TValue Get(TKey key, bool isManage, Func<TKey, TValue> creator)
        {
            return GetCore(key, DefaultTimeout, isManage, creator);
        }
        public TValue Get(TKey key, TimeSpan timeout, bool isManage, Func<TKey, TValue> creator)
        {
            return GetCore(key, timeout, isManage, creator);
        }

        /// <summary>
        /// 破棄処理。
        /// </summary>
        /// <param name="item">呼び出し側で lock すること。</param>
        private void IncinerateCore(TKey key, ReferenceItem<TValue> item)
        {
            if(item.Alive) {
                return;
            }

#if DEBUG
#pragma warning disable HAA0101 // Array allocation for params parameter
#pragma warning disable HAA0601 // Value type to reference type conversion causing boxing allocation
            Logger.LogTrace("参照アイテム削除: {0}", key);
#pragma warning restore HAA0601 // Value type to reference type conversion causing boxing allocation
#pragma warning restore HAA0101 // Array allocation for params parameter
#endif
            if(!item.IsManage) {
                return;
            }

            if(item.Value is IDisposable disposer) {
#if DEBUG
#pragma warning disable HAA0101 // Array allocation for params parameter
#pragma warning disable HAA0601 // Value type to reference type conversion causing boxing allocation
                Logger.LogTrace("参照アイテム破棄: {0}", key);
#pragma warning restore HAA0601 // Value type to reference type conversion causing boxing allocation
#pragma warning restore HAA0101 // Array allocation for params parameter
#endif
                disposer.Dispose();
            }
        }

        public void Refresh()
        {
            Logger.LogTrace("参照アイテム削除一括削除開始"); // こいつは一応残しておく
            var pairs = Store.Where(i => !i.Value.Alive).ToArray();
            foreach(var pair in pairs) {
                lock(pair.Value) {
                    IncinerateCore(pair.Key, pair.Value);
                }
            }
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Timer.Elapsed -= Timer_Elapsed;
                if(disposing) {
                    Timer.Dispose();
                    foreach(var pair in Store) {
                        // ここでロックする事態は状態がおかしいと思う
                        IncinerateCore(pair.Key, pair.Value);
                    }

                }
            }
            base.Dispose(disposing);
        }

        #endregion

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Timer.Stop();
            try {
                Refresh();
            } finally {
                Timer.Start();
            }
        }
    }
}
