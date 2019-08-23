using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Main.ViewModel.CustomizeLauncherItem;

namespace ContentTypeTextNet.Pe.Main.View.CustomizeLauncherItem
{
    /// <summary>
    /// CustomizeLauncherItemControl.xaml の相互作用ロジック
    /// </summary>
    public partial class CustomizeLauncherItemControl : UserControl
    {
        public CustomizeLauncherItemControl()
        {
            InitializeComponent();
        }

        #region Item

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Item),
            typeof(CustomizeLauncherItemViewModel),
            typeof(CustomizeLauncherItemControl),
            new FrameworkPropertyMetadata(
                default(CustomizeLauncherItemViewModel),
                new PropertyChangedCallback(OnItemChanged)
            )
        );

        public CustomizeLauncherItemViewModel Item
        {
            get { return (CustomizeLauncherItemViewModel)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        private static void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is CustomizeLauncherItemControl control) {
            }
        }

        #endregion

        #region function

        void SetSyntaxHighlighting(ICSharpCode.AvalonEdit.TextEditor editor, System.IO.Stream stream)
        {
            var instance = global::ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance;
            using(var reader = new System.Xml.XmlTextReader(stream)) {
                var define = global::ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(reader, instance);
                editor.SyntaxHighlighting = define;
            }
        }

        #endregion

        private void EnvUpdateEditor_Loaded(object sender, RoutedEventArgs e)
        {
            using(var stream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(Properties.Resources.File_Highlighting_EnvironmentVariable_Update))) {
                SetSyntaxHighlighting((ICSharpCode.AvalonEdit.TextEditor)sender, stream);
            }
        }

        private void EnvRemoveEditor_Loaded(object sender, RoutedEventArgs e)
        {
            using(var stream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(Properties.Resources.File_Highlighting_EnvironmentVariable_Remove))) {
                SetSyntaxHighlighting((ICSharpCode.AvalonEdit.TextEditor)sender, stream);
            }
        }

        private void TagEditor_Loaded(object sender, RoutedEventArgs e)
        {
            using(var stream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(Properties.Resources.File_Highlighting_Tag))) {
                SetSyntaxHighlighting((ICSharpCode.AvalonEdit.TextEditor)sender, stream);
            }
        }
    }
}
