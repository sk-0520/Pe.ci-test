using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Theme
{
    internal class NoteTheme : ThemeBase, INoteTheme
    {
        public NoteTheme(IPlatformThemeLoader platformThemeLoader, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(platformThemeLoader, dispatcherWrapper, loggerFactory)
        { }

        #region property
        #endregion

        #region function


        string GetResourceKey(NoteCaption noteCaption, bool isEnabled)
        {
            switch(noteCaption) {
                case NoteCaption.Compact:
                    if(isEnabled) {
                        return "Image-NoteCaption-Compact-Enabled";
                    } else {
                        return "Image-NoteCaption-Compact-Disabled";
                    }

                case NoteCaption.Topmost:
                    if(isEnabled) {
                        return "Image-NoteCaption-Topmost-Enabled";
                    } else {
                        return "Image-NoteCaption-Topmost-Disabled";
                    }

                case NoteCaption.Close:
                    return "Image-NoteCaption-Close";

                default:
                    throw new NotSupportedException();
            }
        }

        DependencyObject GetCaptionImageCore(NoteCaption noteCaption, bool isEnabled, IReadOnlyColorPair<Color> baseColor)
        {
            var viewBox = new Viewbox();
            using(Initializer.Begin(viewBox)) {
                viewBox.Width = GetCaptionHeight();
                viewBox.Height = viewBox.Width;

                var canvas = new Canvas();
                using(Initializer.Begin(canvas)) {
                    canvas.Width = 24;
                    canvas.Height = 24;

                    var path = new Path();
                    using(Initializer.Begin(path)) {
                        var resourceKey = GetResourceKey(noteCaption, isEnabled);
                        var geometry = (Geometry)Application.Current.Resources[resourceKey];
                        FreezableUtility.SafeFreeze(geometry);
                        path.Data = geometry;
                        path.Fill = FreezableUtility.GetSafeFreeze(new SolidColorBrush(baseColor.Foreground));
                        //path.Stroke = FreezableUtility.GetSafeFreeze(new SolidColorBrush(MediaUtility.GetAutoColor(baseColor.Foreground)));
                        //path.StrokeThickness = 1;
                    }
                    canvas.Children.Add(path);
                }
                viewBox.Child = canvas;
            }

            return viewBox;
        }


        #endregion

        #region INoteTheme

        [return: PixelKind(Px.Logical)]
        public double GetCaptionHeight()
        {
            return SystemParameters.CaptionHeight;
        }

        [return: PixelKind(Px.Logical)]
        public double GetCaptionFontSize(double baseFontSize)
        {
            return SystemFonts.CaptionFontSize;
        }

        public FontFamily GetCaptionFontFamily(FontFamily baseFontFamily)
        {
            return SystemFonts.CaptionFontFamily;
        }

        [return: PixelKind(Px.Logical)]
        public Thickness GetBorderThickness()
        {
            return SystemParameters.WindowResizeBorderThickness;
        }
        [return: PixelKind(Px.Logical)]
        public Size GetResizeGripSize()
        {
            var thickness = GetBorderThickness();

            return new Size(
                thickness.Right * 6,
                thickness.Bottom * 6
            );
        }

        public IReadOnlyColorPair<Brush> GetCaptionBrush(IReadOnlyColorPair<Color> baseColor)
        {
            var pair = ColorPair.Create(new SolidColorBrush(baseColor.Foreground), new SolidColorBrush(baseColor.Background));

            FreezableUtility.SafeFreeze(pair.Foreground);
            FreezableUtility.SafeFreeze(pair.Background);

            return pair;
        }

        public Brush GetBorderBrush(IReadOnlyColorPair<Color> baseColor)
        {
            return FreezableUtility.GetSafeFreeze(new SolidColorBrush(baseColor.Background));
        }

        public IReadOnlyColorPair<Brush> GetContentBrush(IReadOnlyColorPair<Color> baseColor)
        {
            /*
            旧PeではXAML上でこれをかけ合わせてた
            <LinearGradientBrush StartPoint="0,0" EndPoint="0,1">
                <GradientStop Color="#A0ffffff" Offset="0" />
                <GradientStop Color="Transparent" Offset="0.4"/>
                <GradientStop Color="Transparent" Offset="0.8"/>
                <GradientStop Color="#20101010" Offset="1"/>
            </LinearGradientBrush>
            */
            var collection = new GradientStopCollection(new[] {
                new GradientStop(MediaUtility.AddColor(baseColor.Background, Color.FromArgb(0xa0, 0xff, 0xff, 0xff)), 0),
                new GradientStop(baseColor.Background, 0.4),
                new GradientStop(baseColor.Background, 0.8),
                new GradientStop(MediaUtility.AddColor(baseColor.Background, Color.FromArgb(0x20, 0x10, 0x10, 0x10)), 1),
            });
            var gradation = new LinearGradientBrush(collection, new Point(0, 0), new Point(0, 1));
            return ColorPair.Create<Brush>(
                FreezableUtility.GetSafeFreeze(new SolidColorBrush(baseColor.Foreground)),
                FreezableUtility.GetSafeFreeze(gradation)
            );
        }

        public Brush GetCaptionButtonBackgroundBrush(NoteCaptionButtonState buttonState, IReadOnlyColorPair<Color> baseColor)
        {
            // TODO: 色調整
            switch(buttonState) {
                case NoteCaptionButtonState.None:
                    return FreezableUtility.GetSafeFreeze(Brushes.Transparent);

                case NoteCaptionButtonState.Over:
                    return FreezableUtility.GetSafeFreeze(Brushes.Lime);

                case NoteCaptionButtonState.Pressed:
                    return FreezableUtility.GetSafeFreeze(Brushes.Red);

                default:
                    throw new NotImplementedException();
            }
        }

        public DependencyObject GetCaptionImage(NoteCaption noteCaption, bool isEnabled, IReadOnlyColorPair<Color> baseColor)
        {
            return DispatcherWrapper.Get(() => GetCaptionImageCore(noteCaption, isEnabled, baseColor));
        }

        public DependencyObject GetResizeGripImage(IReadOnlyColorPair<Color> baseColor)
        {
            var viewBox = new Viewbox();
            using(Initializer.Begin(viewBox)) {
                viewBox.Width = GetResizeGripSize().Width;
                viewBox.Height = GetResizeGripSize().Height;

                var canvas = new Canvas();
                using(Initializer.Begin(canvas)) {
                    canvas.Width = 24;
                    canvas.Height = 24;

                    var path = new Path();
                    using(Initializer.Begin(path)) {
                        var resourceKey = "Image-Note-ResizeGrip";
                        var geometry = (Geometry)Application.Current.Resources[resourceKey];
                        FreezableUtility.SafeFreeze(geometry);
                        path.Data = geometry;
                        path.Fill = FreezableUtility.GetSafeFreeze(new SolidColorBrush(MediaUtility.GetAutoColor(baseColor.Foreground)));
                        path.Stroke = FreezableUtility.GetSafeFreeze(new SolidColorBrush(baseColor.Foreground));
                        path.StrokeThickness = 1;
                    }
                    canvas.Children.Add(path);
                }
                viewBox.Child = canvas;
            }

            return viewBox;
        }

        public DependencyObject GetIconImage(IconBox iconBox, bool isCompact, bool isLocked, IReadOnlyColorPair<Color> baseColor)
        {
            var size = new Size((int)iconBox, isCompact ? (int)iconBox / 2 : (int)iconBox);
            var box = CreateBox(baseColor.Foreground, baseColor.Background, size);
            return box;
        }

        #endregion
    }
}
