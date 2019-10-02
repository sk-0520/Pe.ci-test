using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ContentTypeTextNet.Pe.Core.Views
{
    /// <summary>
    /// ColorCanvas.xaml の相互作用ロジック
    /// </summary>
    public partial class ColorCanvas : UserControl
    {
        public ColorCanvas()
        {
            InitializeComponent();
        }

        #region property

        #endregion

        #region BaseColorProperty

        public static readonly DependencyProperty BaseColorProperty = DependencyProperty.Register(
            nameof(BaseColor),
            typeof(Color),
            typeof(ColorCanvas),
            new PropertyMetadata(Colors.Red, OnBaseColorPropertyChanged)
        );

        public Color BaseColor
        {
            get { return (Color)GetValue(BaseColorProperty); }
            set { SetValue(BaseColorProperty, value); }
        }

        private static void OnBaseColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ColorCanvas)d;
            ctrl.BaseColor = (Color)e.NewValue;
        }

        #endregion

        #region HasAlphaProperty

        public static readonly DependencyProperty HasAlphaProperty = DependencyProperty.Register(
            nameof(HasAlpha),
            typeof(bool),
            typeof(ColorCanvas),
            new PropertyMetadata(false, OnHasAlphaPropertyChanged)
        );

        public bool HasAlpha
        {
            get { return (bool)GetValue(HasAlphaProperty); }
            set { SetValue(HasAlphaProperty, value); }
        }

        private static void OnHasAlphaPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ColorCanvas)d;
            ctrl.HasAlpha = (bool)e.NewValue;
        }

        #endregion

        #region SelectedColorProperty

        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
            nameof(SelectedColor),
            typeof(Color),
            typeof(ColorCanvas),
            new PropertyMetadata(Colors.Red, OnSelectedColorPropertyChanged)
        );

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        private static void OnSelectedColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ColorCanvas)d;
            var color = (Color)e.NewValue;
            if(ctrl.HasAlpha) {
                ctrl.SelectedColor = color;
            } else {
                ctrl.SelectedColor = Color.FromRgb(color.R, color.G, color.B);
            }
        }

        #endregion

        #region HueGradationsProperty

        public static readonly DependencyProperty HueGradationsProperty = DependencyProperty.Register(
            nameof(HueGradations),
            typeof(GradientStopCollection),
            typeof(ColorCanvas),
            new PropertyMetadata(
                new GradientStopCollection(
                    new[] {
                        new GradientStop(Color.FromRgb(0xff,0x00,0x00), 0),
                        new GradientStop(Color.FromRgb(0xff,0xff,0x00), 0.167),
                        new GradientStop(Color.FromRgb(0x00,0xff,0x00), 0.333),
                        new GradientStop(Color.FromRgb(0x00,0xff,0xff), 0.5),
                        new GradientStop(Color.FromRgb(0x00,0x00,0xff), 0.667),
                        new GradientStop(Color.FromRgb(0xff,0x00,0xff), 0.833),
                        new GradientStop(Color.FromRgb(0xff,0x00,0x00), 1),
                    }
                ),
                OnHueGradationsPropertyChanged
            )
        );

        public GradientStopCollection HueGradations
        {
            get { return (GradientStopCollection)GetValue(HueGradationsProperty); }
            set { SetValue(HueGradationsProperty, value); }
        }

        private static void OnHueGradationsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ColorCanvas)d;
            ctrl.HueGradations = (GradientStopCollection)e.NewValue;
        }

        #endregion

        #region function

        /// <summary>
        /// https://stackoverflow.com/a/9651053
        /// </summary>
        /// <param name="gsc"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Color GetRelativeColor(GradientStopCollection gsc, double offset)
        {
            var point = gsc.SingleOrDefault(f => f.Offset == offset);
            if(point != null) {
                return point.Color;
            }

            GradientStop before = gsc.Where(w => w.Offset == gsc.Min(m => m.Offset)).First();
            GradientStop after = gsc.Where(w => w.Offset == gsc.Max(m => m.Offset)).First();

            foreach(var gs in gsc) {
                if(gs.Offset < offset && gs.Offset > before.Offset) {
                    before = gs;
                }
                if(gs.Offset > offset && gs.Offset < after.Offset) {
                    after = gs;
                }
            }

            var color = new Color();

            color.ScA = (float)((offset - before.Offset) * (after.Color.ScA - before.Color.ScA) / (after.Offset - before.Offset) + before.Color.ScA);
            color.ScR = (float)((offset - before.Offset) * (after.Color.ScR - before.Color.ScR) / (after.Offset - before.Offset) + before.Color.ScR);
            color.ScG = (float)((offset - before.Offset) * (after.Color.ScG - before.Color.ScG) / (after.Offset - before.Offset) + before.Color.ScG);
            color.ScB = (float)((offset - before.Offset) * (after.Color.ScB - before.Color.ScB) / (after.Offset - before.Offset) + before.Color.ScB);

            return color;
        }

        #endregion

        private void PART_HueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var color = GetRelativeColor(HueGradations, e.NewValue);
            BaseColor = color;
        }
    }
}
