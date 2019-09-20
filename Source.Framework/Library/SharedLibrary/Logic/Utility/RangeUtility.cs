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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF.ReadOnly;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
    public static class RangeUtility
    {
        /// <summary>
        /// min &lt;= value &lt;= max
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool Between<TValue>(TValue value, TValue min, TValue max)
            where TValue : IComparable
        {
            return min.CompareTo(value) <= 0 && 0 <= max.CompareTo(value);
        }

        /// <summary>
        /// 丸め。
        /// <para>valueがmin未満かmaxより多ければminかmaxの適応する方に丸める。</para>
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static TValue Clamp<TValue>(TValue value, TValue min, TValue max)
            where TValue : IComparable
        {
            if(min.CompareTo(value) > 0) {
                return min;
            } else if(max.CompareTo(value) < 0) {
                return max;
            } else {
                return value;
            }
        }

        public static TValue Clamp<TValue>(TValue value, IReadOnlyRange<TValue> range)
            where TValue : IComparable
        {
            return Clamp(value, range.Head, range.Tail);
        }

        /// <summary>
        /// それっぽくインクリメント。
        /// </summary>
        /// <param name="value"></param>
        /// <returns>最大値であればそのまま返す。</returns>
        public static int Increment(int value)
        {
            if(value == int.MaxValue) {
                return value;
            }

            return value + 1;
        }

        /// <summary>
        /// それっぽくインクリメント。
        /// </summary>
        /// <param name="value"></param>
        /// <returns>最大値であればそのまま返す。</returns>
        public static uint Increment(uint value)
        {
            if(value == uint.MaxValue) {
                return value;
            }

            return value + 1;
        }

        /// <summary>
        /// それっぽくインクリメント。
        /// </summary>
        /// <param name="value"></param>
        /// <returns>最大値であればそのまま返す。</returns>
        public static short Increment(short value)
        {
            if(value == short.MaxValue) {
                return value;
            }

            return (short)(value + 1);
        }

        /// <summary>
        /// それっぽくインクリメント。
        /// </summary>
        /// <param name="value"></param>
        /// <returns>最大値であればそのまま返す。</returns>
        public static ushort Increment(ushort value)
        {
            if(value == ushort.MaxValue) {
                return value;
            }

            return (ushort)(value + 1);
        }

        /// <summary>
        /// それっぽくインクリメント。
        /// </summary>
        /// <param name="value"></param>
        /// <returns>最大値であればそのまま返す。</returns>
        public static byte Increment(byte value)
        {
            if(value == byte.MaxValue) {
                return value;
            }

            return (byte)(value + 1);
        }

    }
}
