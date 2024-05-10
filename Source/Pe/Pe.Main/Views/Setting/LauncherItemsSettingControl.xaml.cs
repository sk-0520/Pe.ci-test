using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
                var result = MessageBox.Show(Window.GetWindow(this), parameter.Message, parameter.Caption, parameter.Button, parameter.Icon, parameter.DefaultResult, parameter.Options);
                var response = new YesNoResponse();
                switch(result) {
                    case MessageBoxResult.Yes:
                        response.ResponseIsCancel = false;
                        response.ResponseIsYes = true;
                        break;

                    case MessageBoxResult.No:
                        response.ResponseIsCancel = false;
                        response.ResponseIsYes = false;
                        break;

                    default:
                        response.ResponseIsCancel = true;
                        response.ResponseIsYes = false;
                        break;
                }

                o.Callback(response);
            }
        );

        #endregion
    }
}
