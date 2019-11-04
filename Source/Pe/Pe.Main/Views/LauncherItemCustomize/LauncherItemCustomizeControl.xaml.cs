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
        }

        #region Item

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Item),
            typeof(LauncherItemCustomizeViewModel),
            typeof(LauncherItemCustomizeControl),
            new FrameworkPropertyMetadata(
                default(LauncherItemCustomizeViewModel),
                new PropertyChangedCallback(OnItemChanged)
            )
        );

        public LauncherItemCustomizeViewModel Item
        {
            get { return (LauncherItemCustomizeViewModel)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        private static void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is LauncherItemCustomizeControl control) {
            }
        }

        #endregion

        #region property

        CommandStore CommandStore { get; } = new CommandStore();

        #endregion

        #region command

        public ICommand FileSelectCommand => CommandStore.GetOrCreate(() => new DelegateCommand<RequestEventArgs>(
            o => {
                var fileSelectParameter = (LauncherFileSelectRequestParameter)o.Parameter;
                FileSystemDialogBase dialog = fileSelectParameter.IsFile switch
                {
                    true => new OpenFileDialog(),
                    false => new FolderBrowserDialog(),
                };
                using(dialog) {
                    dialog.FileName = fileSelectParameter.FilePath;
                    dialog.Filters.SetRange(fileSelectParameter.Filter);

                    if(dialog.ShowDialog(Window.GetWindow(this)).GetValueOrDefault()) {
                        o.Callback(new LauncherFileSelectRequestResponse() {
                            ResponseIsCancel = false,
                            ResponseFilePaths = new[] { dialog.FileName },
                        });
                    } else {
                        o.Callback(new LauncherFileSelectRequestResponse() {
                            ResponseIsCancel = true,
                        });
                    }
                }
            }
        ));

        public ICommand IconSelectCommand => CommandStore.GetOrCreate(() => new DelegateCommand<RequestEventArgs>(
            o => {
                var iconSelectParameter = (LauncherIconSelectRequestParameter)o.Parameter;
                var dialog = new IconDialog();
                dialog.IconPath = iconSelectParameter.FileName;
                dialog.IconIndex = iconSelectParameter.IconIndex;
                if(dialog.ShowDialog().GetValueOrDefault()) {
                    o.Callback(new LauncherIconSelectRequestResponse() {
                        ResponseIsCancel = false,
                        FileName = dialog.IconPath,
                        IconIndex = dialog.IconIndex,
                    });
                } else {
                    o.Callback(new LauncherIconSelectRequestResponse() {
                        ResponseIsCancel = true,
                    });
                }
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
