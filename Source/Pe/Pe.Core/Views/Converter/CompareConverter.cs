using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Core.Views.Converter
{
    public enum Compare
    {
        /// <summary>
        /// value == parameter
        /// </summary>
        Equal,
        /// <summary>
        /// value != parameter
        /// </summary>
        NotEqual,
        /// <summary>
        /// value &gt; parameter
        /// </summary>
        Greater,
        /// <summary>
        /// value &gt;= parameter
        /// </summary>
        GreaterEqual,
        /// <summary>
        /// value &lt; parameter
        /// </summary>
        Less,
        /// <summary>
        /// value &lt;= parameter
        /// </summary>
        LessEqual,
    }

    public abstract class CompareConverterBase<T>: IValueConverter
        where T : IComparable<T>
    {
        #region property

        public Compare Compare { get; set; }

        #endregion

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is T left) {
                var right = (T)System.Convert.ChangeType(parameter, typeof(T), CultureInfo.InvariantCulture);
                switch(Compare) {
                    case Compare.Equal:
                        return left.CompareTo(right) == 0;

                    case Compare.NotEqual:
                        return left.CompareTo(right) != 0;

                    case Compare.Greater:
                        return 0 < left.CompareTo(right);

                    case Compare.GreaterEqual:
                        return 0 < left.CompareTo(right) || left.CompareTo(right) == 0;

                    case Compare.Less:
                        return 0 < left.CompareTo(right);

                    case Compare.LessEqual:
                        return left.CompareTo(right) < 0 || left.CompareTo(right) == 0;

                    default:
                        throw new NotImplementedException();
                }
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    public class DoubleCompareConverter: CompareConverterBase<double>
    { }
}
