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
using ContentTypeTextNet.Pe.Main.Models;
using ICSharpCode.AvalonEdit.Document;

namespace ContentTypeTextNet.Pe.Main.Views.LauncherItemCustomize
{
    /// <summary>
    /// EnvironmentValieableEditor.xaml の相互作用ロジック
    /// </summary>
    public partial class EnvironmentValieableEditor : UserControl
    {
        public EnvironmentValieableEditor()
        {
            InitializeComponent();
        }

        #region MergeTextDocumentProperty

        public static readonly DependencyProperty MergeTextDocumentProperty = DependencyProperty.Register(
            nameof(MergeTextDocument),
            typeof(TextDocument),
            typeof(EnvironmentValieableEditor),
            new PropertyMetadata(
                default(TextDocument),
                OnMergeTextDocumentChanged
            )
        );

        public TextDocument MergeTextDocument
        {
            get { return (TextDocument)GetValue(MergeTextDocumentProperty); }
            set { SetValue(MergeTextDocumentProperty, value); }
        }

        private static void OnMergeTextDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is EnvironmentValieableEditor control) {
                control.MergeTextDocument = (TextDocument)e.NewValue;
            }
        }

        #endregion

        #region RemoveTextDocumentProperty

        public static readonly DependencyProperty RemoveTextDocumentProperty = DependencyProperty.Register(
            nameof(RemoveTextDocument),
            typeof(TextDocument),
            typeof(EnvironmentValieableEditor),
            new PropertyMetadata(
                default(TextDocument),
                OnRemoveTextDocumentChanged
            )
        );

        public TextDocument RemoveTextDocument
        {
            get { return (TextDocument)GetValue(RemoveTextDocumentProperty); }
            set { SetValue(RemoveTextDocumentProperty, value); }
        }

        private static void OnRemoveTextDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is EnvironmentValieableEditor control) {
                control.RemoveTextDocument = (TextDocument)e.NewValue;
            }
        }

        #endregion

        private void EnvMergeEditor_Loaded(object sender, RoutedEventArgs e)
        {
            using(var stream = ResourceUtility.OpenSyntaxStream(Properties.Resources.File_Highlighting_EnvironmentVariable_Merge)) {
                AvalonEditHelper.SetSyntaxHighlightingDefault((ICSharpCode.AvalonEdit.TextEditor)sender, stream);
            }
        }

        private void EnvRemoveEditor_Loaded(object sender, RoutedEventArgs e)
        {
            using(var stream = ResourceUtility.OpenSyntaxStream(Properties.Resources.File_Highlighting_EnvironmentVariable_Remove)) {
                AvalonEditHelper.SetSyntaxHighlightingDefault((ICSharpCode.AvalonEdit.TextEditor)sender, stream);
            }
        }

    }
}
