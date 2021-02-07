using System;
using System.Globalization;
using System.Windows.Data;

namespace ContentTypeTextNet.Pe.Main.Views.Converter
{
    public class SettingCultureConverter: IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is "") {
                return System.Windows.DependencyProperty.UnsetValue;
            }
            var member = (string)parameter;
            var isDisplayName = member == "DisplayName";
            var cultureInfo = (CultureInfo)value;
            if(cultureInfo == CultureInfo.InvariantCulture) {
                return isDisplayName
                    ? Properties.Resources.String_Converter_SettingCultureNameConverter_DisplayName_Auto
                    : Properties.Resources.String_Converter_SettingCultureNameConverter_Name_Auto
                ;
            }

            return isDisplayName
                ? cultureInfo.DisplayName
                : cultureInfo.Name
            ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
