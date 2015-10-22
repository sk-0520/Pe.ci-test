/**
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
namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ContentTypeTextNet.Library.SharedLibrary.IF;

    /// <summary>
    /// 生成データを保持しておく。
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class Caching<TKey, TValue>: IEnumerable<KeyValuePair<TKey, TValue>>
    {
        public Caching()
        {
            Cache = new Dictionary<TKey, TValue>();
        }

        #region property

        protected Dictionary<TKey, TValue> Cache { get; private set; }

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

        public bool ClearCache(TKey key)
        {
            TValue result;
            if(!Cache.TryGetValue(key, out result)) {
                return Cache.Remove(key);
            } else {
                return false;
            }
        }

        public void Clear()
        {
            foreach(var key in Cache.Keys) {
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
}
