using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Core.Views
{
    /// <summary>
    /// NumericUpDown.xaml の相互作用ロジック
    /// </summary>
    public partial class NumericUpDown: UserControl
    {
        public NumericUpDown()
        {
            InitializeComponent();
        }

        #region property

        private Regex IntegerRegex { get; } = new Regex(@"[\+\-,0-9]", default, Timeout.InfiniteTimeSpan);
        private Regex DecimalRegex { get; } = new Regex(@"[\+\-,0-9\.]", default, Timeout.InfiniteTimeSpan);

        public int ScrollNotch { get; set; } = 120;
        //public int ScrollLines { get; set; } = SystemParameters.WheelScrollLines;

        #endregion

        #region ValueProperty

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(decimal),
            typeof(NumericUpDown),
            new FrameworkPropertyMetadata(
                0m,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnValuePropertyChanged
            )
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
            var rangeValue = Math.Max(ctrl.Minimum, Math.Min(value, ctrl.Maximum));
            ctrl.Value = Math.Round(rangeValue, ctrl.DecimalPlaces, MidpointRounding.ToEven);

            if(!ctrl.IsReadOnly) {
                ctrl.PART_UP_BUTTON.IsEnabled = ctrl.Value < ctrl.Maximum;
                ctrl.PART_DOWN_BUTTON.IsEnabled = ctrl.Minimum < ctrl.Value;
            }
        }

        #endregion

        #region MinimumProperty

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            nameof(Minimum),
            typeof(decimal),
            typeof(NumericUpDown),
            new PropertyMetadata(decimal.MinValue, OnMinimumPropertyChanged)
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
                throw new ArgumentOutOfRangeException($"{nameof(ctrl.Maximum)} < {nameof(minimum)}");
            }
            ctrl.Minimum = minimum;
            ctrl.PART_DOWN_BUTTON.IsEnabled = ctrl.Minimum < ctrl.Value;
        }

        #endregion

        #region MaximumProperty

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            nameof(Maximum),
            typeof(decimal),
            typeof(NumericUpDown),
            new PropertyMetadata(decimal.MaxValue, OnMaximumPropertyChanged)
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
                throw new ArgumentOutOfRangeException($"{nameof(maximum)} < {nameof(ctrl.Minimum)}");
            }
            ctrl.Maximum = maximum;
            ctrl.PART_UP_BUTTON.IsEnabled = ctrl.Value < ctrl.Maximum;
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

        #region TextAlignmentProperty

        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(
            nameof(TextAlignment),
            typeof(TextAlignment),
            typeof(NumericUpDown),
            new PropertyMetadata(TextAlignment.Right, OnTextAlignmentPropertyChanged)
        );

        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        private static void OnTextAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (NumericUpDown)d;
            ctrl.TextAlignment = (TextAlignment)e.NewValue;
        }

        #endregion

        #region DecimalPlacesProperty

        public static readonly DependencyProperty DecimalPlacesProperty = DependencyProperty.Register(
            nameof(DecimalPlaces),
            typeof(int),
            typeof(NumericUpDown),
            new PropertyMetadata(2, OnDecimalPlacesPropertyChanged)
        );

        public int DecimalPlaces
        {
            get { return (int)GetValue(DecimalPlacesProperty); }
            set { SetValue(DecimalPlacesProperty, value); }
        }

        private static void OnDecimalPlacesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (NumericUpDown)d;
            if(ctrl.DecimalPlaces < 0) {
                throw new InvalidOperationException(nameof(DecimalPlaces));
            }
            ctrl.DecimalPlaces = (int)e.NewValue;
        }

        #endregion

        #region IsReadOnlyProperty

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
            nameof(IsReadOnly),
            typeof(bool),
            typeof(NumericUpDown),
            new PropertyMetadata(false, OnIsReadOnlyPropertyChanged)
        );

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        private static void OnIsReadOnlyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (NumericUpDown)d;
            ctrl.IsReadOnly = (bool)e.NewValue;
        }

        #endregion

        #region function

        private void UpValue()
        {
            if(IsReadOnly) {
                return;
            }
            if(Value + Increment <= Maximum) {
                Value += Increment;
            } else {
                Value = Maximum;
            }

            UpdateCursor();
        }

        private void DownValue()
        {
            if(IsReadOnly) {
                return;
            }
            if(Minimum <= Value - Increment) {
                Value -= Increment;
            } else {
                Value = Minimum;
            }

            UpdateCursor();
        }

        private void UpdateCursor()
        {
            this.PART_NUMERIC.SelectionStart = this.PART_NUMERIC.Text.Length;
        }

        #endregion

        #region UserControl

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);

            var parentScrollViewer = UIUtility.GetVisualClosest<ScrollViewer>(this);
            if(parentScrollViewer == null) {
                return;
            }

            var scrollLineCount = Math.Abs(e.Delta) / ScrollNotch * SystemParameters.WheelScrollLines;
            if(e.Delta > 0) { // ↑
                if(Value == Maximum) {
                    // 一番上なので親側をスクロールさせる
                    foreach(var counter in new Counter(scrollLineCount)) {
                        parentScrollViewer.LineUp();
                    }
                } else {
                    UpValue();
                }
                e.Handled = true;
            } else if(e.Delta < 0) { // ↓
                if(Value == Minimum) {
                    // 一番下なので親側をスクロールさせる
                    foreach(var counter in new Counter(scrollLineCount)) {
                        parentScrollViewer.LineDown();
                    }
                } else {
                    DownValue();
                }
                e.Handled = true;
            }
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

        private void PART_NUMERIC_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(DecimalPlaces == 0) {
                var unsafeInput = !IntegerRegex.IsMatch(e.Text);
                if(unsafeInput) {
                    e.Handled = true;
                    return;
                }
                //TODO: 入力値との検証
            } else {
                var unsafeInput = !DecimalRegex.IsMatch(e.Text);
                if(unsafeInput) {
                    e.Handled = true;
                    return;
                }
                //TODO: 入力値との検証
            }
        }

        private void PART_NUMERIC_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Up || e.Key == Key.PageUp) {
                UpValue();
                e.Handled = true;
            } else if(e.Key == Key.Down || e.Key == Key.PageDown) {
                DownValue();
                e.Handled = true;
            }
        }

        private void PART_NUMERIC_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            //TODO: 値検証
            if(e.Command == ApplicationCommands.Paste) {
                string text = Clipboard.GetText();
                if(!decimal.TryParse(text, out _)) {
                    e.Handled = true;
                }
            }
        }
    }
}
