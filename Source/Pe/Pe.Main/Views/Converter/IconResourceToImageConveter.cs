using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Views.Converter
{
    [ValueConversion(typeof(string), typeof(ImageSource))]
    public class IconResourceToImageConveter: IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(parameter == null) {
                Debug.WriteLine("[ERROR] パラメータ未設定");
                if(Debugger.IsAttached) {
                    Debugger.Break();
                }
                return DependencyProperty.UnsetValue;
            }
            var size = System.Convert.ToInt32(parameter);
            var resourceUri = new Uri((string)value);
            var decoder = BitmapDecoder.Create(
                resourceUri,
                BitmapCreateOptions.DelayCreation,
                BitmapCacheOption.OnDemand
            );

            var result = decoder.Frames.FirstOrDefault(f => f.Width == size);
            if(result != null) {
                // ダメっぽいときは一番近くっぽいのをとる。
                int diff = decoder.Frames.Min(i => Math.Abs(i.PixelWidth - size));
                result = decoder.Frames.First(i => Math.Abs(i.PixelWidth - size) == diff);
            }

            return result!;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
