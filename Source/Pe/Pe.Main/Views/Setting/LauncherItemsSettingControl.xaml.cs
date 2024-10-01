using System.Windows;
using System.Windows.Controls;
using Forms = System.Windows.Forms;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.ViewModels.Setting;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.Setting
{
    /// <summary>
    /// LauncherItemsSettingControl.xaml の相互作用ロジック
    /// </summary>
    public partial class LauncherItemsSettingControl: UserControl
    {
        public LauncherItemsSettingControl()
        {
            InitializeComponent();
        }

        #region Editor

        public static readonly DependencyProperty EditorProperty = DependencyProperty.Register(
            nameof(Editor),
            typeof(LauncherItemsSettingEditorViewModel),
            typeof(LauncherItemsSettingControl),
            new FrameworkPropertyMetadata(
                default(LauncherItemsSettingEditorViewModel),
                new PropertyChangedCallback(OnEditorChanged)
            )
        );

        public LauncherItemsSettingEditorViewModel Editor
        {
            get { return (LauncherItemsSettingEditorViewModel)GetValue(EditorProperty); }
            set { SetValue(EditorProperty, value); }
        }

        private static void OnEditorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is LauncherItemsSettingControl control) {
            }
        }

        #endregion

        #region property

        #endregion

        #region command

        private ICommand? _ScrollSelectedItemCommand;
        public ICommand ScrollSelectedItemCommand => this._ScrollSelectedItemCommand ??= new DelegateCommand<RequestEventArgs>(
            o => {
                var listItems = (ListBox)FindName("items");
                listItems.ScrollIntoView(Editor.SelectedItem);
            }
        );

        private ICommand? _ScrollToTopCustomizeCommand;
        public ICommand ScrollToTopCustomizeCommand => this._ScrollToTopCustomizeCommand ??= new DelegateCommand<RequestEventArgs>(
            o => {
                var scrollCustomize = (ScrollViewer)FindName("customize");
                scrollCustomize.ScrollToTop();
            }
        );

        private ICommand? _OpenCommonMessageDialogCommand;
        public ICommand OpenCommonMessageDialogCommand => this._OpenCommonMessageDialogCommand ??= new DelegateCommand<RequestEventArgs>(
            o => {
                var parameter = (CommonMessageDialogRequestParameter)o.Parameter;
                var result = Forms.TaskDialog.ShowDialog(
                    HandleUtility.GetWindowHandle(Window.GetWindow(this)),
                    parameter.ToTaskDialogPage(),
                    Forms.TaskDialogStartupLocation.CenterOwner
                );
                o.Callback(new CommonMessageDialogRequestResponse() {
                    Result = result
                });
            }
        );

        #endregion
    }
}
