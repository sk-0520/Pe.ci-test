using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models;

namespace ContentTypeTextNet.Pe.Main.Views.Converter
{
    public class EnumToCultureConverter : IValueConverter
    {
        #region property

        public CultureService CultureService { get; set; } = CultureService.Current;

        #endregion

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var resourceNameKind = ResourceNameKind.Normal;
            var param = (string)parameter;
            if(!string.IsNullOrWhiteSpace(param)) {
                EnumUtility.TryParse(param, out resourceNameKind);
            }

            return CultureService.GetString(value, resourceNameKind);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
