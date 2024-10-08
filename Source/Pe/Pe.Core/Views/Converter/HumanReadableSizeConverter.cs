using System;
using System.Globalization;
using System.Windows.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Core.Views.Converter
{
    [ValueConversion(typeof(long), typeof(string))]
    public class HumanReadableSizeConverter: IValueConverter
    {
        #region property

        public string SizeFormat { get; set; } = string.Empty;

        #endregion

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var size = System.Convert.ToInt64(value, CultureInfo.InvariantCulture);

            var sizeConverter = new SizeConverter();

            if(string.IsNullOrWhiteSpace(SizeFormat)) {
                return sizeConverter.ConvertHumanReadableByte(size);
            }

            return sizeConverter.ConvertHumanReadableByte(size, SizeFormat);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
