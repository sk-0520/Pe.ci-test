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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Data;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Extension
{
    public static class LinqExtension
    {
        /// <summary>
        /// シーケンスを真偽値により処理を分岐させる。
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="seq">入力シーケンス</param>
        /// <param name="cond">条件</param>
        /// <param name="t">真の場合に返すシーケンス</param>
        /// <returns></returns>
        public static IEnumerable<TSource> If<TSource>(this IEnumerable<TSource> seq, bool cond, Func<IEnumerable<TSource>, IEnumerable<TSource>> t)
        {
            if(cond) {
                return t(seq);
            } else {
                return seq;
            }
        }

        /// <summary>
        /// シーケンスを真偽値により処理を分岐させる
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="seq">入力シーケンス</param>
        /// <param name="cond">条件</param>
        /// <param name="t">真の場合に返すシーケンス</param>
        /// <param name="f">偽の場合に返すシーケンス</param>
        /// <returns></returns>
        public static IEnumerable<TResult> IfElse<TSource, TResult>(this IEnumerable<TSource> seq, bool cond, Func<IEnumerable<TSource>, IEnumerable<TResult>> t, Func<IEnumerable<TSource>, IEnumerable<TResult>> f)
        {
            if(cond) {
                return t(seq);
            } else {
                return f(seq);
            }
        }

        /// <summary>
        /// 条件が真の場合にシーケンスを反転させる。
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="seq"></param>
        /// <param name="cond"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> IfRevese<TSource>(this IEnumerable<TSource> seq, bool cond)
        {
            return IfElse(seq, cond, s => s.Reverse(), s => s);
        }

        public static IEnumerable<TSource> IfOrderByAsc<TSource, TKey>(this IEnumerable<TSource> seq, Func<TSource, TKey> keySelector, bool orderByAsc)
        {
            return IfElse(seq, orderByAsc, s => s.OrderBy(keySelector), s => s.OrderByDescending(keySelector));
        }

        /// <summary>
        /// <para>http://stackoverflow.com/questions/13767451/ilistt-findindexint32-predicate-t?answertab=votes#tab-top</para>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static int FindIndex<TSource>(this IList<TSource> source, int startIndex, Predicate<TSource> match)
        {
            // TODO: Validation
            for(int i = startIndex; i < source.Count; i++) {
                if(match(source[i])) {
                    return i;
                }
            }
            return -1;
        }

        public static int FindIndex<TSource>(this IList<TSource> source, Predicate<TSource> match)
        {
            return FindIndex(source, 0, match);
        }

        public static int FindIndex<TSource>(this ObservableCollection<TSource> source, int startIndex, Predicate<TSource> match)
        {
            // TODO: Validation
            for(int i = startIndex; i < source.Count; i++) {
                if(match(source[i])) {
                    return i;
                }
            }
            return -1;
        }

        public static int FindIndex<TSource>(this ObservableCollection<TSource> source, Predicate<TSource> match)
        {
            return FindIndex(source, 0, match);
        }

        public static IEnumerable<IndexValue<TSource>> SelectValueIndex<TSource>(this IEnumerable<TSource> source)
        {
            return source.Select((v, i) => new IndexValue<TSource>(v, i));
        }
    }
}
