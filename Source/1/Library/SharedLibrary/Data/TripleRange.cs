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
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.IF.ReadOnly;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
    /// <summary>
    /// 最小値・中間値・最大値を保持するデータ。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct TripleRange<T>: IReadOnlyRange<T>
        where T : IComparable
    {
        #region variable

        // 互換用。
        public readonly T minimum, median, maximum;

        #endregion

        public TripleRange(T minimum, T median, T maximum)
        {
            this.minimum = minimum;
            this.median = median;
            this.maximum = maximum;
        }

        #region property

        public T Minimum => this.minimum;
        public T Median => this.median;
        public T Maximum => this.maximum;

        #endregion

        #region IReadOnlyRange

        public T Head => Minimum;
        public T Tail => Maximum;

        #endregion
    }

    /// <summary>
    /// ラッパー。
    /// </summary>
    public static class TripleRange
    {
        public static TripleRange<T> Create<T>(T minimum, T median, T maximum)
            where T : IComparable
        {
            return new TripleRange<T>(minimum, median, maximum);
        }

        /// <summary>
        /// パース処理。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TripleRange<T> Parse<T>(string value)
            where T : IComparable
        {
            var values = value.Split(',');

            if(values.Length != 3) {
                throw new ArgumentException($"{nameof(value)}: illegal, {value}");
            }

            var rawRanges = values
                .Select(s => (T)Convert.ChangeType(s.Trim(), typeof(T)))
                .ToArray()
            ;

            return Create(rawRanges[0], rawRanges[1], rawRanges[2]);
        }

        public static bool TryParse<T>(string value, out TripleRange<T> result)
            where T : IComparable
        {
            try {
                result = Parse<T>(value);
                return true;
            } catch {
                result = default(TripleRange<T>);
                return false;
            }
        }

    }
}
