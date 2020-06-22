using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Views.Converter
{
    [ValueConversion(typeof(string), typeof(ImageSource))]
    public class IconResourceToImageConveter: IValueConverter
    {
        #region proeprty

        public int Size { get; set; } = (int)IconBox.Normal;

        #endregion

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var resourceUri = new Uri((string)value);
            var decoder = BitmapDecoder.Create(
                resourceUri,
                BitmapCreateOptions.DelayCreation,
                BitmapCacheOption.OnDemand
            );

            var result = decoder.Frames.FirstOrDefault(f => f.Width == Size);
            if(result == default(BitmapFrame)) {
                // ダメっぽいときは一番近くっぽいのをとるべき。
                result = decoder.Frames.OrderBy(f => f.Width).First();
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
