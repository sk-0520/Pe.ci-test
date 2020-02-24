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

        public bool UndefinedIsRaw { get; set; }

        #endregion

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var resourceNameKind = ResourceNameKind.Normal;
            var param = (string)parameter;
            if(!string.IsNullOrWhiteSpace(param)) {
                if(!EnumUtility.TryParse(param, out resourceNameKind)) {
                    resourceNameKind = param.ToUpperInvariant() switch
                    {
                        "A" => ResourceNameKind.AccessKey,
                        _ => ResourceNameKind.Normal,
                    };
                }
            }

            return CultureService.GetString(value, resourceNameKind, UndefinedIsRaw);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
