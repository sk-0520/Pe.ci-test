using System;
using System.Globalization;
using System.Windows.Data;

namespace ContentTypeTextNet.Pe.Core.Views.Converter
{
    [ValueConversion(typeof(string), typeof(string))]
    public class EscapeAccessKeyConverter: IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = (string)value;
            if(string.IsNullOrWhiteSpace(s)) {
                return s;
            }
            return s.Replace("_", "__");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = (string)value;
            if(string.IsNullOrWhiteSpace(s)) {
                return s;
            }
            return s.Replace("__", "_");
        }

        #endregion
    }
}
