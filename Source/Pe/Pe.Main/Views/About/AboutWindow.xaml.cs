using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using Prism.Commands;

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

        private ICommand? _OpenCommonMessageDialogCommand;
        public ICommand OpenCommonMessageDialogCommand => this._OpenCommonMessageDialogCommand ??= new DelegateCommand<RequestEventArgs>(
            o => {
                var parameter = (CommonMessageDialogRequestParameter)o.Parameter;
                MessageBox.Show(Window.GetWindow(this), parameter.Message, parameter.Caption, parameter.Button, parameter.Icon, parameter.DefaultResult, parameter.Options);
            }
        );

        #endregion
    }
}
