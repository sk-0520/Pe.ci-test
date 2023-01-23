using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models;
using Microsoft.Extensions.Logging.Abstractions;

namespace ContentTypeTextNet.Pe.Main.Views.Converter
{
    [ValueConversion(typeof(string), typeof(ImageSource))]
    public class IconResourceToImageConveter: IValueConverter, IMultiValueConverter
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

            //var hWnd = NativeMethods.GetDesktopWindow();
            var imageLoader = new ImageLoader(NullLoggerFactory.Instance);

            //var hDC = NativeMethods.GetWindowDC(IntPtr.Zero);
            //var dpiX = NativeMethods.GetDeviceCaps(hDC, DeviceCap.LOGPIXELSX);
            ////NativeMethods.ReleaseDC(hWnd, hDC);
            //var scaleX = dpiX / 96.0;
            //var size = (int)(System.Convert.ToInt32(parameter) * scaleX);
            var resourceUri = new Uri((string)value);
            var decoder = BitmapDecoder.Create(
                resourceUri,
                BitmapCreateOptions.DelayCreation,
                BitmapCacheOption.OnDemand
            );
            // メッチャクチャやなぁ
            var iconScale = new IconScale((IconBox)System.Convert.ToInt32(parameter, CultureInfo.InvariantCulture), imageLoader.GetPrimaryDpiScale());
            var result = imageLoader.GetImageFromFrames(decoder.Frames, iconScale);

            //var result = decoder.Frames.FirstOrDefault(f => f.Width == size);
            //if(result == null) {
            //    // ダメっぽいときは一番近くっぽいのをとる。
            //    int diff = decoder.Frames.Min(i => Math.Abs(i.PixelWidth - size));
            //    result = decoder.Frames.First(i => Math.Abs(i.PixelWidth - size) == diff);
            //}

            return result!;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IMultiValueConverter

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values == null || values.Length != 2) {
                return DependencyProperty.UnsetValue;
            }

            if(parameter == null) {
                Debug.WriteLine("[ERROR] パラメータ未設定");
                if(Debugger.IsAttached) {
                    Debugger.Break();
                }
                return DependencyProperty.UnsetValue;
            }

            var imageLoader = new ImageLoader(NullLoggerFactory.Instance);

            var resourceUri = new Uri((string)values[0]);
            var decoder = BitmapDecoder.Create(
                resourceUri,
                BitmapCreateOptions.DelayCreation,
                BitmapCacheOption.OnDemand
            );
            var visual = (Visual)values[1];

            var iconScale = new IconScale((IconBox)System.Convert.ToInt32(parameter, CultureInfo.InvariantCulture), UIUtility.GetDpiScale(visual));
            var result = imageLoader.GetImageFromFrames(decoder.Frames, iconScale);

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
