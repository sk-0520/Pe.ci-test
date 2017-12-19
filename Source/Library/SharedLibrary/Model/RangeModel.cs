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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF.ReadOnly;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
    /// <summary>
    /// 範囲持ちアイテム。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class RangeModel<T>: ModelBase, IReadOnlyRange<T>
        where T : IComparable
    {
        public RangeModel()
        { }

        public RangeModel(T head, T tail)
        {
            Head = head;
            Tail = tail;
        }

        #region property

        /// <summary>
        /// 範囲の開始点。
        /// </summary>
        [DataMember]
        public T Head { get; set; }
        /// <summary>
        /// 範囲の終了点。
        /// </summary>
        [DataMember]
        public T Tail { get; set; }

        #endregion
    }

    /// <summary>
    /// ヘルパ。
    /// </summary>
    public static class RangeModel
    {
        public static RangeModel<T> Create<T>(T head, T tail)
            where T : IComparable
        {
            return new RangeModel<T>(head, tail);
        }

        public static RangeModel<T> Parse<T>(string value)
            where T : IComparable
        {
            var values = value.Split(',');

            if(values.Length != 2) {
                throw new ArgumentException($"{nameof(value)}: illegal, {value}");
            }

            var rawRanges = values
                .Select(s => (T)Convert.ChangeType(s.Trim(), typeof(T)))
                .ToArray()
            ;

            return Create(rawRanges[0], rawRanges[1]);
        }

        public static bool TryParse<T>(string value, out RangeModel<T> result)
            where T : IComparable
        {
            try {
                result = Parse<T>(value);
                return true;
            } catch {
                result = default(RangeModel<T>);
                return false;
            }
        }

    }

}
