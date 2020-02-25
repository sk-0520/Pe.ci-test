/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    /// <summary>
    /// 生成データを保持しておく。
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    internal class Cacher<TKey, TValue>: IEnumerable<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        ///非スレッドセーフでキャッシュ構築。
        /// </summary>
        public Cacher()
            : this(false)
        { }

        /// <summary>
        /// キャッシュ構築。
        /// </summary>
        /// <param name="isSynchronized">スレッドセーフにするか。</param>
        public Cacher(bool isSynchronized)
        {
            IsSynchronized = isSynchronized;

            if(IsSynchronized) {
                Cache = new ConcurrentDictionary<TKey, TValue>();
            } else {
                Cache = new Dictionary<TKey, TValue>();
            }
        }

        #region property

        /// <summary>
        /// スレッドセーフか。
        /// </summary>
        public bool IsSynchronized { get; }

        /// <summary>
        /// キャッシュデータ。
        /// </summary>
        protected IDictionary<TKey, TValue> Cache { get; private set; }

        #endregion

        #region function

        /// <summary>
        /// 指定キーからデータを取得する。
        /// <para>指定キーにデータがなければデータを生成してキャッシュに入れる。</para>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public TValue Get(TKey key, Func<TValue> creator)
        {
            TValue result;
            if(!Cache.TryGetValue(key, out result)) {
                result = creator();
                Cache[key] = result;
            }

            return result;
        }

        public void Add(TKey key, TValue value)
        {
            Cache.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return Cache.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return Cache.TryGetValue(key, out value);
        }

        /// <summary>
        /// 指定キーのキャッシュをクリア。
        /// </summary>
        /// <param name="key"></param>
        /// <returns>クリア出来れば真。</returns>
        public bool ClearCache(TKey key)
        {
            TValue result;
            if(Cache.TryGetValue(key, out result)) {
                return Cache.Remove(key);
            } else {
                return false;
            }
        }

        /// <summary>
        /// 全キャッシュデータのクリア。
        /// </summary>
        public void Clear()
        {
            foreach(var key in Cache.Keys.ToArray()) {
                ClearCache(key);
            }

            Cache.Clear();
        }

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return Cache.GetEnumerator();
        }

        #endregion
    }

    [Obsolete("old: Cacher")]
    internal class Caching<TKey, TValue>: Cacher<TKey, TValue>
    {
        /// <summary>
        ///非スレッドセーフでキャッシュ構築。
        /// </summary>
        public Caching()
            : this(false)
        { }

        /// <summary>
        /// キャッシュ構築。
        /// </summary>
        /// <param name="isSynchronized">スレッドセーフにするか。</param>
        public Caching(bool isSynchronized)
            : base(isSynchronized)
        { }

    }
}
