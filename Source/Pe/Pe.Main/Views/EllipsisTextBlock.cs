using System.Diagnostics;
using System.Globalization;
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

        #region function

        FormattedText CreateFormattedText(string s)
        {
            var result = new FormattedText(
                s,
                CultureInfo.CurrentUICulture,
                FlowDirection,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                FontSize,
                Foreground,
                VisualTreeHelper.GetDpi(this).PixelsPerDip
            );

            return result;
        }

        #endregion

        #region Control

        protected override void OnRender(DrawingContext drawingContext)
        {
            var text = Text;

            var isEmpty = string.IsNullOrEmpty(text);
            var formattedText = isEmpty
                ? CreateFormattedText("M")
                : CreateFormattedText(text!)
            ;

            Height = formattedText.Height;
            drawingContext.DrawRectangle(Background, new Pen(), new Rect(0, 0, ActualWidth, Height));

            if(isEmpty) {
                return;
            }
            Debug.Assert(text != null);

            if(formattedText.Width <= ActualWidth) {
                drawingContext.DrawText(formattedText, new Point(0, 0));
                return;
            }

            Debug.WriteLine("何とかして縮める");

            var markChars = new[] { '\\', '/', };
            var ellipsis = "...";

            string markedText;
            int frontLength;

            var markIndex = text.LastIndexOfAny(markChars);
            if(markIndex != -1) {
                var markChar = text[markIndex];
                var lastIndex = text.LastIndexOf(markChar);

                if(lastIndex < ellipsis.Length) {
                    drawingContext.DrawText(formattedText, new Point(0, 0));
                    return;
                }

                markedText = text.Substring(lastIndex);
                frontLength = lastIndex - 1;
            } else {
                var lastIndex = text.Length / 2;

                if(lastIndex < 2) {
                    drawingContext.DrawText(formattedText, new Point(0, 0));
                    return;
                }

                markedText = text.Substring(lastIndex);
                frontLength = lastIndex - 1;
            }

            while(true) {
                var front = text.Substring(0, frontLength);

                var shortText = front + ellipsis + markedText;

                var shortFormattedText = CreateFormattedText(shortText);

                if(shortFormattedText.Width <= ActualWidth) {
                    drawingContext.DrawText(shortFormattedText, new Point(0, 0));
                    return;
                }

                frontLength -= 1;

                if(frontLength == 0) {
                    drawingContext.DrawText(shortFormattedText, new Point(0, 0));
                    return;
                }
            }
        }

        #endregion
    }
}
