using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.ViewModels.Plugin;
using ContentTypeTextNet.Pe.Main.ViewModels.Setting;
using ContentTypeTextNet.Pe.Main.Views.Plugin;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.Setting
{
    /// <summary>
    /// PluginsSettingControl.xaml の相互作用ロジック
    /// </summary>
    public partial class PluginsSettingControl: UserControl
    {
        public PluginsSettingControl()
        {
            InitializeComponent();
            DialogRequestReceiver = new DialogRequestReceiver(this);
        }

        #region property

        private DialogRequestReceiver DialogRequestReceiver { get; }

        #endregion

        #region command

        private ICommand? _SelectPluginFileCommand;
        public ICommand SelectPluginFileCommand => this._SelectPluginFileCommand ??= new DelegateCommand<RequestEventArgs>(
            o => {
                DialogRequestReceiver.ReceiveFileSystemSelectDialogRequest(o);
            }
        );

        private ICommand? _ShowMessageCommand;
        public ICommand ShowMessageCommand => this._ShowMessageCommand ??= new DelegateCommand<RequestEventArgs>(
             o => {
                 var parameter = (CommonMessageDialogRequestParameter)o.Parameter;
                 var result = MessageBox.Show(UIUtility.GetLogicalClosest<Window>(this)!, parameter.Message, parameter.Caption, parameter.Button, parameter.Icon, parameter.DefaultResult, parameter.Options);
             }
         );

        private ICommand? _WebInstallCommand;
        public ICommand WebInstallCommand => this._WebInstallCommand ??= new DelegateCommand<RequestEventArgs>(
            o => {
                using var parameter = (PluginWebInstallRequestParameter)o.Parameter;

                var viewModel = new PluginWebInstallViewModel(
                    parameter.Element,
                    parameter.UserTracker,
                    parameter.DispatcherWrapper,
                    parameter.LoggerFactory
                );
                var dialog = new PluginWebInstallWindow() {
                    Owner = UIUtility.GetRootView(this),
                    DataContext = viewModel,
                };
                var windowItem = new WindowItem(WindowKind.PluginWebInstaller, parameter.Element, viewModel, dialog);
                parameter.WindowManager.Register(windowItem);
                dialog.ShowDialog();

                var hasPluginArchiveFile = parameter.Element.PluginArchiveFile is not null;
                var response = new PluginWebInstallRequestResponse() {
                    ResponseIsCancel = !hasPluginArchiveFile,
                };

                if(!response.ResponseIsCancel) {
                    response.ArchiveFile = parameter.Element.GetArchiveFile();
                }

                o.Callback(response);
            }
        );

        #endregion

        #region Editor

        public static readonly DependencyProperty EditorProperty = DependencyProperty.Register(
            nameof(Editor),
            typeof(PluginsSettingEditorViewModel),
            typeof(PluginsSettingControl),
            new FrameworkPropertyMetadata(
                default(PluginsSettingEditorViewModel),
                new PropertyChangedCallback(OnEditorChanged)
            )
        );

        public PluginsSettingEditorViewModel Editor
        {
            get { return (PluginsSettingEditorViewModel)GetValue(EditorProperty); }
            set { SetValue(EditorProperty, value); }
        }

        private static void OnEditorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is PluginsSettingControl control) {
            }
        }

        #endregion
    }
}
