using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public interface IReadOnlyMinMax<T>
        where T : IComparable
    {
        #region property

        T Minimum { get; }
        T Maximum { get; }

        #endregion
    }

    /// <summary>
    /// 範囲持ちアイテム。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable, DataContract]
    public struct MinMax<T> : IReadOnlyMinMax<T>
        where T : IComparable
    {
        public MinMax(T minimum, T maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        #region IReadOnlyRange

        /// <summary>
        /// 範囲の開始点。
        /// </summary>
        [DataMember]
        public T Minimum { get; set; }
        /// <summary>
        /// 範囲の終了点。
        /// </summary>
        [DataMember]
        public T Maximum { get; set; }

        #endregion

    }

    /// <summary>
    /// ヘルパ。
    /// </summary>
    public static class MinMax
    {
        public static MinMax<T> Create<T>(T head, T tail)
            where T : IComparable
        {
            return new MinMax<T>(head, tail);
        }

        public static MinMax<T> Parse<T>(string value)
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

        public static bool TryParse<T>(string value, out MinMax<T> result)
            where T : IComparable
        {
            try {
                result = Parse<T>(value);
                return true;
            } catch {
                result = default(MinMax<T>);
                return false;
            }
        }

    }

    public interface IReadOnlyMinMaxDefault<T>: IReadOnlyMinMax<T>
        where T : IComparable
    {
        #region property

        T Default { get; }

        #endregion
    }

    public struct MinMaxDefault<T> : IReadOnlyMinMaxDefault<T>
        where T : IComparable
    {
        public MinMaxDefault(T minimum, T maximum, T defaultValue)
        {
            Minimum = minimum;
            Maximum = maximum;
            Default = defaultValue;
        }

        #region property

        /// <summary>
        /// 範囲の開始点。
        /// </summary>
        [DataMember]
        public T Minimum { get; set; }
        /// <summary>
        /// 範囲の終了点。
        /// </summary>
        [DataMember]
        public T Maximum { get; set; }

        [DataMember]
        public T Default { get; set; }

        #endregion
    }

    /// <summary>
    /// ヘルパ。
    /// </summary>
    public static class MinMaxDefault
    {
        public static MinMaxDefault<T> Create<T>(T minimum, T maximum, T defaultValue)
            where T : IComparable
        {
            return new MinMaxDefault<T>(minimum, maximum, defaultValue);
        }

        public static MinMaxDefault<T> Parse<T>(string value)
            where T : IComparable
        {
            var values = value.Split(',');

            if(values.Length != 3) {
                throw new ArgumentException($"{nameof(value)}: illegal, {value}");
            }

            var rawValues = values
                .Select(s => (T)Convert.ChangeType(s.Trim(), typeof(T))!)
                .ToArray()
            ;

            return Create(rawValues[0], rawValues[1], rawValues[2]);
        }

        public static bool TryParse<T>(string value, out MinMaxDefault<T> result)
            where T : IComparable
        {
            try {
                result = Parse<T>(value);
                return true;
            } catch {
                result = default(MinMaxDefault<T>);
                return false;
            }
        }

    }

    public static class MinMaxExtensions
    {
        #region function

        /// <summary>
        /// 指定された値が範囲内にあるか。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsIn<T>(this IReadOnlyMinMax<T> range, T value)
            where T : IComparable
        {
            return 0 <= value.CompareTo(range.Minimum) && value.CompareTo(range.Maximum) <= 0;
        }

        #endregion
    }
}
