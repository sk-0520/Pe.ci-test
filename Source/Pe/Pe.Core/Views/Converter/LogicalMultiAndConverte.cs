using System;
using System.Linq;
using System.Windows.Data;

namespace ContentTypeTextNet.Pe.Core.Views.Converter
{
    public class LogicalMultiAndConverter: IMultiValueConverter
    {
        #region IMultiValueConverter

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var targets = values.OfType<bool>().ToArray();
            if(targets.Length == values.Length) {
                return targets.All(v => v);
            } else {
                return false;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
