using System.Windows;
using Forms = System.Windows.Forms;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using Prism.Commands;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;

namespace ContentTypeTextNet.Pe.Main.Views.About
{
    /// <summary>
    /// AboutWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AboutWindow: Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            DialogRequestReceiver = new DialogRequestReceiver(this);
        }

        #region property

        DialogRequestReceiver DialogRequestReceiver { get; }

        #endregion

        #region command

        private ICommand? _CloseCommand;
        public ICommand CloseCommand => this._CloseCommand ??= new DelegateCommand(
            () => Close()
        );

        private ICommand? _FileSelectCommand;
        public ICommand FileSelectCommand => this._FileSelectCommand ??= new DelegateCommand<RequestEventArgs>(
            o => {
                DialogRequestReceiver.ReceiveFileSystemSelectDialogRequest(o);
            }
        );

        private ICommand? _OutputSettingCommand;
        public ICommand OutputSettingCommand => this._OutputSettingCommand ??= new DelegateCommand<RequestEventArgs>(
            o => {
                DialogRequestReceiver.ReceiveFileSystemSelectDialogRequest(o);
            }
        );

        private ICommand? _OpenCommonMessageDialogCommand;
        public ICommand OpenCommonMessageDialogCommand => this._OpenCommonMessageDialogCommand ??= new DelegateCommand<RequestEventArgs>(
            o => {
                var parameter = (CommonMessageDialogRequestParameter)o.Parameter;
                Forms.TaskDialog.ShowDialog(
                    HandleUtility.GetWindowHandle(this),
                    parameter.ToTaskDialogPage(),
                    Forms.TaskDialogStartupLocation.CenterOwner
                );
            }
        );

        #endregion
    }
}
