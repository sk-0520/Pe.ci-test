using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ContentTypeTextNet.Pe.Main.Views
{
    public class EllipsisTextBlock: Control
    {
        #region TextProperty

        public string? Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(EllipsisTextBlock),
            new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(TextPropertyChanged)
            )
        );

        private static void TextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is EllipsisTextBlock control) {
                control.Text = (string)e.NewValue;
                control.InvalidateVisual();
            }
        }

        #endregion

        #region Control

        protected override void OnRender(DrawingContext drawingContext)
        {
            var text = Text;

            if(string.IsNullOrEmpty(text)) {
                var emptyText = new FormattedText(
                    "M",
                    CultureInfo.CurrentUICulture,
                    FlowDirection,
                    new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                    FontSize,
                    Foreground,
                    VisualTreeHelper.GetDpi(this).PixelsPerDip
                );

                Height = emptyText.Height;
                drawingContext.DrawRectangle(Background, new Pen(), new Rect(0, 0, ActualWidth, Height));
                return;
            }

            var formattedText = new FormattedText(
                text,
                CultureInfo.CurrentUICulture,
                FlowDirection,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                FontSize,
                Foreground,
                VisualTreeHelper.GetDpi(this).PixelsPerDip
            );
            Height = formattedText.Height;

            drawingContext.DrawRectangle(Background, new Pen(), new Rect(0, 0, ActualWidth, Height));

            if(formattedText.Width <= ActualWidth) {
                drawingContext.DrawText(formattedText, new Point(0, 0));
                return;
            }

            Debug.WriteLine("何とかして縮める");

            var markChars = new[] { '\\', '/', };
            var ellipsis = "...";

            string markedText;
            int fronLength;

            var markIndex = text.LastIndexOfAny(markChars);
            if(markIndex != -1) {
                var markChar = text[markIndex];
                var lasIndex = text.LastIndexOf(markChar);

                if(lasIndex < ellipsis.Length) {
                    drawingContext.DrawText(formattedText, new Point(0, 0));
                    return;
                }

                markedText = text.Substring(lasIndex);
                fronLength = lasIndex - 1;
            } else {
                var lasIndex = text.Length / 2;

                if(lasIndex < 2) {
                    drawingContext.DrawText(formattedText, new Point(0, 0));
                    return;
                }

                markedText = text.Substring(lasIndex);
                fronLength = lasIndex - 1;
            }

            while(true) {
                var front = text.Substring(0, fronLength);

                var shortText = front + ellipsis + markedText;

                var shortFormattedText = new FormattedText(
                   shortText,
                   CultureInfo.CurrentUICulture,
                   FlowDirection,
                   new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                   FontSize,
                   Foreground,
                   VisualTreeHelper.GetDpi(this).PixelsPerDip
               );

                if(shortFormattedText.Width <= ActualWidth) {
                    drawingContext.DrawText(shortFormattedText, new Point(0, 0));
                    return;
                }

                fronLength -= 1;

                if(fronLength == 0) {
                    drawingContext.DrawText(shortFormattedText, new Point(0, 0));
                    return;
                }
            }
        }

        #endregion
    }
}
