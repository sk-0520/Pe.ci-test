using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ContentTypeTextNet.Pe.Main.Views.Converter
{
    [ValueConversion(typeof(TimeSpan), typeof(double))]
    public class TimeSpanToMillisecondsConverter: IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is TimeSpan timeSpan) {
                return timeSpan.TotalMilliseconds;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is double milliseconds) {
                return TimeSpan.FromMilliseconds(milliseconds);
            }

            return DependencyProperty.UnsetValue;
        }

        #endregion
    }
}
