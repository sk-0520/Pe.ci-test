using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Core.Views.Converter
{
    [ValueConversion(typeof(long), typeof(string))]
    public class HumanLikeSizeConverter : IValueConverter
    {
        #region property

        public string SizeFormat { get; set; } = string.Empty;

        #endregion

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var size = System.Convert.ToInt64(value);

            var sizeConverter = new SizeConverter();

            if(string.IsNullOrWhiteSpace(SizeFormat)) {
                return sizeConverter.ConvertHumanLikeByte(size);
            }

            return sizeConverter.ConvertHumanLikeByte(size, SizeFormat);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
