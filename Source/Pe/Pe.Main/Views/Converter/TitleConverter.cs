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


            var footer = new StringBuilder();
            if(BuildStatus.BuildType != BuildType.Release) {
                footer
                    .Append(' ')
                    .Append(new VersionConverter().ConvertNormalVersion(BuildStatus.Version))
                    .Append(" APP:")
                    .Append(ProcessArchitecture.ApplicationArchitecture)
                    .Append("/OS:")
                    .Append(ProcessArchitecture.PlatformArchitecture)
                    .Append(" <")
                    .Append(BuildStatus.Revision)
                    .Append('>')
                ;
            }

            return new StringBuilder()
                .Append(header)
                .Append(caption)
                .Append(footer)
                .Append(" - ")
                .Append(BuildStatus.Name)
                .ToString()
            ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new NotSupportedException();
        }

        #endregion
    }
}
