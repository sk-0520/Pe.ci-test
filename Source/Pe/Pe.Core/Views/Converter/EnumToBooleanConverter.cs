using System;
using System.Windows.Data;

namespace ContentTypeTextNet.Pe.Core.Views.Converter
{
    /// <summary>
    /// <para>http://stackoverflow.com/questions/397556/how-to-bind-radiobuttons-to-an-enum</para>
    /// </summary>
    public class EnumToBooleanConverter: IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value == null) {
                return false;
            }
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }

        #endregion
    }
}
