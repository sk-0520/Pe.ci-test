using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.ExtendsExecute
{
    /// <summary>
    /// ExtendsExecuteWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ExtendsExecuteWindow: Window, IDpiScaleOutpour
    {
        public ExtendsExecuteWindow()
        {
            InitializeComponent();
            DialogRequestReceiver = new DialogRequestReceiver(this);
        }


        #region property
        private DialogRequestReceiver DialogRequestReceiver { get; }

        [DiInjection]
        private ILogger? Logger { get; set; }

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

        #endregion

        #region IDpiScaleOutputor

        public Point GetDpiScale() => UIUtility.GetDpiScale(this);
        public IScreen GetOwnerScreen() => Screen.FromHandle(HandleUtility.GetWindowHandle(this));

        #endregion


    }
}
