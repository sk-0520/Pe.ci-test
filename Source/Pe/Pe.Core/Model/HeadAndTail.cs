using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Model
{
    public interface IReadOnlyHeadAndTail<T>
        where T : IComparable
    {
        #region property

        T Head { get; }
        T Tail { get; }

        #endregion
    }

    /// <summary>
    /// 範囲持ちアイテム。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable, DataContract]
    public struct HeadAndTail<T> : IReadOnlyHeadAndTail<T>
        where T : IComparable
    {
        public HeadAndTail(T head, T tail)
        {
            Head = head;
            Tail = tail;
        }

        #region IReadOnlyRange

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
    public static class HeadAndTail
    {
        public static HeadAndTail<T> Create<T>(T head, T tail)
            where T : IComparable
        {
            return new HeadAndTail<T>(head, tail);
        }

        public static HeadAndTail<T> Parse<T>(string value)
            where T : IComparable
        {
            var values = value.Split(',');

            if(values.Length != 2) {
                throw new ArgumentException($"{nameof(value)}: illegal, {value}");
            }

            var rawRanges = values
                .Select(s => (T)Convert.ChangeType(s.Trim(), typeof(T))!)
                .ToArray()
            ;

            return Create(rawRanges[0], rawRanges[1]);
        }

        public static bool TryParse<T>(string value, out HeadAndTail<T> result)
            where T : IComparable
        {
            try {
                result = Parse<T>(value);
                return true;
            } catch {
                result = default(HeadAndTail<T>);
                return false;
            }
        }

    }

    public static class RangeExtensions
    {
        #region function

        /// <summary>
        /// 指定された値が範囲内にあるか・。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsIn<T>(this IReadOnlyHeadAndTail<T> range, T value)
            where T : IComparable
        {
            return 0 <= value.CompareTo(range.Head) && value.CompareTo(range.Tail) <= 0;
        }

        #endregion
    }
}
