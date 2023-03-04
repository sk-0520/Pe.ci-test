using System;
using System.Windows.Data;

namespace ContentTypeTextNet.Pe.Core.Views.Converter
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class LogicalNotConverter: IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }

        #endregion
    }
}
