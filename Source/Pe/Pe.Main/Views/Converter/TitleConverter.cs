using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;

namespace ContentTypeTextNet.Pe.Main.Views.Converter
{
    public class TitleConverter : IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var caption = (string)value ?? string.Empty;
            var header = BuildStatus.BuildType switch
            {
                BuildType.Debug => "[DEBUG] ",
                BuildType.Beta => "[β] ",
                BuildType.Release => string.Empty,
                _ => "💩 "
            };
            return header + caption;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new NotSupportedException();
        }

        #endregion
    }
}
