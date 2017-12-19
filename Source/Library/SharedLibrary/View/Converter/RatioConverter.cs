using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ContentTypeTextNet.Library.SharedLibrary.View.Converter
{
    /// <summary>
    /// 値と比率(ConverterParameter)から掛け合わせた値を取得。
    /// </summary>
    [ValueConversion(typeof(double), typeof(double))]
    public class RatioConverter: IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var baseValue = (double)value;
            var percent = System.Convert.ToDouble(parameter);

            return baseValue * percent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
