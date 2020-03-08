using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows.Data;
using ContentTypeTextNet.Pe.Main.Models;
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
                BuildType.Release => string.Empty,
                _ => "[" + BuildStatus.BuildType.ToString() + "] ",
            };

            var footer = BuildStatus.BuildType switch
            {
                BuildType.Release => string.Empty,
                _ => " " + new VersionConverter().ConvertNormalVersion(BuildStatus.Version) + " APP:" +  ProcessArchitecture.ApplicationArchitecture + "/OS:" + ProcessArchitecture.PlatformArchitecture + " <" + BuildStatus.Revision + ">",
            };

            return header + caption + footer + " - " + BuildStatus.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new NotSupportedException();
        }

        #endregion
    }
}
