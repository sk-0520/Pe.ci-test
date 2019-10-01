using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// NumericUpDown.xaml の相互作用ロジック
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public NumericUpDown()
        {
            InitializeComponent();
        }

        #region ValueProperty

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(decimal),
            typeof(NumericUpDown),
            new PropertyMetadata(0m, OnValuePropertyChanged)
        );

        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (NumericUpDown)d;
            var value = (decimal)e.NewValue;

            // 入力値は範囲内に収まるようにする
            ctrl.Value = Math.Max(ctrl.Minimum, Math.Min(value, ctrl.Maximum));

            ctrl.PART_UP_BUTTON.IsEnabled = ctrl.Maximum != ctrl.Value;
            ctrl.PART_DOWN_BUTTON.IsEnabled = ctrl.Minimum != ctrl.Value;
        }

        #endregion

        #region MinimumProperty

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            nameof(Minimum),
            typeof(decimal),
            typeof(NumericUpDown),
            new PropertyMetadata(-100m, OnMinimumPropertyChanged)
        );

        public decimal Minimum
        {
            get { return (decimal)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        private static void OnMinimumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (NumericUpDown)d;
            var minimum = (decimal)e.NewValue;
            if(ctrl.Maximum < minimum) {
                throw new ArgumentOutOfRangeException($"{nameof(ctrl.Maximum)} < {nameof(minimum)}"); ;
            }
            ctrl.Minimum = minimum;
        }

        #endregion

        #region MaximumProperty

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            nameof(Maximum),
            typeof(decimal),
            typeof(NumericUpDown),
            new PropertyMetadata(100m, OnMaximumPropertyChanged)
        );

        public decimal Maximum
        {
            get { return (decimal)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        private static void OnMaximumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (NumericUpDown)d;
            var maximum = (decimal)e.NewValue;
            if(maximum < ctrl.Minimum) {
                throw new ArgumentOutOfRangeException($"{nameof(maximum)} < {nameof(ctrl.Minimum)}"); ;
            }
            ctrl.Maximum = maximum;
        }

        #endregion

        #region IncrementProperty

        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(
            nameof(Increment),
            typeof(decimal),
            typeof(NumericUpDown),
            new PropertyMetadata(1m, OnIncrementPropertyChanged)
        );

        public decimal Increment
        {
            get { return (decimal)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }

        private static void OnIncrementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (NumericUpDown)d;
            ctrl.Increment = (decimal)e.NewValue;
        }

        #endregion

        #region function

        void UpValue()
        {
            Value += Increment;
        }

        void DownValue()
        {
            Value -= Increment;
        }

        #endregion

        private void PART_UP_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            UpValue();
        }

        private void PART_DOWN_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            DownValue();
        }

        private void PART_NUMERIC_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(e.Delta < 0) {
                DownValue();
            } else {
                UpValue();
            }
        }
    }
}
