namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Attached
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Interactivity;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
    using ICSharpCode.AvalonEdit;

    /// <summary>
    /// http://stackoverflow.com/questions/18964176/two-way-binding-to-avalonedit-document-text-using-mvvm?answertab=votes#tab-top
    /// </summary>
    public sealed class AvalonEditBehaviour: Behavior<TextEditor>
    {
        public static readonly DependencyProperty CodeProperty =
            DependencyProperty.Register(DependencyPropertyUtility.GetName(nameof(CodeProperty)), typeof(string), typeof(AvalonEditBehaviour),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PropertyChangedCallback));

        public string Code
        {
            get { return (string)GetValue(CodeProperty); }
            set { SetValue(CodeProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if(AssociatedObject != null)
                AssociatedObject.TextChanged += AssociatedObjectOnTextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if(AssociatedObject != null)
                AssociatedObject.TextChanged -= AssociatedObjectOnTextChanged;
        }

        private void AssociatedObjectOnTextChanged(object sender, EventArgs eventArgs)
        {
            var textEditor = sender as TextEditor;
            if(textEditor != null) {
                if(textEditor.Document != null)
                    Code = textEditor.Document.Text;
            }
        }

        private static void PropertyChangedCallback(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var behavior = dependencyObject as AvalonEditBehaviour;
            if(behavior.AssociatedObject != null) {
                var editor = behavior.AssociatedObject as TextEditor;
                if(editor.Document != null) {
                    var caretOffset = editor.CaretOffset;
                    editor.Document.Text = dependencyPropertyChangedEventArgs.NewValue as string ?? string.Empty;
                    if(caretOffset < editor.CaretOffset) {
                        editor.CaretOffset = caretOffset;
                    }
                }
            }
        }
    }
}
