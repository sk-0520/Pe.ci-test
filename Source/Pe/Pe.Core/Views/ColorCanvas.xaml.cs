using System;
using System.Collections.Generic;
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

        #region ValueProperty

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(Color),
            typeof(ColorCanvas),
            new PropertyMetadata(Colors.Red, OnValuePropertyChanged)
        );

        public Color Value
        {
            get { return (Color)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ColorCanvas)d;
            var color = (Color)e.NewValue;
            if(ctrl.HasAlpha) {
                ctrl.Value = color;
            } else {
                ctrl.Value = Color.FromRgb(color.R, color.G, color.B);
            }
        }

        #endregion

    }
}
