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
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Views;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.LauncherItemCustomize
{
    /// <summary>
    /// LauncherItemCustomizeControl.xaml の相互作用ロジック
    /// </summary>
    public partial class LauncherItemCustomizeControl : UserControl
    {
        public LauncherItemCustomizeControl()
        {
            InitializeComponent();
            DialogRequestReceiver = new DialogRequestReceiver(this);
        }

        #region Item

        public static readonly DependencyProperty EditorProperty = DependencyProperty.Register(
            nameof(Editor),
            typeof(LauncherItemCustomizeEditorViewModel),
            typeof(LauncherItemCustomizeControl),
            new FrameworkPropertyMetadata(
                default(LauncherItemCustomizeEditorViewModel),
                new PropertyChangedCallback(OnEditorChanged)
            )
        );

        public LauncherItemCustomizeEditorViewModel Editor
        {
            get { return (LauncherItemCustomizeEditorViewModel)GetValue(EditorProperty); }
            set { SetValue(EditorProperty, value); }
        }

        private static void OnEditorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is LauncherItemCustomizeControl control) {
            }
        }

        #endregion

        #region property

        DialogRequestReceiver DialogRequestReceiver { get; }
        CommandStore CommandStore { get; } = new CommandStore();

        #endregion

        #region command

        public ICommand FileSelectCommand => CommandStore.GetOrCreate(() => new DelegateCommand<RequestEventArgs>(
            o => {
                DialogRequestReceiver.ReceiveFileSystemSelectDialogRequest(o);
            }
        ));

        public ICommand IconSelectCommand => CommandStore.GetOrCreate(() => new DelegateCommand<RequestEventArgs>(
            o => {
                DialogRequestReceiver.ReceiveIconSelectDialogRequest(o);
            }
        ));

        #endregion

        #region function

        #endregion

        private void TagEditor_Loaded(object sender, RoutedEventArgs e)
        {
            using(var stream = ResourceUtility.OpenSyntaxStream(Properties.Resources.File_Highlighting_Tag)) {
                AvalonEditHelper.SetSyntaxHighlightingDefault((ICSharpCode.AvalonEdit.TextEditor)sender, stream);
            }
        }
    }
}
