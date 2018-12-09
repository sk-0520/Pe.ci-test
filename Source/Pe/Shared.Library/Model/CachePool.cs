using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public interface ICacheItem<TValue>
    {
        #region property

        /// <summary>
        /// キャッシュデータ。
        /// </summary>
        TValue Value { get; }

        /// <summary>
        /// 最終使用日時。
        /// </summary>
        DateTime AccessTimestamp { get; }

        /// <summary>
        /// キャッシュアイテムが <see cref="IDisposable"/>を実装している場合、キャッシュクリア時に <see cref="IDisposable.Dispose"/> を呼び出すか。
        /// </summary>
        bool IsManaged { get; }

        /// <summary>
        /// キャッシュ時間。
        /// </summary>
        TimeSpan LifeTime { get; }

        #endregion
    }

    public interface IWraitableCacheItem
    {
        DateTime AccessTimestamp { get; set; }
    }

    public static class ICacheItemExtensions
    {
        #region function

        public static bool CheckEnabled<TValue>(this ICacheItem<TValue> @this, DateTime timestamp)
        {
            return timestamp < @this.AccessTimestamp + @this.LifeTime;
        }

        #endregion
    }

    public class CacheItem<TValue> : ICacheItem<TValue>, IWraitableCacheItem
    {
        public CacheItem(TValue value, bool isManaged, TimeSpan lifeTime)
        {
            Value = value;
            IsManaged = isManaged;
            LifeTime = lifeTime;
        }

        #region ICacheItem

        public TValue Value { get; }

        public bool IsManaged { get; }

        #region IWraitableCacheItem

        public DateTime AccessTimestamp { get; set; }

        #endregion

        public TimeSpan LifeTime { get; set; }

        #endregion
    }

    public class CachePool<TKey, TValue>
    {
        public CachePool(TimeSpan defaultLifeTime)
        {
            DefaultLifeTime = defaultLifeTime;
        }

        #region property

        /// <summary>
        /// キャッシュの中身。
        /// </summary>
        ConcurrentDictionary<TKey, CacheItem<TValue>> Store { get; } = new ConcurrentDictionary<TKey, CacheItem<TValue>>();

        /// <summary>
        /// キャッシュ時間。
        /// </summary>
        public TimeSpan DefaultLifeTime { get; private set; }

        #endregion

        #region function

        protected CacheItem<TValue> CreateCacheItem(TValue value, bool isManaged, TimeSpan lifeTime)
        {
            var item = new CacheItem<TValue>(value, isManaged, lifeTime);
            return item;
        }

        void UpdateItemState(IWraitableCacheItem item)
        {
            item.AccessTimestamp = DateTime.Now;
        }

        void DisposeItem(ICacheItem<TValue> item)
        {
            if(item.IsManaged) {
                if(item is IDisposable diposer) {
                    diposer.Dispose();
                }
            }
        }

        bool DisposeStore(TKey key)
        {
            if(Store.TryRemove(key, out var item)) {
                DisposeItem(item);
                return true;
            }

            return false;
        }

        public TValue GetOrAdd(TKey key, bool isManaged, TimeSpan lifeTime, Func<TValue> valueFactory)
        {
            var item = Store.GetOrAdd(key, k => CreateCacheItem(valueFactory(), isManaged, lifeTime));

            if(!item.CheckEnabled(DateTime.Now)) {
                DisposeItem(item);
                // スレッドとか難しいことは考えないでござる
                Store[key] = CreateCacheItem(valueFactory(), isManaged, lifeTime);
            }

            UpdateItemState(item);
            return item.Value;
        }

        public TValue GetOrAdd(TKey key, bool isManaged, Func<TValue> valueFactory)
        {
            return GetOrAdd(key, isManaged, DefaultLifeTime, valueFactory);
        }

        public TValue GetOrAdd(TKey key, Func<TValue> valueFactory)
        {
            return GetOrAdd(key, true, valueFactory);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if(Store.TryGetValue(key, out var item)) {
                if(item.CheckEnabled(DateTime.Now)) {
                    if(item.CheckEnabled(DateTime.Now)) {
                        UpdateItemState(item);
                        value = item.Value;
                        return true;
                    }

                    DisposeStore(key);
                }
            }

            value = default(TValue);
            return false;
        }

        /// <summary>
        /// 生値取得。
        /// <para>注意: キャッシュコントロール配下には置かれない。</para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ICacheItem<TValue> GetItem(TKey key)
        {
            if(Store.TryGetValue(key, out var item)) {
                return item;
            }

            return null;
        }

        public bool Remove(TKey key)
        {
            return DisposeStore(key);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>破棄されたデータ数。</returns>
        public int Refresh()
        {
            var timestamp = DateTime.Now;
            var pairs = Store
                .ToList()
                .Where(p => p.Value.CheckEnabled(timestamp))
                .ToList()
            ;

            var removeCount = 0;

            foreach(var pair in pairs) {
                if(Store.TryRemove(pair.Key, out _)) {
                    DisposeItem(pair.Value);
                    removeCount += 1;
                }
            }

            return removeCount;
        }

        #endregion
    }
}
