using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Views.Converter
{
    [ValueConversion(typeof(FontFamily), typeof(string))]
    public class FontFamilyToNameConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is FontFamily fontFamily) {
                var currentLang = XmlLanguage.GetLanguage(culture.IetfLanguageTag);
                return fontFamily.FamilyNames.FirstOrDefault(o => o.Key == currentLang).Value ?? fontFamily.Source;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
