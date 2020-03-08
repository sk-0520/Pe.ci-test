using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Views.Converter
{
    [ValueConversion(typeof(FontFamily), typeof(string))]
    public class FontFamilyToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return CastUtility.AsFunc<FontFamily, string>(value, fontFamily => {
                var currentLang = XmlLanguage.GetLanguage(culture.IetfLanguageTag);
                return fontFamily.FamilyNames.FirstOrDefault(o => o.Key == currentLang).Value ?? fontFamily?.Source ?? string.Empty;
            });
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
