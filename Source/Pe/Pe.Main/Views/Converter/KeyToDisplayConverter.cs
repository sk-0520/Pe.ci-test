using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Main.Models.KeyMapping;

namespace ContentTypeTextNet.Pe.Main.Views.Converter
{
    class KeyToDisplayConverter : IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var keyViewConverter = new KeyViewConverter();
            return keyViewConverter.ConvertFromKey((Key)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
