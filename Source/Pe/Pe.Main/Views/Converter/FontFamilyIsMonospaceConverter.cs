using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Views.Converter
{
    [ValueConversion(typeof(FontFamily), typeof(bool))]
    public class FontFamilyIsMonospaceConverter: IValueConverter
    {
        #region property

        private IDictionary<FontFamily, bool> Cache { get; } = new Dictionary<FontFamily, bool>();

        #endregion

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return CastUtility.AsFunc<FontFamily, bool>(value, fontFamily => {
                if(Cache.TryGetValue(fontFamily, out var result)) {
                    return result;
                }

                var typeface = fontFamily.GetTypefaces().FirstOrDefault();
                if(typeface == null) {
                    return Cache[fontFamily] = false;
                }

                var cs = new[] { "i", "W", "m" };
                var widths = new List<int>(cs.Length);
                foreach(var c in cs) {
                    var formattedText = new FormattedText(
                        c,
                        culture,
                        System.Windows.FlowDirection.LeftToRight,
                        typeface,
                        10,
                        Brushes.Transparent,
                        1
                    );
                    widths.Add((int)formattedText.Width);
                }

                return Cache[fontFamily] = widths.Distinct().Count() == 1;
            });
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
