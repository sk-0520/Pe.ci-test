using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace ContentTypeTextNet.Pe.Core.Views.Attached
{
    public class InputControlBehavior : Behavior<TextBox>
    {
        #region AccepPatternProperty

        public static readonly DependencyProperty AccepPatternProperty = DependencyProperty.RegisterAttached(
            nameof(AccepPattern),
            typeof(string),
            typeof(InputControlBehavior),
            new FrameworkPropertyMetadata(
                default(string),
                new PropertyChangedCallback(OnAccepPatternChanged)
            )
        );

        public string AccepPattern
        {
            get { return (string)GetValue(AccepPatternProperty); }
            set { SetValue(AccepPatternProperty, value); }
        }

        private static void OnAccepPatternChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as InputControlBehavior;
            if(control != null) {
                control.AccepPattern = (string)e.NewValue;
                control.AccepRegex = new Regex("^(" + control.AccepPattern + ")$");
            }
        }

        Regex? AccepRegex { get; set; }

        #endregion

        #region function

        private bool CanInput(string value)
        {
            if(AccepPattern == null) {
                return true;
            }

            if(AccepRegex == null) {
                return true;
            }

            var isHit = AccepRegex.IsMatch(value);
            return isHit;
        }

        #endregion

        #region Behavior

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
            AssociatedObject.PreviewTextInput += AssociatedObject_PreviewTextInput;
            DataObject.AddPastingHandler(AssociatedObject, OnPaste);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
            AssociatedObject.PreviewTextInput -= AssociatedObject_PreviewTextInput;
            DataObject.RemovePastingHandler(AssociatedObject, OnPaste);

            base.OnDetaching();
        }

        #endregion

        private void AssociatedObject_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }

        private void AssociatedObject_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;
            if(textBox == null) {
                return;
            }

            if(!CanInput(textBox.Text + e.Text)) {
                e.Handled = true;
            }
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if(!e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true)) {
                return;
            }
            var data = (string)e.SourceDataObject.GetData(DataFormats.UnicodeText, true);

            if(!CanInput(data)) {
                e.Handled = true;
            }

        }

    }
}
