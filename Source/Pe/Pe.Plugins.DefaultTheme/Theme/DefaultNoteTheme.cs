using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;
using IO = System.IO;

namespace ContentTypeTextNet.Pe.Plugins.DefaultTheme.Theme
{
    internal class DefaultNoteTheme: DefaultThemeBase, INoteTheme
    {
        public DefaultNoteTheme(IThemeParameter parameter)
            : base(parameter)
        { }

        #region property
        #endregion

        #region function

        private string GetResourceBaseKey(NoteCaptionButtonKind noteCaption, bool isEnabled)
        {
            switch(noteCaption) {
                case NoteCaptionButtonKind.Compact:
                    if(isEnabled) {
                        return "Path-NoteCaption-Compact-Enabled";
                    } else {
                        return "Path-NoteCaption-Compact-Disabled";
                    }

                case NoteCaptionButtonKind.Topmost:
                    if(isEnabled) {
                        return "Path-NoteCaption-Topmost-Enabled";
                    } else {
                        return "Path-NoteCaption-Topmost-Disabled";
                    }

                case NoteCaptionButtonKind.Close:
                    return "Path-NoteCaption-Close";

                default:
                    throw new NotSupportedException();
            }
        }

        private DependencyObject GetCaptionImageCore(NoteCaptionButtonKind noteCaption, NoteCaptionPosition captionPosition, bool isEnabled, ColorPair<Color> baseColor)
        {
            var viewBox = new Viewbox();
            using(Initializer.Begin(viewBox)) {
                viewBox.Width = GetCaptionHeight() * 0.8;
                viewBox.Height = viewBox.Width;

                var canvas = new Canvas();
                using(Initializer.Begin(canvas)) {
                    canvas.Width = 24;
                    canvas.Height = 24;

                    var path = new Path();
                    using(Initializer.Begin(path)) {
                        var resourceBaseKey = GetResourceBaseKey(noteCaption, isEnabled);
                        var geometry = GetResourceValue<Geometry>(nameof(DefaultNoteTheme), resourceBaseKey);
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

        public ColorPair<Brush> GetCaptionBrush(NoteCaptionPosition captionPosition, ColorPair<Color> baseColor)
        {
            var pair = new ColorPair<Brush>(new SolidColorBrush(baseColor.Foreground), new SolidColorBrush(baseColor.Background));

            FreezableUtility.SafeFreeze(pair.Foreground);
            FreezableUtility.SafeFreeze(pair.Background);

            return pair;
        }

        public Brush GetBorderBrush(NoteCaptionPosition captionPosition, ColorPair<Color> baseColor)
        {
            return FreezableUtility.GetSafeFreeze(new SolidColorBrush(baseColor.Background));
        }

        public ColorPair<Brush> GetContentBrush(NoteCaptionPosition captionPosition, ColorPair<Color> baseColor)
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

        public Brush GetCaptionButtonBackgroundBrush(NoteCaptionButtonState buttonState, NoteCaptionPosition captionPosition, ColorPair<Color> baseColor)
        {
            return Brushes.Transparent;
        }

        public DependencyObject GetCaptionImage(NoteCaptionButtonKind buttonKind, NoteCaptionPosition captionPosition, bool isEnabled, ColorPair<Color> baseColor)
        {
            return GetCaptionImageCore(buttonKind, captionPosition, isEnabled, baseColor);
        }

        public DependencyObject GetResizeGripImage(NoteCaptionPosition captionPosition, ColorPair<Color> baseColor)
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
                        var resourceBaseKey = "Path-Note-ResizeGrip";
                        var geometry = GetResourceValue<Geometry>(nameof(DefaultNoteTheme), resourceBaseKey);
                        path.Data = geometry;
                        path.Fill = FreezableUtility.GetSafeFreeze(new SolidColorBrush(MediaUtility.GetAutoColor(baseColor.Foreground)));
                        path.Stroke = FreezableUtility.GetSafeFreeze(new SolidColorBrush(baseColor.Foreground));
                        path.StrokeThickness = 1;
                    }
                    canvas.Children.Add(path);
                }

                if(captionPosition == NoteCaptionPosition.Bottom) {
                    canvas.RenderTransformOrigin = new Point(0.5, 0.5);
                    canvas.RenderTransform = new ScaleTransform() {
                        ScaleY = -1,
                    };
                } else if(captionPosition == NoteCaptionPosition.Right) {
                    canvas.RenderTransformOrigin = new Point(0.5, 0.5);
                    canvas.RenderTransform = new ScaleTransform() {
                        ScaleX = -1,
                    };
                }

                viewBox.Child = canvas;
            }

            return viewBox;
        }

        /// <inheritdoc cref="INoteTheme.GetBlindEffect(ColorPair{Color})"/>
        public Effect GetBlindEffect(ColorPair<Color> baseColor)
        {
            var content = GetResourceValue<Effect>(nameof(DefaultNoteTheme), nameof(GetBlindEffect));
            return content;
        }


        /// <inheritdoc cref="INoteTheme.GetBlindContent(ColorPair{Color})"/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded")]
        public DependencyObject GetBlindContent(ColorPair<Color> baseColor)
        {
            var srcContent = GetResourceValue<DependencyObject>(nameof(DefaultNoteTheme), nameof(GetBlindContent));

            using var srcContentStream = new IO.MemoryStream();
            using(var keepStream = new KeepStream(srcContentStream)) {
                XamlWriter.Save(srcContent, keepStream);
            }

            using var srcContentReader = new XmlTextReader(srcContentStream);

            var xmlNamespaceManager = new XmlNamespaceManager(srcContentReader.NameTable);
            var xamlNamespace = "theme";
            xmlNamespaceManager.AddNamespace(xamlNamespace, "http://schemas.microsoft.com/winfx/2006/xaml/presentation");

            var newContentXml = XDocument.Load(srcContentReader);
            var penElement = newContentXml.XPathSelectElement("//*/" + xamlNamespace + ":Pen", xmlNamespaceManager);
            penElement!.SetAttributeValue(Pen.BrushProperty.Name, baseColor.Foreground.ToString(CultureInfo.InvariantCulture));

            using var newContentStream = new IO.MemoryStream();
            using(var keepStream = new KeepStream(newContentStream)) {
                newContentXml.Save(keepStream);
            }

            using var newContentReader = XmlReader.Create(newContentStream);
            var newContent = (Border)XamlReader.Load(newContentReader);

            return newContent;
        }


        #endregion
    }
}
