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
        /// <c>value == parameter</c>
        /// </summary>
        Equal,
        /// <summary>
        /// <c>value != parameter</c>
        /// </summary>
        NotEqual,
        /// <summary>
        /// <c>value &gt; parameter</c>
        /// </summary>
        Greater,
        /// <summary>
        /// <c>value &gt;= parameter</c>
        /// </summary>
        GreaterEqual,
        /// <summary>
        /// <c>value &lt; parameter</c>
        /// </summary>
        Less,
        /// <summary>
        /// <c>value &lt;= parameter</c>
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
                var right = (T)System.Convert.ChangeType(parameter, typeof(T), culture);
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
                        return left.CompareTo(right) < 0;

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
