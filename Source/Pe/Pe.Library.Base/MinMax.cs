using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Base
{
    /// <summary>
    /// 読み込み専用範囲持ちアイテム。
    /// </summary>
    public interface IReadOnlyMinMax<out T>
        where T : IComparable<T>
    {
        #region property

        /// <summary>
        /// 範囲の開始点。
        /// </summary>
        T Minimum { get; }

        /// <summary>
        /// 範囲の終了点。
        /// </summary>
        T Maximum { get; }

        #endregion
    }

    /// <summary>
    /// 範囲持ちアイテム。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable, DataContract]
    public struct MinMax<T>: IReadOnlyMinMax<T>
        where T : IComparable<T>
    {
        public MinMax(T minimum, T maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        #region IReadOnlyMinMax

        [DataMember]
        public T Minimum { get; set; }
        [DataMember]
        public T Maximum { get; set; }

        #endregion
    }

    /// <summary>
    /// <see cref="MinMax{T}"/> ヘルパ。
    /// </summary>
    public static class MinMax
    {
        public static MinMax<T> Create<T>(T head, T tail)
            where T : IComparable<T>
        {
            return new MinMax<T>(head, tail);
        }

        public static MinMax<T> Parse<T>(string value, IFormatProvider provider)
            where T : IComparable<T>
        {
            //BUGS: ロケールに合わせる必要あり
            //      追記: 合わせる必要があるか？
            var values = value.Split(',');

            if(values.Length != 2) {
                throw new ArgumentException($"{nameof(value)}: illegal, {value}", nameof(value));
            }

            var rawRanges = values
                .Select(s => (T)Convert.ChangeType(s.Trim(), typeof(T), provider)!)
                .ToArray()
            ;

            return Create(rawRanges[0], rawRanges[1]);
        }

        public static MinMax<T> Parse<T>(string value)
            where T : IComparable<T>
        {
            return Parse<T>(value, CultureInfo.InvariantCulture);
        }

        public static bool TryParse<T>(string value, IFormatProvider provider, out MinMax<T> result)
            where T : IComparable<T>
        {
            try {
                result = Parse<T>(value, provider);
                return true;
            } catch {
                result = default;
                return false;
            }
        }

        public static bool TryParse<T>(string value, out MinMax<T> result)
            where T : IComparable<T>
        {
            return TryParse(value, CultureInfo.InvariantCulture, out result);
        }
    }

    /// <summary>
    /// 読み込み専用範囲・標準値もちアイテム。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlyMinMaxDefault<out T>: IReadOnlyMinMax<T>
        where T : IComparable<T>
    {
        #region property

        /// <summary>
        /// 標準値。
        /// </summary>
        T Default { get; }

        #endregion
    }

    /// <summary>
    /// 範囲・標準値持ちアイテム。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable, DataContract]
    public struct MinMaxDefault<T>: IReadOnlyMinMaxDefault<T>
        where T : IComparable<T>
    {
        public MinMaxDefault(T minimum, T maximum, T defaultValue)
        {
            Minimum = minimum;
            Maximum = maximum;
            Default = defaultValue;
        }

        #region IReadOnlyMinMaxDefault

        [DataMember]
        public T Minimum { get; set; }
        [DataMember]
        public T Maximum { get; set; }

        [DataMember]
        public T Default { get; set; }

        #endregion
    }

    /// <summary>
    /// <see cref="MinMaxDefault{T}"/>/> ヘルパ。
    /// </summary>
    public static class MinMaxDefault
    {
        public static MinMaxDefault<T> Create<T>(T minimum, T maximum, T defaultValue)
            where T : IComparable<T>
        {
            return new MinMaxDefault<T>(minimum, maximum, defaultValue);
        }

        public static MinMaxDefault<T> Parse<T>(string value, IFormatProvider provider)
            where T : IComparable<T>
        {
            var values = value.Split(',');

            if(values.Length != 3) {
                throw new ArgumentException($"{nameof(value)}: illegal, {value}", nameof(value));
            }

            var rawValues = values
                .Select(s => (T)Convert.ChangeType(s.Trim(), typeof(T), provider)!)
                .ToArray()
            ;

            return Create(rawValues[0], rawValues[1], rawValues[2]);
        }

        public static MinMaxDefault<T> Parse<T>(string value)
            where T : IComparable<T>
        {
            return Parse<T>(value, CultureInfo.InvariantCulture);
        }

        public static bool TryParse<T>(string value, IFormatProvider provider, out MinMaxDefault<T> result)
            where T : IComparable<T>
        {
            try {
                result = Parse<T>(value, provider);
                return true;
            } catch {
                result = default;
                return false;
            }
        }

        public static bool TryParse<T>(string value, out MinMaxDefault<T> result)
            where T : IComparable<T>
        {
            return TryParse(value, CultureInfo.InvariantCulture, out result);
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
            where T : IComparable<T>
        {
            return 0 <= value.CompareTo(range.Minimum) && value.CompareTo(range.Maximum) <= 0;
        }

        #endregion
    }
}
